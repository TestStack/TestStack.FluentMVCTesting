using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Xania.AspNet.Simulator
{
    public class Router
    {
        private readonly Dictionary<string, ControllerBase> _controllerMap;

        public RouteCollection Routes { get; private set; }

        public Router()
        {
            _controllerMap = new Dictionary<String, ControllerBase>();
            Routes = new RouteCollection(new ActionRouterPathProvider());
            if (RouteTable.Routes.Any())
                foreach (var r in RouteTable.Routes)
                    Routes.Add(r);
            else
                foreach (var r in DefaultRoutes)
                    Routes.Add(r);
        }

        public virtual Router RegisterController(string name, ControllerBase controller)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            _controllerMap.Add(name.ToLower(CultureInfo.InvariantCulture), controller);

            return this;
        }

        protected internal virtual ControllerBase CreateController(string controllerName)
        {
            ControllerBase controller;
            if (_controllerMap.TryGetValue(controllerName.ToLower(CultureInfo.InvariantCulture), out controller))
                return controller;

            throw new KeyNotFoundException(controllerName);
        }

        public RouteData GetRouteData(HttpContextBase context)
        {
            return Routes.GetRouteData(context);
        }

        public static RouteCollection DefaultRoutes { get; set; }

        static Router ()
        {
            DefaultRoutes = new RouteCollection();
            DefaultRoutes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}
