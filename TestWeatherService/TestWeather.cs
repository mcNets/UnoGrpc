using Grpc.Net.Client;
using System.Threading.Channels;

namespace TestWeatherService;

public class WeatherServiceTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetWeather_WhenCityExistsSuccess()
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7014");

        var weatherClient = new Weather.WeatherClient(channel);
        var weatherReply = await weatherClient.GetWeatherAsync(
            new WeatherRequest { City = "Barcelona" });

        Assert.That(weatherReply.Success, Is.True);
    }

    [Test]
    public async Task GetWeather_WhenCityDoesNotExistFails()
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7014");

        var weatherClient = new Weather.WeatherClient(channel);
        var weatherReply = await weatherClient.GetWeatherAsync(
            new WeatherRequest { City = "NonExistentCity" });

        Assert.That(weatherReply.Success, Is.False);
    }
}
