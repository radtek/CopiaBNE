using System;
using System.Collections.Generic;
using System.Linq;

namespace BNE.Auth.Core
{
    public class MultipleAuthBehavior : LoginBehaviorBase
    {
        public IEnumerable<LoginBehaviorBase> Behaviors
        {
            get;
            set;
        }

        public MultipleAuthBehavior(params LoginBehaviorBase[] behaviors)
        {
            this.Behaviors = behaviors ?? new LoginBehaviorBase[0];
        }

        protected override void OnBeforeLogInSafe(Func<System.Web.HttpContextBase> contextAccessor, Microsoft.IdentityModel.Claims.ClaimsIdentity info)
        {
            foreach (var item in Behaviors ?? Enumerable.Empty<LoginBehaviorBase>())
            {
                var exp = new List<Exception>();
                try
                {
                    item.OnBeforeLogin(contextAccessor, info);
                }
                catch (Exception ex)
                {
                    exp.Add(ex);
                }

                if (exp.Count > 0)
                    this.SetExceptionBeforeLogIn(new AggregateException(exp));
            }
        }

        protected override void OnBeforeLogoffSafe(Func<System.Web.HttpContextBase> context, LogOffInfo info)
        {
            foreach (var item in Behaviors ?? Enumerable.Empty<LoginBehaviorBase>())
            {
                var exp = new List<Exception>();
                try
                {
                    item.OnBeforeLogoff(context, info);
                }
                catch (Exception ex)
                {
                    exp.Add(ex);
                }

                if (exp.Count > 0)
                    this.SetExceptionBeforeLogoff(new AggregateException(exp));
            }
        }

        public override void OnAfterLogin(System.Web.HttpContextBase context, Microsoft.IdentityModel.Claims.ClaimsIdentity claims, System.Web.HttpCookie formsAuthCookie)
        {
            foreach (var item in Behaviors ?? Enumerable.Empty<LoginBehaviorBase>())
            {
                item.OnAfterLogin(context, claims, formsAuthCookie);
            }
        }

        public override void OnAfterLogoff(System.Web.HttpContextBase context, LogOffInfo info)
        {
            foreach (var item in Behaviors ?? Enumerable.Empty<LoginBehaviorBase>())
            {
                item.OnAfterLogoff(context, info);
            }
        }
    }
}
