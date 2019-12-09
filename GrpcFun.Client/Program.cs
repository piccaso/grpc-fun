using System;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcFun.Server;

namespace GrpcFun.Client {
    class Program {
        static void Main(string[] args) {
            using var channel = GrpcChannel.ForAddress("https://grpc-fun.m.887.at/");
            var client = new Greeter.GreeterClient(channel);
            
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
