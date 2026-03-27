namespace AuthSystemTemplate.Application.Common;

public record ErrorResponse(string Code, string Message, string TraceId);