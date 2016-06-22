using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToanThangEdu.Startup))]
namespace ToanThangEdu
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
