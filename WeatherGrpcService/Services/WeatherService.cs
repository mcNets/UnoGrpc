using Grpc.Core;
using System.Text.Json;

namespace WeatherGrpcService.Services;

public class WeatherService(ILogger<WeatherService> logger) : Weather.WeatherBase
{
    private static readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://goweather.xyz/weather/")
    };

    public override async Task<WeatherReply> GetWeather(WeatherRequest request, ServerCallContext context)
    {
        logger.LogInformation("Requested temperature for {Name}", request.City);

        var encodedCity = Uri.EscapeDataString(request.City);
        var url = $"{encodedCity}";

        try
        {
            using var response = await _httpClient.GetAsync(url, context.CancellationToken);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(context.CancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: context.CancellationToken);

            var weather = document.RootElement;

            var reply = new WeatherReply
            {
                City = request.City,
                Temperature = weather.GetProperty("temperature").GetString() ?? string.Empty,
                Description = weather.GetProperty("description").GetString() ?? string.Empty,
                Wind = weather.GetProperty("wind").GetString() ?? string.Empty,
                Success = true
            };

            logger.LogInformation("Success: Temperature={temp}", reply.Temperature);

            return reply;
        }
        catch (Exception ex)
        {
            logger.LogError("Exception: {msg}", ex.Message);

            return new WeatherReply
            {
                City = request.City,
                Temperature = "",
                Description = ex.Message,
                Wind = string.Empty,
                Success = false
            };
        }

    }
}
