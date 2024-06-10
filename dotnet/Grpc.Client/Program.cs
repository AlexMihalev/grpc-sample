using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Sample;

var options = new GrpcChannelOptions();
var channel = GrpcChannel.ForAddress("http://localhost:5156", options);
var client = new Chat.ChatClient(channel);

var call = client.Talk();

for (var prompt = "say";;) {
    Console.Write($"{prompt} >> ");

    var msg = Console.ReadLine();

    await call.RequestStream.WriteAsync(new ChatMessage{Message = msg });

    if (await call.ResponseStream.MoveNext()) {
        prompt = call.ResponseStream.Current.Message;
    }
}



