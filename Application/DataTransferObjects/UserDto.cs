
using Microsoft.AspNetCore.Http;

namespace Application.DataTransferObjects
{
    public record TokenDto(string AccessToken, string RefreshToken, DateTime ExpiresIn);

    public record UserCreateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
    public record UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public record RefreshTokenDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public record ResetPasswordDTO
    {
        public string Email { get; set; }

    }
    public record SetPasswordDTO
    {
        public string Password { get; set; }
        public string Token { get; set; }
    }
    public record AuthDto
    {
        public string AccessToken { get; set; }
        public DateTime? ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }

    public record UserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public record UpdateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public record ActivateDeactivateUserDTO
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
    public record LoggedinUserDto
    {
        public Guid UserId { get; set; }
        public Guid InstanceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}