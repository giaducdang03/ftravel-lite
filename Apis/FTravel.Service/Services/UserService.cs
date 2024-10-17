using AutoMapper;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.BusinessModels.AccountModels;
using FTravel.Service.BusinessModels.AuthenModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using FTravel.Service.Utils.Email;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FTravel.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;
        private readonly IMailService _mailService;
        private readonly IWalletRepository _walletRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
            IOtpService otpService,
            IMailService mailService,
            IWalletRepository walletRepository,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _otpService = otpService;
            _mailService = mailService;
            _walletRepository = walletRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthenModel> LoginByEmailAndPassword(string email, string password)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            {
                try
                {
                    var existUser = await _userRepository.GetUserByEmailAsync(email);
                    if (existUser == null)
                    {
                        return new AuthenModel
                        {
                            HttpCode = 401,
                            Message = "Tài khoản không tồn tại."
                        };
                    }
                    var verifyUser = PasswordUtils.VerifyPassword(password, existUser.PasswordHash);
                    if (verifyUser)
                    {
                        // check status user
                        if (existUser.Status == UserStatus.BANNED.ToString() || existUser.IsDeleted == true)
                        {
                            return new AuthenModel
                            {
                                HttpCode = 401,
                                Message = "Tài khoản đã bị cấm."
                            };
                        }

                        if (existUser.ConfirmEmail == false)
                        {
                            // send otp email
                            await _otpService.CreateOtpAsync(existUser.Email, "confirm");

                            await transaction.CommitAsync();

                            return new AuthenModel
                            {
                                HttpCode = 401,
                                Message = "Bạn phải xác nhận email trước khi đăng nhập vào hệ thống. OTP đã gửi qua email."
                            };
                        }

                        var accessToken = await GenerateAccessToken(email, existUser);
                        var refreshToken = GenerateRefreshToken(email);

                        await transaction.CommitAsync();

                        return new AuthenModel
                        {
                            HttpCode = 200,
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        };
                    }
                    return new AuthenModel
                    {
                        HttpCode = 401,
                        Message = "Sai mật khẩu."
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

        }

        public async Task<AuthenModel> RefreshToken(string jwtToken)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = authSigningKey,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                SecurityToken validatedToken;
                var principal = handler.ValidateToken(jwtToken, validationParameters, out validatedToken);
                var email = principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                if (email != null)
                {
                    var existUser = await _userRepository.GetUserByEmailAsync(email);
                    if (existUser != null)
                    {
                        var accessToken = await GenerateAccessToken(email, existUser);
                        var refreshToken = GenerateRefreshToken(email);
                        return new AuthenModel
                        {
                            HttpCode = 200,
                            Message = "Refresh token successfully.",
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        };
                    }
                }
                return new AuthenModel
                {
                    HttpCode = 401,
                    Message = "Tài khoản không tồn tại."
                };
            }
            catch
            {
                throw new Exception("Token không hợp lệ");
            }

        }

        public async Task<bool> RegisterAsync(SignUpModel model)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            {
                try
                {
                    // add wallet
                    Wallet userWallet = new Wallet
                    {
                        AccountBalance = 0,
                        Status = WalletStatus.ACTIVE.ToString(),
                    };

                    User newUser = new User()
                    {
                        Email = model.Email,
                        FullName = model.FullName,
                        UnsignFullName = StringUtils.ConvertToUnSign(model.FullName),
                        Status = UserStatus.ACTIVE.ToString(),
                        Role = RoleEnums.USER.ToString(),
                        Wallet = userWallet,
                    };

                    var existUser = await _userRepository.GetUserByEmailAsync(model.Email);

                    if (existUser != null)
                    {
                        throw new Exception("Tài khoản đã tồn tại.");
                    }

                    // hash password
                    newUser.PasswordHash = PasswordUtils.HashPassword(model.Password);

                    await _userRepository.AddAsync(newUser);


                    // send otp email
                    await _otpService.CreateOtpAsync(newUser.Email, "confirm");

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

        }

        public async Task<AuthenModel> ConfirmEmail(ConfirmOtpModel confirmOtpModel)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            {
                try
                {
                    bool checkOtp = await _otpService.ValidateOtpAsync(confirmOtpModel.Email, confirmOtpModel.OtpCode);

                    if (checkOtp)
                    {
                        // return accesstoken

                        var existUser = await _userRepository.GetUserByEmailAsync(confirmOtpModel.Email);

                        if (existUser == null)
                        {
                            return new AuthenModel
                            {
                                HttpCode = 401,
                                Message = "Tài khoản không tồn tại."
                            };
                        }

                        // update confirm email for user
                        existUser.ConfirmEmail = true;
                        await _userRepository.UpdateAsync(existUser);

                        var accessToken = await GenerateAccessToken(confirmOtpModel.Email, existUser);
                        var refreshToken = GenerateRefreshToken(confirmOtpModel.Email);

                        await transaction.CommitAsync();

                        return new AuthenModel
                        {
                            HttpCode = 200,
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        };
                    }

                    return new AuthenModel
                    {
                        HttpCode = 401,
                        Message = "OTP không hợp lệ."
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> RequestResetPassword(string email)
        {
            var existUser = await _userRepository.GetUserByEmailAsync(email);

            if (existUser != null)
            {
                if (existUser.ConfirmEmail == true)
                {
                    await _otpService.CreateOtpAsync(email, "reset");
                    return true;
                }
            }
            else
            {
                throw new Exception("Tài khoản không tồn tại.");
            }
            return false;
        }

        public async Task<bool> ConfirmResetPassword(ConfirmOtpModel confirmOtpModel)
        {
            return await _otpService.ValidateOtpAsync(confirmOtpModel.Email, confirmOtpModel.OtpCode);
        }

        public async Task<bool> ExecuteResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var user = await _userRepository.GetUserByEmailAsync(resetPasswordModel.Email);
            if (user != null)
            {
                user.PasswordHash = PasswordUtils.HashPassword(resetPasswordModel.Password);
                await _userRepository.UpdateAsync(user);
                return true;
            }
            else
            {
                throw new Exception("Tài khoản không tồn tại.");
            }
        }

        public async Task<bool> ChangePasswordAsync(string email, ChangePasswordModel changePasswordModel)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user != null)
            {
                bool checkPassword = PasswordUtils.VerifyPassword(changePasswordModel.OldPassword, user.PasswordHash);
                if (checkPassword)
                {
                    user.PasswordHash = PasswordUtils.HashPassword(changePasswordModel.NewPassword);
                    await _userRepository.UpdateAsync(user);
                    return true;
                }
                else
                {
                    throw new Exception("Mật khẩu cũ không đúng.");
                }
            }
            else
            {
                throw new Exception("Tài khoản không tồn tại.");
            }
        }

        public async Task<AuthenModel> LoginWithGoogle(string credental)
        {
            string cliendId = _configuration["GoogleCredential:ClientId"];

            if (string.IsNullOrEmpty(cliendId))
            {
                throw new Exception("ClientId is null");
            }

            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { cliendId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credental, settings);
            if (payload == null)
            {
                throw new Exception("Credental không hợp lệ.");
            }

            var existUser = await _userRepository.GetUserByEmailAsync(payload.Email);

            if (existUser != null && existUser.GoogleId != null)
            {
                var roleUser = existUser.Role;

                if (roleUser != RoleEnums.USER.ToString())
                {
                    throw new Exception("Tài khoản của bạn không được phép đăng nhập với Google.");
                }

                if (existUser.Status == UserStatus.BANNED.ToString())
                {
                    throw new Exception("Tài khoản đã bị cấm.");
                }
                else
                {
                    // create accesstoken
                    var accessToken = await GenerateAccessToken(existUser.Email, existUser);
                    var refreshToken = GenerateRefreshToken(existUser.Email);

                    return new AuthenModel()
                    {
                        HttpCode = 200,
                        Message = "Login with Google sucessfully",
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    };
                }
            }
            else
            {
                // create new user

                using (var transaction = await _userRepository.BeginTransactionAsync())
                {
                    try
                    {
                        // add wallet
                        Wallet USERWallet = new Wallet
                        {
                            AccountBalance = 0,
                            Status = WalletStatus.ACTIVE.ToString(),
                        };

                        User newUser = new User()
                        {
                            Email = payload.Email,
                            FullName = payload.Name,
                            ConfirmEmail = true,
                            UnsignFullName = StringUtils.ConvertToUnSign(payload.Name),
                            AvatarUrl = payload.Picture,
                            Status = UserStatus.ACTIVE.ToString(),
                            GoogleId = payload.JwtId,
                            Wallet = USERWallet
                        };

                        await _userRepository.AddAsync(newUser);

                        await transaction.CommitAsync();

                        // create accesstoken
                        var accessToken = await GenerateAccessToken(newUser.Email, newUser);
                        var refreshToken = GenerateRefreshToken(newUser.Email);

                        return new AuthenModel()
                        {
                            HttpCode = 200,
                            Message = "Login with Google sucessfully",
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        };
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<List<User>> GetUsersByRoleAsync(RoleEnums roleEnums)
        {
            var users = await _userRepository.GetAllAsync();
            return users.Where(x => x.Role == roleEnums.ToString()).ToList();
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }
        public async Task<List<User>> GetUsersByUserIdsAsync(List<int> userIds)
        {
            var users = await _userRepository.GetAllAsync();
            return users.Where(x => userIds.Contains(x.Id)).ToList();
        }

        private async Task<string> GenerateAccessToken(string email, User user)
        {
            var authClaims = new List<Claim>();

            if (user.Role != null)
            {
                authClaims.Add(new Claim(ClaimTypes.Email, email));
                authClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                authClaims.Add(new Claim(ClaimTypes.Role, user.Role));
                //authClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            }
            var accessToken = GenerateJWTToken.CreateToken(authClaims, _configuration, DateTime.UtcNow);
            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }

        private string GenerateRefreshToken(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
            };
            var refreshToken = GenerateJWTToken.CreateRefreshToken(claims, _configuration, DateTime.UtcNow);
            return new JwtSecurityTokenHandler().WriteToken(refreshToken).ToString();
        }

        public async Task<UserModel> GetLoginUserInformationAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user != null)
            {
                UserModel userModel = _mapper.Map<UserModel>(user);
                return userModel;
            }
            return null;
        }

        public async Task<Pagination<AccountModel>> GetAllUsersAsync(PaginationParameter paginationParameter)
        {
            var users = await _userRepository.ToPagination(paginationParameter);
            var accountModels = _mapper.Map<List<AccountModel>>(users);
            return new Pagination<AccountModel>(accountModels,
                users.TotalCount,
                users.CurrentPage,
                users.PageSize);
        }

        public async Task<bool> CreateAccountAsync(CreateAccountModel model)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            {
                try
                {
                    User newUser = _mapper.Map<User>(model);
                    newUser.Status = UserStatus.ACTIVE.ToString();
                    newUser.UnsignFullName = StringUtils.ConvertToUnSign(model.FullName);
                    newUser.Role = model.Role.ToString();

                    var existUser = await _userRepository.GetUserByEmailAsync(model.Email);

                    if (existUser != null)
                    {
                        throw new Exception("Tài khoản đã tồn tại.");
                    }

                    if (model.Role.ToString() == RoleEnums.USER.ToString())
                    {
                        // add wallet
                        Wallet USERWallet = new Wallet
                        {
                            AccountBalance = 0,
                            Status = WalletStatus.ACTIVE.ToString(),
                        };
                        newUser.Wallet = USERWallet;
                    }

                    // generate password
                    string password = PasswordUtils.GeneratePassword();

                    // hash password
                    newUser.PasswordHash = PasswordUtils.HashPassword(password);

                    await _userRepository.AddAsync(newUser);

                    // send email password
                    MailRequest passwordEmail = new MailRequest()
                    {
                        ToEmail = model.Email,
                        Subject = "FTravel Welcome",
                        Body = EmailCreateAccount.EmailSendCreateAccount(model.Email, password)
                    };

                    await _mailService.SendEmailAsync(passwordEmail);

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> DeleteAccountAsync(int id, string currentEmail)
        {
            var account = await _userRepository.GetByIdAsync(id);
            if (account != null)
            {
                // check current user
                if (account.Email == currentEmail)
                {
                    throw new Exception("Tài khoản đang đăng nhập. Không thể xóa.");
                }

                // check confirm email
                if (account.ConfirmEmail == true)
                {
                    account.Status = UserStatus.BANNED.ToString();
                    await _userRepository.SoftDeleteAsync(account);
                    return true;
                }
                else
                {
                    using (var transaction = await _userRepository.BeginTransactionAsync())
                    {
                        try
                        {
                            var accountRole = account.Role;
                            if (accountRole != null)
                            {
                                // if USER delete USER and wallet
                                if (accountRole == RoleEnums.USER.ToString())
                                {
                                    var userWallet = await _walletRepository.GetWalletByUserId(account.Id);
                                    if (userWallet != null)
                                    {
                                        await _walletRepository.PermanentDeletedAsync(userWallet);
                                        await _userRepository.PermanentDeletedAsync(account);
                                        await transaction.CommitAsync();
                                        return true;
                                    }
                                }
                                else
                                {
                                    await _userRepository.PermanentDeletedAsync(account);
                                    await transaction.CommitAsync();
                                    return true;
                                }
                            }
                        }
                        catch
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }

                }
            }
            throw new Exception("Tài khoản không tồn tại.");
        }

        public async Task<bool> UpdateFcmTokenAsync(string email, string fcmToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user != null && !fcmToken.IsNullOrEmpty())
            {
                if (user.Fcmtoken != fcmToken)
                {
                    user.Fcmtoken = fcmToken;
                    var result = await _userRepository.UpdateAsync(user);
                    return true ? result > 0 : false;
                }
            }
            return false;
        }

        public async Task<bool> UpdateAccount(UpdateAccountModel accountModel)
        {
            var existAccount = await _userRepository.GetByIdAsync(accountModel.AccountId);

            if (existAccount != null)
            {
                var newUnsignName = StringUtils.ConvertToUnSign(accountModel.FullName);

                // update account
                existAccount.FullName = accountModel.FullName;
                existAccount.UnsignFullName = newUnsignName;
                existAccount.PhoneNumber = accountModel.PhoneNumber;
                existAccount.Dob = accountModel.Dob;
                existAccount.Address = accountModel.Address;
                existAccount.Gender = accountModel.Gender;
                if (!accountModel.AvatarUrl.IsNullOrEmpty())
                {
                    existAccount.AvatarUrl = accountModel.AvatarUrl;
                }

                await _userRepository.UpdateAsync(existAccount);
                return true;
            }
            else
            {
                throw new Exception("Tài khoản không tồn tại.");
            }
        }
    }
}
