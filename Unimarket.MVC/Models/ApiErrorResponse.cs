namespace Unimarket.MVC.Models
{
    public class ApiErrorResponse
    {
        public string Message { get; set; }
        public string Detail { get; set; }
    }

    public class LoginResponseResult
    {
        public int status { get; set; }
        public AuthenticateObject data { get; set; }
    }

    public class AuthenticateObject
    {
        public int userId { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string role { get; set; }
    };
}