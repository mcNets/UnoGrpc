# Uno gRPC example

## Uno Solution

- [x] Add a new Uno Solution.
- [x] Use Agent to design a main page that input a citty name and a button to call a service to get the current temperature.
- [x] Add 3 labels to show 'temperature', 'wind' and 'weather description'.
- [x] Add a view model with 4 dependency properties: CityName, Temperature, Wind and Description.
- [x] Add a command to call the gRPC service and show the results.

### IMPORTANT

<ItemGroup>
  <PackageVersion Include="Google.Protobuf" Version="3.34.1" />
  <PackageVersion Include="Grpc.Net.Client" Version="2.76.0" />
  <PackageVersion Include="Grpc.Net.Client.Web" Version="2.76.0" />
  <PackageVersion Include="Grpc.Tools" Version="2.80.0" />
</ItemGroup>

<LangVersion>preview</LangVersion>

<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">
    <UseNativeHttpHandler>false</UseNativeHttpHandler>
</PropertyGroup>

<ItemGroup>
    <PackageReference Include="Google.Protobuf" />
    <PackageReference Include="Grpc.Net.Client" />
    <PackageReference Include="Grpc.Net.Client.Web" />
    <PackageReference Include="Grpc.Tools">
        <PrivateAssets>all</PrivateAssets>
        <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
</ItemGroup>
<ItemGroup>
    <Protobuf Include="Protos\greet.proto" GrpcServices="Client" />
</ItemGroup>

- Look at 'launchsettings.json' to know the port where WASM will be launched, it has to be added to the the CORS of gRPC service.

- Add INTERNET permissions to the Android project.


## gRPC Service

- [x] Add a new project to build a gRPC service.
- [x] Add a Protos folder and a service.proto file


### IMPORTANT

  <ItemGroup>
    <Protobuf Include="Protos\greet.proto" GrpcServices="Server" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Grpc.AspNetCore" Version="2.64.0" />
	<PackageReference Include="Grpc.AspNetCore.Web" Version="2.64.0" />
  </ItemGroup>

```csharp
builder.Services.AddGrpc();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Wasm", p => p
        .WithOrigins("http://localhost:5000", "https://localhost:5001")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

var app = builder.Build();

app.UseRouting();
app.UseGrpcWeb();
app.UseCors();

app.MapGrpcService<WeatherService>().EnableGrpcWeb().RequireCors("Wasm");
app.MapGet("/", () => "gRPC service");

app.Run();
```

```
"AllowedHosts": "*",
"Kestrel": {
  "EndpointDefaults": {
    "Protocols": "Http1AndHttp2"
  }
}
```

### Protos

```
service Weather {
	rpc GetWeather (WeatherRequest) returns (WeatherReply);
}

message WeatherRequest {
  string city = 1;
}

message WeatherReply {
  string city = 1;
  string temperature = 2;
  string description = 3;
  string wind = 4;
  bool success = 5;
}
```