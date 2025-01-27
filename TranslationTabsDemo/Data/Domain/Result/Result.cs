namespace TranslationTabsDemo.Data.Domain.Result;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);
    public static Result<TValue> Success<TValue>(TValue data) => new(data, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? data) =>
        data is not null ? Success(data) : Failure<TValue>(Error.None);
}

public class Result<TValue> : Result
{
    private readonly TValue _data;

    public Result(TValue? data, bool isSuccess, Error error) : base(isSuccess, error)
        => _data = data!;

    public TValue Data =>
        IsSuccess ? _data : throw new InvalidOperationException("Cannot access data on a failed result.");

    public static implicit operator Result<TValue>(TValue? data) => Create(data);
}