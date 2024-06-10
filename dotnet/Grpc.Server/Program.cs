using Grpc.Sample;
using Grpc.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CharService>();
app.MapGrpcReflectionService();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.RunAsync();

public class CharService : Chat.ChatBase
{
    public override async Task Talk(IAsyncStreamReader<ChatMessage> requestStream, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
    {
        Console.WriteLine($">> client connected :)");

        while (await requestStream.MoveNext())
        {
            Console.Write($"{requestStream.Current.Message} >> ");

            // Totally wrong, don't block async method ever
            var reply = await Task.Run(() => Console.ReadLine());

            await responseStream.WriteAsync(new ChatMessage{Message = reply});
        }        

        Console.WriteLine($">> client leave us :(");
    }
}