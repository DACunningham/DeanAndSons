using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeanAndSons.Startup))]
namespace DeanAndSons
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
