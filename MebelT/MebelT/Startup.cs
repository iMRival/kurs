using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MebelT.Startup))]
namespace MebelT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
