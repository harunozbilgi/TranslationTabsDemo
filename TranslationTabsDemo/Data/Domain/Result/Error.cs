namespace TranslationTabsDemo.Data.Domain.Result;

public sealed record Error(string Message)
{
    public static readonly Error None = new Error(string.Empty);

    public static readonly Error NullValue = new Error("The specified value is null.");

    public static implicit operator Result(Error error) => Result.Failure(error);
}