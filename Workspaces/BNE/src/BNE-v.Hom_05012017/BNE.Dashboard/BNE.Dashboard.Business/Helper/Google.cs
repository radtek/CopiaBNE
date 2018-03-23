using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace BNE.Dashboard.Business.Helper
{
    public class Google
    {

        public sealed class Analytics
        {
            private const string ServiceAccountEmail = "305159901946-45m2p81b5n1g3iluekdgqdf60bucso7v@developer.gserviceaccount.com";

            private static volatile Analytics _analytics;
            private static volatile AnalyticsService _googleAnalyticsService;
            private static readonly object SyncRoot = new Object();

            private static AnalyticsService GetAnalyticsServiceInstance()
            {
                try
                {
                    string certificatePath = ConfigurationManager.AppSettings["CertificatePath"];

                    var certificate = new X509Certificate2(certificatePath, "notasecret", X509KeyStorageFlags.Exportable);

                    var credential = new ServiceAccountCredential(
                       new ServiceAccountCredential.Initializer(ServiceAccountEmail)
                       {
                           Scopes = new[] { AnalyticsService.Scope.Analytics }
                       }.FromCertificate(certificate));

                    return new AnalyticsService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "BNE",
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }

            private static AnalyticsService GoogleAnalyticsService
            {
                get
                {
                    if (_googleAnalyticsService == null)
                    {
                        lock (SyncRoot)
                        {
                            if (_googleAnalyticsService == null)
                                _googleAnalyticsService = GetAnalyticsServiceInstance();
                        }
                    }

                    return _googleAnalyticsService;
                }
            }

            public static Analytics Instance
            {
                get
                {
                    if (_analytics == null)
                    {
                        lock (SyncRoot)
                        {
                            if (_analytics == null)
                                _analytics = new Analytics();
                        }
                    }

                    return _analytics;
                }
            }

            public string GetData(string viewID, string metric)
            {
                try
                {
                    return GoogleAnalyticsService.Data.Realtime.Get("ga:" + viewID, "rt:" + metric).Execute().TotalsForAllResults["rt:" + metric];
                }
                catch
                {
                    return string.Empty;
                }
            }

        }

    }
}
