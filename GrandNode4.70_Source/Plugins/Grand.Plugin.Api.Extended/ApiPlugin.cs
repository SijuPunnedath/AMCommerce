using Grand.Core.Plugins;
using System.Threading.Tasks;

namespace Grand.Plugin.Api.Extended
{
    public partial class ApiPlugin : BasePlugin
    {
        public ApiPlugin()
        {

        }

        public override async Task Install()
        {
            await base.Install();
        }

        public override async Task Uninstall()
        {
            await base.Uninstall();
        }
    }
}
