using UnoGrpcCommon;

Console.WriteLine("Uno gRPC Console test.");

var result = await UnoGrpcClient.AddAsync(12, 21);

Console.WriteLine($"Addition result: 12 + 21 = {result.Result} ({result.Message})");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
