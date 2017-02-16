using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PedidosOnline.Startup))]
namespace PedidosOnline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
