using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcFun.Server;

namespace GrpcFun.Client {
    class Program {
        static void Main(string[] args) {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001/");
            var client = new Greeter.GreeterClient(channel);

            var meta = new Metadata {{"foo", "bar"}};
            var stream = client.StreamHello(new HelloRequest() { Name = "Me" }, meta);

            Task.Run(async () => {
                await foreach (var x in stream.ResponseStream.ReadAllAsync()) {
                    Console.WriteLine($"'{x.Key}' => {x.StringValue}");
                }
            }).GetAwaiter().GetResult();
            
            
            var world = client.SayHello(new HelloRequest{Name = "world"});
            Console.WriteLine(world.Message);
            
            try {
                var pete = client.SayHello(new HelloRequest{Name = "pete"});
                Console.WriteLine(pete.Message);
            } catch (RpcException e) {
                Console.WriteLine(e.Message);
            }

            try {
                var rumpel = client.SayHello(new HelloRequest{Name = "rumpelstilzchen"});
                Console.WriteLine(rumpel.Message);
            } catch (RpcException e) {
                Console.WriteLine(e.Message);
            }

        }
    }
}
