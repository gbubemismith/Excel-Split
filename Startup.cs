using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CsvUpload.Startup))]
namespace CsvUpload
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
