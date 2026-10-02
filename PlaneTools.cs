using System;
using ModelContextProtocol.Server;
using System.Text.Json;
using System.ComponentModel;

[McpServerToolType]
public class PlaneTools
{
    [McpServerTool, Description("Get All Work Item Statuses Retrieves all work item statuses from the Plane API.")]
    public static async Task<string> GetAllWorkItemStatuses(PlaneApiService planeApiService)
    {
        try
        {
            var response = await planeApiService.GetProjectStatessAsync();
            return JsonSerializer.Serialize(response);
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
    [McpServerTool, Description("Create Work Item Creates a new work item in the Plane API.")]
    public static async Task<string> CreateWorkItem(PlaneApiService planeApiService, 
    [Description("The name of the work item")] string name, 
    [Description("The description of the work item")] string description,
    [Description("The ID of the state for the work item")] string stateId)
    {
        try
        {
            var response = await planeApiService.createworkItemAsync(name, description, stateId);
            return JsonSerializer.Serialize(response);
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}