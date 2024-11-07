using AutoMapper;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels.PaymentModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public WalletService(IWalletRepository walletRepository,
            ITransactionRepository transactionRepository,
            INotificationService notificationService,
            IUserService userService,
            IMapper mapper)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _notificationService = notificationService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<Pagination<WalletModel>> GetAllWalletsAsync(PaginationParameter paginationParameter)
        {
            var wallets = await _walletRepository.GetWalletPaginationAsync(paginationParameter);

            var walletModels = _mapper.Map<Pagination<WalletModel>>(wallets);

            return new Pagination<WalletModel>(walletModels,
                wallets.TotalCount,
                wallets.CurrentPage,
                wallets.PageSize);
        }

        public async Task<WalletModel> GetWalletByEmailAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user != null)
            {
                var customerWallet = await _walletRepository.GetWalletByUserId(user.Id);
                WalletModel walletModel = _mapper.Map<WalletModel>(customerWallet);
                return walletModel;
            }
            return null;
        }

        public async Task<string> RequestRechargeIntoWallet(string customerEmail, int amountRecharge, HttpContext context)
        {
            using (var dbTransaction = await _walletRepository.BeginTransactionAsync())
            {
                try
                {
                    if (amountRecharge > 5000)
                    {
                        throw new Exception("Bạn chỉ có thể nộp tối đa 5000 FTokens");
                    }

                    var customer = await _userService.GetUserByEmailAsync(customerEmail);
                    if (customer == null)
                    {
                        throw new Exception("Người dùng không tồn tại");
                    }
                    var customerWallet = await _walletRepository.GetWalletByUserId(customer.Id);
                    if (customerWallet == null)
                    {
                        throw new Exception("Ví người dùng không tồn tại");
                    }

                    // create new transaction

                    Transaction newTransaction = new Transaction()
                    {
                        Amount = amountRecharge,
                        WalletId = customerWallet.Id,
                        TransactionType = TransactionType.IN.ToString(),
                        Status = TransactionStatus.PENDING.ToString(),
                        Description = $"Nạp FToken vào ví {customer.UnsignFullName} từ VNPAY",
                        TrasactionCode = NumberUtils.GenerateSixDigitNumber()
                    };

                    var transactionAdded = await _transactionRepository.AddAsync(newTransaction);

                    // create URL payment

                    IConfiguration _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

                    DateTime timeNow = DateTime.UtcNow.AddHours(7);

                    // get ftoken value
                    // default = 1000
                    int ftokenValue = 1000;

                    int paymentPrice = amountRecharge * ftokenValue;

                    var ipAddress = VnPayUtils.GetIpAddress(context);

                    var pay = new VnPayLibrary();

                    pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
                    pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
                    pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
                    pay.AddRequestData("vnp_Amount", ((int)paymentPrice * 100).ToString());
                    pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
                    pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
                    pay.AddRequestData("vnp_IpAddr", ipAddress);
                    pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
                    pay.AddRequestData("vnp_OrderInfo", transactionAdded.Description);
                    pay.AddRequestData("vnp_OrderType", "250000");
                    pay.AddRequestData("vnp_TxnRef", transactionAdded.TrasactionCode.ToString());

                    // check server running
                    if (ipAddress == "::1")
                    {
                        pay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:UrlReturnLocal"]);
                    }
                    else
                    {
                        pay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:UrlReturnAzure"]);
                    }

                    var paymentUrl =
                        pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

                    await dbTransaction.CommitAsync();
                    return paymentUrl;
                }
                catch
                {
                    await dbTransaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> RechargeIntoWallet(VnPayModel vnPayResponse)
        {
            using (var dbTransaction = await _walletRepository.BeginTransactionAsync())
            {
                try
                {
                    IConfiguration _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

                    // get info transaction
                    if (vnPayResponse != null)
                    {
                        // check signature
                        var vnpay = new VnPayLibrary();

                        // get all data from vnpay model
                        foreach (PropertyInfo prop in vnPayResponse.GetType().GetProperties())
                        {
                            string name = prop.Name;
                            object value = prop.GetValue(vnPayResponse, null);
                            string valueStr = value?.ToString() ?? string.Empty;
                            vnpay.AddResponseData(name, valueStr);
                        }

                        var vnpayHashSecret = _configuration["Vnpay:HashSecret"];
                        bool validateSignature = vnpay.ValidateSignature(vnPayResponse.vnp_SecureHash, vnpayHashSecret);

                        int transactionCode = 0;
                        _ = int.TryParse(vnPayResponse.vnp_TxnRef, out transactionCode);
                        var updateTransaction = await _transactionRepository.GetTransactionsByCode(transactionCode);

                        if (updateTransaction != null)
                        {
                            if (validateSignature)
                            {
                                if (vnPayResponse.vnp_TransactionStatus == "00")
                                {
                                    updateTransaction.Status = TransactionStatus.SUCCESS.ToString();
                                    updateTransaction.TransactionDate = DateTime.ParseExact(vnPayResponse.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                                    // update wallet account balance

                                    // await UpdateAccountBalanceAsync(updateTransaction.WalletId.Value, TransactionType.IN, updateTransaction.Amount.Value);

                                    var updateWallet = await _walletRepository.GetWalletByIdAsync(updateTransaction.WalletId);
                                    if (updateWallet != null)
                                    {
                                        updateWallet.AccountBalance += updateTransaction.Amount;

                                        await _walletRepository.UpdateAsync(updateWallet);
                                    }

                                    await _transactionRepository.UpdateAsync(updateTransaction);

                                    await SendNotificationToUser(updateWallet.Id, updateTransaction.Amount);

                                    await dbTransaction.CommitAsync();
                                    return true;
                                }
                                else
                                {
                                    updateTransaction.Status = TransactionStatus.FAILED.ToString();
                                    updateTransaction.TransactionDate = DateTime.ParseExact(vnPayResponse.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                                    await _transactionRepository.UpdateAsync(updateTransaction);

                                    await dbTransaction.CommitAsync();
                                    return false;
                                }
                            }
                            else
                            {
                                updateTransaction.Status = TransactionStatus.FAILED.ToString();
                                updateTransaction.TransactionDate = DateTime.ParseExact(vnPayResponse.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                                await _transactionRepository.UpdateAsync(updateTransaction);

                                await dbTransaction.CommitAsync();
                                return false;
                            }

                        }
                        return false;
                    }
                    return false;
                }
                catch
                {
                    await dbTransaction.RollbackAsync();
                    throw;
                }
            }

        }

        public async Task<Wallet> GetWalletByCustomerIdAsync(int customerId)
        {
            return await _walletRepository.GetWalletByUserId(customerId);
        }

        public async Task<int> ExecutePaymentAsync(int walletId, TransactionType transactionType, int amount, int transactionId)
        {
            if (amount <= 0)
            {
                throw new Exception("Amount must > 0.");
            }
            // get wallet
            var wallet = await _walletRepository.GetWalletByIdAsync(walletId);
            if (wallet != null)
            {
                if (transactionType.ToString() == TransactionType.OUT.ToString())
                {
                    // check account balance
                    var currentAccountBalance = wallet.AccountBalance;
                    if (currentAccountBalance >= amount)
                    {
                        // update new account balance

                        currentAccountBalance -= amount;
                        wallet.AccountBalance = currentAccountBalance;
                        await _walletRepository.UpdateAsync(wallet);

                        // update transaction
                        var currentTransaction = await _transactionRepository.GetByIdAsync(transactionId);
                        if (currentTransaction != null)
                        {
                            currentTransaction.TransactionDate = TimeUtils.GetTimeVietNam();
                            currentTransaction.Status = TransactionStatus.SUCCESS.ToString();

                            await _transactionRepository.UpdateAsync(currentTransaction);

                            return walletId;
                        }
                    }
                    else
                    {
                        var currentTransaction = await _transactionRepository.GetByIdAsync(transactionId);
                        if (currentTransaction != null)
                        {
                            currentTransaction.TransactionDate = TimeUtils.GetTimeVietNam();
                            currentTransaction.Status = TransactionStatus.FAILED.ToString();

                            await _transactionRepository.UpdateAsync(currentTransaction);

                            return walletId;
                        }
                    }
                }
            }
            return 0;
        }

        public async Task<bool> CheckWalletPaymentAysnc(int walletId, int amount)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(walletId);
            if (wallet != null)
            {
                if (amount > 0 && wallet.AccountBalance >= amount)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> RefundToWalletAsync(int walletId, int amount, string message)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(walletId);
            if (wallet != null)
            {
                // create transaction
                Transaction newTransaction = new Transaction()
                {
                    Amount = amount,
                    WalletId = wallet.Id,
                    TransactionType = TransactionType.IN.ToString(),
                    Status = TransactionStatus.SUCCESS.ToString(),
                    Description = message,
                    TrasactionCode = NumberUtils.GenerateSixDigitNumber(),
                    TransactionDate = TimeUtils.GetTimeVietNam(),
                };

                await _transactionRepository.AddAsync(newTransaction);

                // update wallet
                wallet.AccountBalance += amount;
                await _walletRepository.UpdateAsync(wallet);
                await SendRefundNotificationToUser(wallet.Id, message);

                return true;
            }
            return false;
        }

        private async Task<bool> SendNotificationToUser(int walletId, int amount)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(walletId);
            if (wallet != null)
            {
                var customer = await _userService.GetUserByIdAsync(wallet.UserId.Value);
                if (customer != null)
                {
                    var user = await _userService.GetUserByEmailAsync(customer.Email);
                    if (user != null)
                    {
                        var newNoti = new Notification
                        {
                            EntityId = walletId,
                            Type = NotificationType.WALLET.ToString(),
                            Title = "Nạp tiền vào ví thành công",
                            Message = $"Bạn vừa nạp thành công {amount} ftokens vào ví từ VNPAY"
                        };
                        await _notificationService.AddNotificationByUserId(user.Id, newNoti);
                        if (user.Fcmtoken != null)
                        {
                            await _notificationService.PushMessagePaymentFirebase(newNoti.Title, newNoti.Message, user.Id);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<bool> SendRefundNotificationToUser(int walletId, string body)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(walletId);
            if (wallet != null)
            {
                var customer = await _userService.GetUserByIdAsync(wallet.UserId.Value);
                if (customer != null)
                {
                    var user = await _userService.GetUserByEmailAsync(customer.Email);
                    if (user != null)
                    {
                        var newNoti = new Notification
                        {
                            EntityId = walletId,
                            Type = NotificationType.WALLET.ToString(),
                            Title = "Hoàn tiền mua vé",
                            Message = $"Bạn vừa được {body}"
                        };
                        await _notificationService.AddNotificationByUserId(user.Id, newNoti);
                        if (user.Fcmtoken != null)
                        {
                            await _notificationService.PushMessagePaymentFirebase(newNoti.Title, newNoti.Message, user.Id);
                        }
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
