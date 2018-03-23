using API.GatewayV2.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiThrottle;

namespace API.GatewayV2.Throttle
{
    public static class QuotaManager
    {
        //public static void InsertOrUpdate(Usuario usuario, Quota quota) 
        //{

        //    var policyRepository = new PolicyMemoryCacheRepository();
        //    var policy = policyRepository.FirstOrDefault(ThrottleManager.GetPolicyKey());

        //    string route = string.Format("{0}/api/{1}/{2}/{3}/{4}", usuario.Idf_Usuario, quota.Endpoint.VersionAPI,
        //        quota.Endpoint.Nme_Api, quota.Endpoint.Controller, quota.Endpoint.Action);

        //    var rates = new RateLimits()
        //    {
        //        PerSecond = quota.Per_Second,
        //        PerMinute = quota.Per_Minute,
        //        PerHour =   quota.Per_Hour,
        //        PerDay =    quota.Per_Day,
        //        PerWeek = 0 //Controlado pelo ProxyController
        //    };

        //    if (policy.ClientRules.ContainsKey(route))
        //        policy.ClientRules[route] = rates;
        //    else
        //        policy.ClientRules.Add(route, rates);

        //    ThrottleManager.UpdatePolicy(policy, policyRepository);
        //}

        public static void Update(Usuario usuario)
        {
            using(var dbo = new APIGatewayContext())
            {
                List<Quota> quotas = dbo.Quota.Include("Endpoint").Where(q => q.Idf_Perfil == usuario.Idf_Perfil).ToList();

                var policyRepository = new PolicyMemoryCacheRepository();
                var policy = policyRepository.FirstOrDefault(ThrottleManager.GetPolicyKey());

                foreach(var quota in quotas)
                {
                    string route = string.Format("{0}/api/{1}/{2}/{3}/{4}", usuario.Idf_Usuario, quota.Endpoint.VersionAPI,
                            quota.Endpoint.Nme_Api, quota.Endpoint.Controller, quota.Endpoint.Action);

                    var rates = new RateLimits()
                    {
                        PerSecond = quota.Per_Second,
                        PerMinute = quota.Per_Minute,
                        PerHour = quota.Per_Hour,
                        PerDay = quota.Per_Day,
                        PerWeek = 0 //Controlado pelo ProxyController
                    };

                    if (policy.ClientRules.ContainsKey(route))
                        policy.ClientRules[route] = rates;
                    else
                        policy.ClientRules.Add(route, rates);
                }

                ThrottleManager.UpdatePolicy(policy, policyRepository);
            }
         }


        public static void Clean(Usuario usuario)
        {
            using (var dbo = new APIGatewayContext())
            {
                List<Quota> quotas = dbo.Quota.Include("Endpoint").Where(q => q.Idf_Perfil == usuario.Idf_Perfil).ToList();

                var policyRepository = new PolicyMemoryCacheRepository();
                var policy = policyRepository.FirstOrDefault(ThrottleManager.GetPolicyKey());

                foreach (var quota in quotas)
                {
                    string route = string.Format("{0}/api/{1}/{2}/{3}/{4}", usuario.Idf_Usuario, quota.Endpoint.VersionAPI,
                            quota.Endpoint.Nme_Api, quota.Endpoint.Controller, quota.Endpoint.Action);

                    policy.ClientRules.Remove(route);
                }

                ThrottleManager.UpdatePolicy(policy, policyRepository);
            }
        }


            


        
    }
}