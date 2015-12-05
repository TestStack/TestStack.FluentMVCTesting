using System.Web.Hosting;

namespace Xania.AspNet.Simulator
{
    internal class ActionRouterPathProvider : VirtualPathProvider
    {
        public override bool FileExists(string virtualPath)
        {
            return false;
        }
    }
}