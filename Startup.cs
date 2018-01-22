using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GlsLeague.Startup))]
namespace GlsLeague
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
