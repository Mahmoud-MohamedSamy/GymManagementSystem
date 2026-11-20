namespace GymManagementSystem.BLL.Common;

public enum ResultStatus
{
    Success,
    NotFound,
    Invalid
}

public class Result
{
    protected Result(bool isSuccess, string? error, ResultStatus status)
    {
        IsSuccess = isSuccess;
        Error = error;
        Status = status;
    }

    public bool IsSuccess { get; }
    public string? Error { get; }
    public ResultStatus Status { get; }

    public static Result Ok() => new(true, null, ResultStatus.Success);

    public static Result Fail(string error, ResultStatus status = ResultStatus.Invalid)
        => new(false, error, status);

    public static Result<T> Ok<T>(T value)
        => new(value, true, null, ResultStatus.Success);

    public static Result<T> Fail<T>(string error, ResultStatus status = ResultStatus.Invalid)
        => new(default, false, error, status);
}

public class Result<T> : Result
{
    internal Result(T? value, bool isSuccess, string? error, ResultStatus status)
        : base(isSuccess, error, status)
    {
        Value = value;
    }

    public T? Value { get; }
}
