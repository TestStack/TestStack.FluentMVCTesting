using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestStack.FluentMVCTesting.Sample.Startup))]
namespace TestStack.FluentMVCTesting.Sample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
