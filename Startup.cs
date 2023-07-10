using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WedSiteBanHang.Startup))]
namespace WedSiteBanHang
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
