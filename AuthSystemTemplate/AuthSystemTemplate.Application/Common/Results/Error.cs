namespace AuthSystemTemplate.Application.Common.Results;

public class Error
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error None => new("", "");

    public static Error NotFound(string message) =>
        new("NotFound", message);

    public static Error Validation(string message) =>
        new("Validation", message);

    public static Error Unauthorized(string message) =>
        new("Unauthorized", message);

    public static Error Failure(string message) =>
        new("Failure", message);
}