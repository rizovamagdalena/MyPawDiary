using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyPawDiaryApp.Startup))]
namespace MyPawDiaryApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
