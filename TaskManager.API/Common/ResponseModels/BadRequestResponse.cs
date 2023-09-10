namespace TaskManager.API.Common.ResponseModels;

public sealed class BadRequestResponse
{
    public string Title { get; set; }
    public int StatusCode { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }
}