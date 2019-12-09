using System.ComponentModel.Design;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcFun.Server {
    public class GreeterService : Greeter.GreeterBase {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger) {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {

            if(request.Name?.ToLowerInvariant() == "rumpelstilzchen") throw new CheckoutException("check this out!");
            if (request.Name?.ToLowerInvariant() == "pete") {
                context.Status = Status.DefaultCancelled;
            }

            return Task.FromResult(new HelloReply {
                Message = "Hello " + request.Name + " (" + context.Peer + ")"
            });
        }
    }
}
