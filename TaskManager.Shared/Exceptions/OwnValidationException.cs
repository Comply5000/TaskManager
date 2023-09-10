using FluentValidation.Results;
using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Shared.Exceptions;

public sealed class OwnValidationException : TaskManagerException
{
    private OwnValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public OwnValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    //public OwnValidationException(IDictionary<string, string[]> failuresDictionary)
    //    : this()
    //{
    //    Errors = failuresDictionary;
    //}

    public OwnValidationException(params ValidationEntry[] entries)
        : this()
    {
        if (entries?.Any() is true)
        {
            Errors = entries.ToDictionary(a => a.PropertyName, a => a.Errors);
        }
    }
    
    public IDictionary<string, string[]> Errors { get; }
}

public sealed record ValidationEntry(string PropertyName, string[] Errors);