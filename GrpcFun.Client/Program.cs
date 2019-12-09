using System;
using System.Net.Http;
using Grpc.Net.Client;
using GrpcFun.Server;

namespace GrpcFun.Client {
    class Program {
        static void Main(string[] args) {
            using var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback += (message, certificate2, arg3, arg4) => true;
            using var httpClient = new HttpClient(httpClientHandler);
            var options = new GrpcChannelOptions {HttpClient = httpClient};
            using var channel = GrpcChannel.ForAddress("https://m.887.at:65211/", options);
            var client = new Greeter.GreeterClient(channel);
            
            var world = client.SayHello(new HelloRequest{Name = "world"});
            Console.WriteLine(world.Message);
            
            try {
                var pete = client.SayHello(new HelloRequest{Name = "pete"});
                Console.WriteLine(pete.Message);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            try {
                var rumpel = client.SayHello(new HelloRequest{Name = "rumpelstilzchen"});
                Console.WriteLine(rumpel.Message);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

        }
    }
}
