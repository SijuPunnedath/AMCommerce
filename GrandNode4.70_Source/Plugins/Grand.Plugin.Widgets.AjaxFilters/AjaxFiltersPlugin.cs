using Grand.Core;
using Grand.Core.Domain.Messages;
using Grand.Core.Plugins;
using Grand.Framework.Menu;
using Grand.Services.Cms;
using Grand.Services.Configuration;
using Grand.Services.Localization;
using Grand.Services.Messages;
using Grand.Services.Security;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Plugin.Widgets.AjaxFilters
{
    public class AjaxFiltersPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        private static string pn = "Ajax Filters";

        private readonly AjaxFiltersSettings _ajaxFiltersSettings;
        private readonly IWebHelper _webHelper;
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IStoreContext _storeContext;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IQueuedEmailService _queuedEmailService;

        public AjaxFiltersPlugin(AjaxFiltersSettings ajaxFiltersSettings, IWebHelper webHelper,
            ISettingService settingService,
            IPermissionService permissionService,
            IEmailAccountService emailAccountService,
            EmailAccountSettings emailAccountSettings,
            IStoreContext storeContext,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IQueuedEmailService queuedEmailService)
        {
            _ajaxFiltersSettings = ajaxFiltersSettings;
            _webHelper = webHelper;
            _settingService = settingService;
            _permissionService = permissionService;
            _emailAccountService = emailAccountService;
            _emailAccountSettings = emailAccountSettings;
            _storeContext = storeContext;
            _languageService = languageService;
            _localizationService = localizationService;
            _queuedEmailService = queuedEmailService;
        }

        public void GetPublicViewComponent(string widgetZone, out string viewComponentName)
        {
            viewComponentName = "Grand.Plugin.Widgets.AjaxFilters";
        }

        public IList<string> GetWidgetZones()
        {
            return (!string.IsNullOrWhiteSpace(_ajaxFiltersSettings.FilterWidgetZone) ?
                new List<string> { _ajaxFiltersSettings.FilterWidgetZone } :
                new List<string> { "left_side_column_before" });
        }

        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/AjaxFilters/Configure";
        }

        public override async Task Install()
        {
            var settings = new AjaxFiltersSettings {
                FilterWidgetZone = "left_side_column_before"
            };

            await _settingService.SaveSetting(settings);

             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.FilterEnablePriceRange", "Enable filter price range");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.FilterEnableManufacturers", "Enable filter manufacturers");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.FilterEnableVendors", "Enable filter vendors");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.FilterEnableSpecifications", "Enable filter specifications");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.FilterEnableAttributes", "Enable filter attributes");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.FilterManufacturerCheckOrDropdown", "Filter manufacturer check or dropdown");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.FilterVendorCheckOrDropdown", "Filter vendor check or dropdown");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.FilterSpecyficationCheckOrDropdown", "Filter specyfication check or dropdown");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.FilterAttributesCheckOrDropdown", "Filter attributes check or dropdown");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.FilterWidgetZone", "Filter WidgetZone");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.Filter.All", "All");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.Filter.Empty", "Empty");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.Manufacturers", "Manufacturers");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.Filter.FilterByPrice", "Filter by price");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.PriceRangeFilter.Min", "Min");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.PriceRangeFilter.Max", "Max");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.Filter.ClearAll", "Clear all");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.Vendors", "Vendors");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.AjaxFilters.Toggle", "Toggle");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugin.Widgets.Ajaxfilters.Clearall", "Clear all");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Widgets.Ajaxfilters.Fields.Configuration", "Configuration");

            await CreateMessage();

            await base.Install();
        }

        public override async Task Uninstall()
        {
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "Plugins.Widgets.Ajaxfilters.Fields.Configuration");

            await CreateMessage(false);
            await base.Uninstall();
        }

        #region Sitemap && notifications

        public async Task ManageSiteMap(SiteMapNode rootNode)
        {
            if (await _permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
            {
                var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "extensions");
                if (pluginNode == null)
                {
                    rootNode.ChildNodes.Add(new SiteMapNode() {
                        SystemName = "extensions",
                        Title = "Extensions",
                        Visible = true,
                        IconClass = "icon-paper-clip"
                    });
                    pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "extensions");
                }

                SiteMapNode Menu = new SiteMapNode();
                Menu.Title = "Ajax filters";
                Menu.Visible = true;
                Menu.SystemName = "Ajaxfilters";

                Menu.ChildNodes.Add(new SiteMapNode() {
                    Title = _localizationService.GetResource("Plugins.Widgets.Ajaxfilters.Fields.Configuration"),
                    Visible = true,
                    SystemName = "Ajaxfilters.Settings",
                    IconClass = "",
                    ControllerName = "AjaxFilters",
                    ActionName = "Configure",
                    RouteValues = new RouteValueDictionary() { { "area", "Admin" } },
                });
                Menu.ChildNodes.Add(new SiteMapNode() {
                    Title = "Support",
                    Visible = true,
                    SystemName = "Ajaxfilters.Support",
                    IconClass = "",
                    Url = "https://grandnode.com/boards/forum/59f6f028ba1646279c61a5e4/community-plugins-themes"
                });

                if (pluginNode != null)
                    pluginNode.ChildNodes.Add(Menu);
                else
                    rootNode.ChildNodes.Add(Menu);
            }
        }

        private async Task CreateMessage(bool install = true)
        {
            var emailAccount = await _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
            await _queuedEmailService.InsertQueuedEmail(new QueuedEmail() {
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = _emailAccountSettings.DefaultEmailAccountId,
                Priority = QueuedEmailPriority.Low,
                Subject = $"Plugin {(install ? "installation" : "uninstall")} notification {pn}",
                Body = $"<p><a href=\"{_storeContext.CurrentStore.Url}\">{_storeContext.CurrentStore.Name}</a>&nbsp;</p><p>Plugin {pn} has been installed</p>",
                To = "support@nop4you.com",
                ToName = "Support nop4you",
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName
            });
        }

        #endregion

    }
}
