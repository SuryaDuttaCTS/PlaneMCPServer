using System.Net.Http;
using System.Text;
using System.Text.Json;

public class PlaneApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _baseUrl;
    private readonly string _workspace;
    private readonly string _projectId;
    private readonly string _apiKey;

    public PlaneApiService(IHttpClientFactory httpClientFactory,
    string baseUrl, 
    string workspace, 
    string projectId,
    string APIKey)
    {
        _httpClientFactory = httpClientFactory;
        _baseUrl = baseUrl.TrimEnd('/');
        _workspace = workspace;
        _projectId = projectId;
        _apiKey = APIKey;
    }

    public async Task<string> GetProjectStatessAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);

       var url = $"{_baseUrl}/api/v1/workspaces/{_workspace}/projects/{_projectId}/states/";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return content;
    }

    public async Task<string> createworkItemAsync(string name, string description, string stateId)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);

        var url = $"{_baseUrl}/api/v1/workspaces/{_workspace}/projects/{_projectId}/work-items/";
        var requestBody = new
        {
            name = name,
            description_html = description,
            state = stateId
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(url, httpContent);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}