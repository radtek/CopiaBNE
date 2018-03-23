using System;
using System.Collections.Generic;

namespace SA.WebPush.Interfaces
{
    public interface IWebPushService
    {
        string SendTemplate(IList<string> users, Guid template);
        string Send(IList<string> users, string title, string message, string startUrl);
    }
}