namespace StablingApiClient
{
    public abstract class SwaggerClientBase
    {
        public string ApiKey { get; private set; }
        public void SetApiKey(string key){ ApiKey = key; }

        // Called by implementing swagger client classes
        protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            var msg = new HttpRequestMessage();
            // SET THE BEARER AUTH TOKEN
            //msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
            //msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("X-API-KEY", ApiKey);
            msg.Headers.Add("X-API-KEY", ApiKey);
            //msg.Headers.Authorization = new Nswag 
            return Task.FromResult(msg);
        }

    }
}