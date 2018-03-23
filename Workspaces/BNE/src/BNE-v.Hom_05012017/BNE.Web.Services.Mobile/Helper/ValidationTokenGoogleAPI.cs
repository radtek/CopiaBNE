using BNE.BLL;
using Google.Apis.AndroidPublisher.v1_1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace BNE.Web.Services.Mobile.Helper
{
    public class ValidationTokenGoogleAPI
    {
        public static bool ValidationToken(String token, int plano)
        {
            try
            {
                if (Pagamento.ExisteTokenCelular(token)) return false;


                var certificate = new X509Certificate2(ConfigurationManager.AppSettings["CertificadoMobile"].ToString(), ConfigurationManager.AppSettings["ChaveAutenticacaoMobile"].ToString(),
                    X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);

                var serviceAccountCredential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(ConfigurationManager.AppSettings["EmailAutenticacaoMobile"].ToString())
                {
                    Scopes = new[] { AndroidPublisherService.Scope.Androidpublisher }
                }.FromCertificate(certificate));


                var androidPublisherService = new AndroidPublisherService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = serviceAccountCredential,
                    ApplicationName = "BNE"
                });


                var inAppPurchaseData = ((InapppurchasesResource.GetRequest)androidPublisherService.Inapppurchases.Get(ConfigurationManager.AppSettings["PackageProject"].ToString(), plano.ToString(), token))
                .Execute();

                if (inAppPurchaseData.PurchaseState.HasValue || inAppPurchaseData.PurchaseState.Value == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Google.GoogleApiException ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return false;
            }
        }
    }
}