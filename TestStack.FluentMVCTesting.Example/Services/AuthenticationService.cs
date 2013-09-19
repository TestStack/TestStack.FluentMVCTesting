using TestStack.FluentMVCTesting.Example.Models;
using WebMatrix.WebData;

namespace TestStack.FluentMVCTesting.Example.Services
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