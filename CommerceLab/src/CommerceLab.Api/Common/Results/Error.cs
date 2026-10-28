namespace CommerceLab.Api.Common.Results;

public sealed record Error(
    string Code,
    string Message
);