using AutoMapper;
using ECommerceApp.Application.Dto;
using ECommerceApp.Application.Dto.Identity;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Services.Interfaces.Authentication;
using ECommerceApp.Application.Services.Interfaces.Logging;
using ECommerceApp.Application.Validation;
using ECommerceApp.Application.Validation.Authentication;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Interface.Authentication;
using ECommerceApp.Domain.Interface.Authentication.ECommerceApp.Domain.Interface.Authentication;
using FluentValidation;

namespace ECommerceApp.Application.Services.Implementation.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenManagement _tokenManagement;
        private readonly IUserManagement _userManagement;
        private readonly IRoleManagement _roleManagement;
        private readonly IAppLogger<AuthenticationService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        private readonly IValidator<CreateUser> _createUserValidator;
        private readonly LoginUserValidator _loginUserValidator;

        public AuthenticationService(
            ITokenManagement tokenManagement,
            IUserManagement userManagement,
            IRoleManagement roleManagement,
            IAppLogger<AuthenticationService> logger,
            IMapper mapper,
            IValidationService validationService,
            IValidator<CreateUser> createUserValidator,
            LoginUserValidator loginUserValidator)
        {
            _tokenManagement = tokenManagement;
            _userManagement = userManagement;
            _roleManagement = roleManagement;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _createUserValidator = createUserValidator;
            _loginUserValidator = loginUserValidator;
        }

        // ✅ إنشاء مستخدم جديد
        public async Task<ServiceResponse> CreateUser(CreateUser user)
        {
            var validationResult = await _validationService.ValidateAsync(user, _createUserValidator);
            if (!validationResult.Success)
                return validationResult;

            var mappedUser = _mapper.Map<AppUser>(user);
            mappedUser.UserName = user.Email;
            mappedUser.PasswordHash = user.Password;

            var result = await _userManagement.CreateUser(mappedUser);
            if (!result)
                return new ServiceResponse { Message = "Email address might already be in use or an unknown error occurred" };

            var createdUser = await _userManagement.GetUserByEmail(user.Email);
            var users = await _userManagement.GetAllUsers();
            bool assignResult = await _roleManagement.AddUserToRole(createdUser, users.Count() > 1 ? "User" : "Admin");

            if (!assignResult)
            {
                int removeResult = await _userManagement.RemoveUserByEmail(user.Email);
                if (removeResult <= 0)
                {
                    _logger.LogError(new Exception($"User {createdUser.Email} failed to be removed due to role assignment error"),
                                     "Error occurred while removing user");
                    return new ServiceResponse { Message = "Error occurred while creating account" };
                }
            }

            return new ServiceResponse { Success = true, Message = "User created successfully" };
        }

        // ✅ تسجيل الدخول وتوليد التوكين
        public async Task<LoginResponse> LoginUser(LoginUser user)
        {
            var validationResult = await _validationService.ValidateAsync(user, _loginUserValidator);
            if (!validationResult.Success)
                return new LoginResponse { Message = validationResult.Message };

            var mappedUser = _mapper.Map<AppUser>(user);
            mappedUser.PasswordHash = user.Password;

            bool loginResult = await _userManagement.LogUser(mappedUser);
            if (!loginResult)
                return new LoginResponse(Message: "Email not found or invalid password");

            var _user = await _userManagement.GetUserByEmail(user.Email);
            var claims = await _userManagement.GetUserClaim(_user.Email!);

            string jwtToken = _tokenManagement.GenerateToken(claims);
            string refreshToken = _tokenManagement.GetRefreshToken();

            // ✅ التحقق من وجود توكن للمستخدم مش من قيمة التوكن الجديدة
            bool hasToken = await _tokenManagement.ValidateRefreshTokenForUser(_user.Id);
            int saveTokenResult = hasToken
                ? await _tokenManagement.UpdateRefreshToken(_user.Id, refreshToken)
                : await _tokenManagement.AddRefreshToken(_user.Id, refreshToken);

            return saveTokenResult <= 0
                ? new LoginResponse(Message: "Internal error occurred while saving token")
                : new LoginResponse(Success: true, Token: jwtToken, RefreshToken: refreshToken);
        }

        // ✅ تجديد التوكين (Refresh Token)
        public async Task<LoginResponse> ReviveToken(string refreshToken)
        {
            bool validateToken = await _tokenManagement.ValidateRefreshToken(refreshToken);
            if (!validateToken)
                return new LoginResponse { Success = false, Message = "Invalid or expired refresh token" };

            string? userId = await _tokenManagement.GetUserIdByRefreshToken(refreshToken);
            if (string.IsNullOrEmpty(userId))
                return new LoginResponse { Success = false, Message = "User not found for this token" };

            var user = await _userManagement.GetUserById(userId);
            if (user == null)
                return new LoginResponse { Success = false, Message = "User not found" };

            var claims = await _userManagement.GetUserClaim(user.Email!);
            string newJwtToken = _tokenManagement.GenerateToken(claims);
            string newRefreshToken = _tokenManagement.GetRefreshToken();

            await _tokenManagement.UpdateRefreshToken(userId, newRefreshToken);

            return new LoginResponse
            {
                Success = true,
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
