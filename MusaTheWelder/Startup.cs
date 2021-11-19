using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusaTheWelder.Startup))]
namespace MusaTheWelder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
