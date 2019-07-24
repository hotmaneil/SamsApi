using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SamsApi.Startup))]

namespace SamsApi
{
	public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
