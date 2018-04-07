using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NaturalFrut.Startup))]
namespace NaturalFrut
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
