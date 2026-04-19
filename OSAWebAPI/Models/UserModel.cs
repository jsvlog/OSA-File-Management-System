namespace OSAWebAPI.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? FullName { get; set; }
    }

    public class LoginViewModel
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string? ReturnUrl { get; set; }
    }
}