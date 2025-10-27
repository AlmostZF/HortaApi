namespace DDDPractice.Application.Shared;

public class Result<T>
{
    public string? Error { get; set; }
    public T? Value { get; set; }

    public int StatusCode { get; set; }

    public static Result<T> Success(T value,int statusCode = 200) 
        => new Result<T> { Value = value, StatusCode = statusCode};
    public static Result<T> Failure(string error,int statusCode = 400) 
        => new Result<T> { Error = error, StatusCode = statusCode };
}
public class Result
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }

    public static Result Success(string message, int statusCode = 204) 
        => new Result { StatusCode = statusCode, Message = message};

    public static Result Failure(string error, int statusCode = 500)
        => new Result { Error = error, StatusCode = statusCode };
}