namespace ResApp.Models;

public record ServiceResult(bool Success, string Message)
{
    public static ServiceResult Successful(string message = "") => new(true, message);
    public static ServiceResult Failed(string message) => new(false, message);
}
