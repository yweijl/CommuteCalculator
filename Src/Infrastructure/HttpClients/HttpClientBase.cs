using AutoMapper;
using System.Web;

namespace Infrastructure.HttpClients
{
    public abstract class HttpClientBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly IMapper _mapper;


        protected HttpClientBase(HttpClient client, IMapper mapper)
        {
            _httpClient = client;
            _mapper = mapper;
        }

        protected virtual Task<HttpResponseMessage> GetAsync(string urlSuffix, Dictionary<string, string> queryParams)
        {
            var uri = new Uri(_httpClient.BaseAddress!, urlSuffix);

            string requestUri = BuildQueryRequestUri(queryParams, uri);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            return _httpClient.SendAsync(request);
        }

        private static string BuildQueryRequestUri(Dictionary<string, string> queryParams, Uri uri)
        {
            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var param in queryParams)
            {
                query[param.Key] = param.Value;
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }
    }
}