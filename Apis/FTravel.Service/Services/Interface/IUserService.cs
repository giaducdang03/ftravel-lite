﻿using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.BusinessModels.AccountModels;
using FTravel.Service.BusinessModels.AuthenModels;
using FTravel.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IUserService
    {
        public Task<AuthenModel> LoginByEmailAndPassword(string email, string password);

        public Task<bool> RegisterAsync(SignUpModel model);

        public Task<AuthenModel> RefreshToken(string jwtToken);

        public Task<AuthenModel> ConfirmEmail(ConfirmOtpModel confirmOtpModel);

        public Task<bool> RequestResetPassword(string email);

        public Task<bool> ConfirmResetPassword(ConfirmOtpModel confirmOtpModel);

        public Task<bool> ExecuteResetPassword(ResetPasswordModel resetPasswordModel);

        public Task<bool> ChangePasswordAsync(string email, ChangePasswordModel changePasswordModel);

        public Task<AuthenModel> LoginWithGoogle(string credental);

        public Task<UserModel> GetLoginUserInformationAsync(string email);

        public Task<List<User>> GetUsersByRoleAsync(RoleEnums roleEnums);

        public Task<User> GetUserByEmailAsync(string email);

        public Task<User> GetUserByIdAsync(int userId);

        public Task<List<User>> GetUsersByUserIdsAsync(List<int> userIds);

        // user manager

        public Task<Pagination<UserModel>> GetAllUsersAsync(PaginationParameter paginationParameter);

        Task<bool> CreateAccountAsync(CreateAccountModel model);

        public Task<bool> UpdateFcmTokenAsync(string email, string fcmToken);

        public Task<bool> DeleteAccountAsync(int id, string currentEmail);

        Task<bool> UpdateAccount(UpdateAccountModel accountModel);
    }
}
