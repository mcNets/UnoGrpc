using UnoGrpcWebService.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

const string CorsPolicyName = "_CorsPolicy";
builder.Services.AddCors(options => {
    options.AddPolicy(CorsPolicyName, builder => {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
    });
});

var app = builder.Build();

app.UseGrpcWeb();
app.UseCors(CorsPolicyName);

app.MapGrpcService<GreeterService>().RequireCors(CorsPolicyName).EnableGrpcWeb();
app.MapGrpcService<OperationsService>().RequireCors(CorsPolicyName).EnableGrpcWeb();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
