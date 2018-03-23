using AllInTriggers.Base;
using AllInTriggers.Helper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AllInTriggers
{
    public class AllInRequest : AllInRequestBase
    {
        private static Lazy<RestSharpJsonNetSerializer> _serializer = new Lazy<RestSharpJsonNetSerializer>(() => new RestSharpJsonNetSerializer());

        private readonly string _baseUrl;

        public AllInRequest(string baseUrl)
        {
            this._baseUrl = baseUrl;
        }

        public override string BaseUrl
        {
            get
            {
                return _baseUrl;
            }
        }

        public override string ResourceRequest
        {
            get;
            set;
        }

        public override Method Method
        {
            get;
            set;
        }
        public override IEnumerable<KeyValuePair<string, string>> UrlSegment
        {
            get;
            set;
        }

        public Action<IRestRequest> ManipulateRequest { get; set; }
        public Func<KeyValuePair<string, string>> PostRequestBodyAccessor { get; protected set; }
        public Func<IRestResponse, string> ResultResponseAccessor { get; set; }

        public AllInRequest SetContentBodyAccessor(Func<KeyValuePair<string, string>> accessor)
        {
            if (accessor == null)
                throw new NullReferenceException("accessor");

            PostRequestBodyAccessor = accessor;
            _lazyRequestAccessor = null;
            return this;
        }

        public AllInRequest SetResultResponseAccesssor(Func<IRestResponse, string> accessor)
        {
            if (accessor == null)
                throw new NullReferenceException("accessor");

            ResultResponseAccessor = accessor;
            return this;
        }
        public override string Execute()
        {
            var client = new RestClient(BaseUrl);
            RestRequest req = PrepareRequest();
            var response = client.Execute(req);
            try
            {
                if (response == null)
                    throw new NullReferenceException("response");

                return (ResultResponseAccessor ?? new Func<IRestResponse, string>((arg) => arg.Content))(response);
            }
            finally
            {
                var disp = response as IDisposable;
                if (disp != null)
                {
                    disp.Dispose();
                }
            }
        }

        protected virtual RestRequest PrepareRequest()
        {
            var req = new RestRequest(ResourceRequest, Method);
            req.JsonSerializer = _serializer.Value;
            req.RequestFormat = DataFormat.Json;

            foreach (var item in UrlSegment ?? Enumerable.Empty<KeyValuePair<string, string>>())
            {
                req.AddUrlSegment(item.Key, item.Value);
            }

            req.AddParameter("Content-Type", @"application/x-www-form-urlencoded", ParameterType.HttpHeader);
            if (Method == RestSharp.Method.POST)
            {
                var body = GetCachedBodyRequest().Value;
                req.AddParameter(body.Key, body.Value, ParameterType.GetOrPost);
            }

            if (ManipulateRequest != null)
                ManipulateRequest(req);
            return req;
        }

        private Lazy<KeyValuePair<string, string>> _lazyRequestAccessor;
        private Lazy<KeyValuePair<string, string>> GetCachedBodyRequest()
        {
            if (_lazyRequestAccessor != null)
                return _lazyRequestAccessor;

            Interlocked.CompareExchange(ref _lazyRequestAccessor, new Lazy<KeyValuePair<string, string>>(() =>
            {
                var access = PostRequestBodyAccessor;

                if (access == null)
                    throw new NullReferenceException("PostRequestAccessor");

                return access();
            }), null);

            return _lazyRequestAccessor;
        }
        public override Task<string> ExecuteAsync()
        {
            RestClient client = new RestClient(BaseUrl);
            RestRequest req = PrepareRequest();
            var response = client.ExecuteTaskAsync(req);

            var rValid = response.ContinueWith(new Func<Task<IRestResponse>, string>(t =>
            {
                try
                {
                    if (t.Result == null)
                        throw new NullReferenceException("response");

                    return ResultResponseAccessor(t.Result);
                }
                finally
                {
                    var disp = response as IDisposable;
                    if (disp != null)
                    {
                        disp.Dispose();
                    }
                }
            }), TaskScheduler.FromCurrentSynchronizationContext());

            return rValid;
        }
    }
}

