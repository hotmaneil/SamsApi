using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SamsApi
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new
				{
					controller = "Home",
					action = "Index",
					id = UrlParameter.Optional
				}
			);

			routes.MapRoute(
				name: "swagger",
				url: "swagger/ui/index",
				defaults: new
				{
					controller = "swagger",
					action = "ui",
					id = UrlParameter.Optional
				},
				constraints: new {controller= "swagger", Action= "ui" }
			);
		}
	}
}
