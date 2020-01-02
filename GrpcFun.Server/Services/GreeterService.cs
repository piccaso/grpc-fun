using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
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

        public override async Task StreamHello(HelloRequest request, IServerStreamWriter<StreamResponse> responseStream, ServerCallContext context) {
            foreach (var header in context.RequestHeaders) {
                var resp = new StreamResponse {
                    IsBinary = header.IsBinary,
                    Key = header.Key,
                };

                if (header.IsBinary) {
                    resp.BytesValue = ByteString.CopyFrom(header.ValueBytes);
                }
                else {
                    resp.StringValue = header.Value;
                }

                await responseStream.WriteAsync(resp);
                Thread.Sleep(1000);
            }
        }
    }
}
