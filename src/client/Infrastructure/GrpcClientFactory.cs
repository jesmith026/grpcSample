using System.Net.Http;
using Grpc.Core;
using Grpc.Net.Client;

namespace client.Infrastructure
{
    public class GrpcClientFactory
    {
        private readonly IHttpClientFactory clientFactory;

        public GrpcClientFactory(IHttpClientFactory clientFactory) => this.clientFactory = clientFactory;

        public TClient Get<TClient>(string name) where TClient : ClientBase<TClient> =>
            GrpcClient.Create<TClient>(clientFactory.CreateClient(name));
    }
}