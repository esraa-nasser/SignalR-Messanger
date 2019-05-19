using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MessangerChat.Startup))]
namespace MessangerChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
