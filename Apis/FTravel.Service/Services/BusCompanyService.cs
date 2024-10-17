﻿//using AutoMapper;
//using FTravel.Repositories.Commons;
//using FTravel.Repository.Commons;
//using FTravel.Repository.EntityModels;
//using FTravel.Repository.Repositories;
//using FTravel.Repository.Repositories.Interface;
//using FTravel.Service.BusinessModels;
//using FTravel.Service.BusinessModels.BuscompanyModels;
//using FTravel.Service.Enums;
//using FTravel.Service.Services.Interface;
//using FTravel.Service.Utils;
//using Google.Apis.Util;

//namespace FTravel.Service.Services
//{
//    public class BusCompanyService : IBusCompanyService
//    {
//        private readonly IBusCompanyRepository _busRepository;
//        private readonly IUserRepository _userRepository;
//        private readonly IRoleRepository _roleRepository;
//        private readonly IMapper _mapper;

//        public BusCompanyService(IBusCompanyRepository busRepository, IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
//        {
//            _busRepository = busRepository;
//            _userRepository = userRepository;
//            _roleRepository = roleRepository;
//            _mapper = mapper;
//        }
//        public async Task<bool> AddBusCompanyAsync(CreateBusCompanyModel model)
//        {
//            try
//            {   var user = await _userRepository.GetUserByEmailAsync(model.ManagerEmail);
//                if (user == null)
//                {
//                    throw new KeyNotFoundException("Tài khoản không tồn tại!");
//                }
//                var role = await _roleRepository.GetByIdAsync(user.RoleId.Value);
//                if (role.Name != RoleEnums.BUSCOMPANY.ToString())
//                {
//                    throw new ArgumentException("Tài khoản không phải là tài khoản của quản lý!");
//                }
//                var existedManager = await _busRepository.GetBusCompanyByManagerEmail(model.ManagerEmail);
//                if (existedManager != null)
//                {
//                    throw new ArgumentException($"Tài khoản đã là quản lý của nhà xe: {existedManager.Name}!");
//                }
//                var busCompany = _mapper.Map<BusCompany>(model);

//                await _busRepository.AddAsync(busCompany);

//                return true;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//                return false;
//            }
//        }

//        public async Task<int> BusCompanySoftDelete(int busCompanyId)
//        {
//            var bcSoftDelete = await _busRepository.SoftDeleteBusCompany(busCompanyId);
//            return bcSoftDelete;
//        }

//        public async Task<Pagination<BusCompany>> GetAllBusCompanies(PaginationParameter paginationParameter)
//        {

//            return await _busRepository.GetAllBusCompanies(paginationParameter);
//        }

//        public async Task<BusCompany> GetBusCompanyById(int id)
//        {
//            return await _busRepository.GetBusCompanyDetailById(id);
//        }

//        public async Task<bool> UpdateBusCompanyAsync(int id, UpdateBusCompanyModel busCompany)
//        {
//            try
//            {
//                var existingBusCompany = await _busRepository.GetBusCompanyDetailById(id);
//                if (existingBusCompany == null)
//                {
//                    return false;
//                }

//                _mapper.Map(busCompany, existingBusCompany);
//                await _busRepository.UpdateAsync(existingBusCompany);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                // Log the exception
//                throw new Exception("Xảy ra lỗi khi cập nhật nhà xe");
//                return false;
//            }
//        }
//    }
//}
