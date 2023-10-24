using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unoproto;

namespace UnoGrpcCommon;


public class UnoGrpcClient
{

    public UnoGrpcClient() {
    }

    public static async Task<AdditionResponse> AddAsync(long op1, long op2) {

        GrpcChannel channel;

        var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
        channel = GrpcChannel.ForAddress("https://localhost:5018",
            new GrpcChannelOptions {

                HttpHandler = httpHandler
            });

        //if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER"))) {
        //    var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
        //    channel = GrpcChannel.ForAddress("http://localhost:5018",
        //        new GrpcChannelOptions {
        //            HttpHandler = httpHandler
        //        });
        //}
        //else if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID"))) {
        //    var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
        //    channel = GrpcChannel.ForAddress("http://10.0.2.2:5018",
        //        new GrpcChannelOptions {
        //            HttpHandler = httpHandler
        //        });
        //}
        //else {
        //    channel = GrpcChannel.ForAddress("http://localhost:5018");
        //}

        var client = new Operations.OperationsClient(channel);

        var reply = new AdditionResponse() { Result = 0, Message = string.Empty };

        //var headers = new Metadata {
        //    { "Access-Control-Allow-Origin", "*" },
        //    { "Access-Control-Allow-Methods", "GET, POST, OPTIONS" },
        //    { "Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With" }
        //};

        try {
            reply = await client.AddAsync(new AdditionRequest { Op1 = op1, Op2 = op2 });
            reply.Message = "OK";
        }
        catch (System.Exception ex) {
            reply.Message = ex.Message;
        }

        return reply;
    }
}
