using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public class UserWithThatEmailExistsException : TaskManagerException
{
    public UserWithThatEmailExistsException() : base("User with that email exists") { }
}