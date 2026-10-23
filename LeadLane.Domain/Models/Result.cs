namespace LeadLane.Domain.Models;

public class Result<T>
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public T? Data { get; init; }

    public static Result<T> Ok(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public static Result<T> Fail(string error) => new()
    {
        IsSuccess = false,
        ErrorMessage = error
    };
}
