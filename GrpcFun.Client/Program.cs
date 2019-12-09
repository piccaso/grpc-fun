using System;
using Grpc.Net.Client;
using GrpcFun.Server;

namespace GrpcFun.Client {
    class Program {
        static void Main(string[] args) {
            var client = new Greeter.GreeterClient(GrpcChannel.ForAddress("https://localhost:5001"));
            var world = client.SayHello(new HelloRequest{Name = "world"});
            Console.WriteLine(world.Message);
            var rumpel = client.SayHello(new HelloRequest{Name = "pete"});
            Console.WriteLine(rumpel.Message);
        }
    }
}
