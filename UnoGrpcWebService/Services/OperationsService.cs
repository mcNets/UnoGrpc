using Grpc.Core;
using Unoproto;

namespace UnoGrpcWebService.Services;

public class OperationsService : Operations.OperationsBase {

    private readonly ILogger<OperationsService> _logger;

    public OperationsService(ILogger<OperationsService> logger) {
        _logger = logger;
    }

    public override Task<AdditionResponse> Add(AdditionRequest request, ServerCallContext context) {
        _logger.LogInformation($"Addition request received: {request.Op1} + {request.Op2}");

        return Task.FromResult(new AdditionResponse {
            Result = request.Op1 + request.Op2, Message = "OK"
        });
    }
}
