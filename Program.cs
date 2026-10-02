using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ModelContextProtocol.Server;
using Microsoft.Extensions.DependencyInjection; 
var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

var planeApiKey = builder.Configuration["PlaneAPIKey"];
var baseUrl = builder.Configuration["BaseUrl"];
var workspace = builder.Configuration["workspace"];
var projectId = builder.Configuration["ProjectId"];

if(string.IsNullOrEmpty(planeApiKey))
    throw new InvalidOperationException("Plane API Key is not set. Please set the PlaneAPIKey in user secrets or environment variables.");

if(string.IsNullOrEmpty(baseUrl))
    throw new InvalidOperationException("Base URL is not set. Please set the BaseUrl in user secrets or environment variables.");

if(string.IsNullOrEmpty(workspace))
    throw new InvalidOperationException("Workspace is not set. Please set the workspace in user secrets or environment variables.");

if(string.IsNullOrEmpty(projectId))
    throw new InvalidOperationException("Project ID is not set. Please set the ProjectId in user secrets or environment variables.");

builder.Services.AddHttpClient();
builder.Services.AddSingleton((sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new PlaneApiService(httpClientFactory, baseUrl, workspace, projectId, planeApiKey);
}));

builder.Services
.AddMcpServer()
.WithStdioServerTransport()
.WithToolsFromAssembly();

await builder.Build().RunAsync();