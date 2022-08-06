using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using AutoMapper;
using Domain.ConfigurationModels;
using Domain.Entities.Identities;
using Domain.Exceptions;
using Infrastructure.Contracts;
using Infrastructure.Utils.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepositoryManager _repository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly JwtConfiguration _jwtConfiguration;

        public AuthenticationService(IRepositoryManager repository, UserManager<User> userManager,
            IMapper mapper, ILoggerManager logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _jwtConfiguration = new JwtConfiguration();
            _configuration.Bind(_jwtConfiguration.Section, _jwtConfiguration);
        }

        public async Task<SuccessResponse<AuthDto>> Login(UserLoginDTO model)
        {
            var email = model.Email.Trim().ToLower();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                throw new RestException(HttpStatusCode.NotFound, "Wrong Email");

            var authenticated = await ValidateUser(user, model.Password);
            if (!authenticated)
                throw new RestException(HttpStatusCode.Unauthorized, "Wrong Email or Password");

            await _userManager.UpdateAsync(user);
            await _repository.SaveChangesAsync();

            var token = await CreateToken(user, true);
            return new SuccessResponse<AuthDto>
            {
                Data = token
            };
        }

        public async Task<SuccessResponse<UserDTO>> RegisterUser(UserCreateDTO model)
        {
            await IsEmailExist(model);
            var user = _mapper.Map<User>(model);
            user.UserName = user.Email;
            user.Password = _userManager.PasswordHasher.HashPassword(user, model.Password);

            var result = await _userManager.CreateAsync(user, user.Password);
            Guard.AgainstFailedTransaction(result.Succeeded);
            // add user to role
            if (!await _userManager.IsInRoleAsync(user, model.Role))
                await _userManager.AddToRoleAsync(user, model.Role);
            await _repository.SaveChangesAsync();
            var userResponse = _mapper.Map<UserDTO>(user);
            return new SuccessResponse<UserDTO>
            {
                Data = userResponse,
                Message = "User successfully registered"
            };
        }
        private async Task IsEmailExist(UserCreateDTO model)
        {
            var email = model.Email.Trim().ToLower();
            var user = await _repository.User.Get(x => x.Email == email).FirstOrDefaultAsync();
            Guard.AgainstDuplicate(user, "Email address already exists");
        }
        private async Task<bool> ValidateUser(User user, string password)
        {
            var result = (user != null && await _userManager.CheckPasswordAsync(user, password));
            if (!result)
                _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed, wrong email or password");

            //if (user != null && !user.Verified)
            //{
            //    _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed, User is not verified");
            //    return false;
            //}
            return result;
        }
        private async Task<AuthDto> CreateToken(User user, bool populateExp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var refreshToken = GenerateRefreshToken();
            if (populateExp)
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new AuthDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = user.RefreshTokenExpiryTime
            };

        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public async Task<SuccessResponse<AuthDto>> RefreshToken(RefreshTokenDTO model)
        {
            var principal = GetPrincipalFromExpiredToken(model.RefreshToken);
            if (principal.Identity != null)
            {
                var user = await _userManager.FindByNameAsync(principal.Identity.Name);
                if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime < DateTime.Now)
                    throw new RefreshTokenBadRequest();
                return new SuccessResponse<AuthDto>
                {
                    Data = await CreateToken(user, populateExp: false)
                };
            }
            throw new RestException(HttpStatusCode.Unauthorized, "This specified token has expired, please login");
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            //var jwtSettins = _configuration.GetSection("JwtSettings");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret)),
                ValidateLifetime = false,
                ValidAudience = _jwtConfiguration.ValidAudience,
                ValidIssuer = _jwtConfiguration.ValidIssuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        private SigningCredentials GetSigningCredentials()
        {
            var jwtSecret = _configuration.GetSection("JwtSettings")["secret"];
            var key = Encoding.UTF8.GetBytes(jwtSecret);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user!.Email),
            new Claim("Email", user.Email),
            new Claim("UserId", user.Id.ToString()),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
        };

            var roles = await _userManager.GetRolesAsync(user);
            var userRoles = new List<string>();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                userRoles.Add(role);
            }
            claims.Add(new Claim("RolesStr", string.Join(",", userRoles)));

            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            //var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_jwtConfiguration.ExpiresIn)),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
    }
}
