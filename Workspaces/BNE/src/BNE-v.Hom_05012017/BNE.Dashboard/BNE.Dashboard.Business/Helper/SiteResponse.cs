using BNE.Dashboard.Entities;
using System.Net;

namespace BNE.Dashboard.Business.Helper
{
    public class SiteResponse
    {

        public static void VerifyBusinessRule(Entities.Watcher watcher)
        {
            try
            {
                var webrequest = WebRequest.Create(watcher.SiteResponse.URL);
                using (var response = (HttpWebResponse)webrequest.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                        watcher.Status = Status.OK;
                    else
                        watcher.Status = Status.ERROR;
                }
            }
            catch
            {
                watcher.Status = Status.ERROR;
            }
        }

    }
}
