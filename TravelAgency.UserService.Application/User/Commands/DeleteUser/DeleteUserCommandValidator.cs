using FluentValidation;

namespace TravelAgency.UserService.Application.User.Commands.DeleteUser;
public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
