namespace BackEndAPI.Models.UserService
{
    public class LoginResult
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }
    }
}
