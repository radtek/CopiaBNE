using System;
using System.Collections.Generic;
using System.Configuration;
using OneSignal.CSharp.SDK;
using OneSignal.CSharp.SDK.Resources.Notifications;
using SA.WebPush.Interfaces;

namespace SA.WebPush
{
    public class OneSignalWebPushService : IWebPushService
    {
        private static readonly Guid appid = Guid.Parse(ConfigurationManager.AppSettings["OneSignal-AppID"]);

        private readonly IOneSignalClient _oneSignalClient;

        public OneSignalWebPushService(IOneSignalClient oneSignalClient)
        {
            _oneSignalClient = oneSignalClient;
        }

        public string SendTemplate(IList<string> users, Guid template)
        {
            return _oneSignalClient.Notifications.Create(new NotificationCreateOptions
            {
                AppId = appid,
                TemplateId = template.ToString(),
                IncludePlayerIds = users,
                Contents = null,
                Headings = null
            }).Id;
        }

        public string Send(IList<string> users, string title, string message, string startUrl)
        {
            return _oneSignalClient.Notifications.Create(new NotificationCreateOptions
            {
                AppId = appid,
                IncludePlayerIds = users,
                Headings = new Dictionary<string, string> { { "en", title } },
                Contents = new Dictionary<string, string> { { "en", message } },
                Url = startUrl
            }).Id;
        }
    }
}