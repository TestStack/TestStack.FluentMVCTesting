using TestStack.FluentMVCTesting.Sample.Models;
using WebMatrix.WebData;

namespace TestStack.FluentMVCTesting.Sample.Services
{
    public interface IAuthenticationService
    {
        bool Login(LoginModel model);
    }

    public class AuthenticationService : IAuthenticationService
    {
        public bool Login(LoginModel model)
        {
            return WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe);
        }
    }
}