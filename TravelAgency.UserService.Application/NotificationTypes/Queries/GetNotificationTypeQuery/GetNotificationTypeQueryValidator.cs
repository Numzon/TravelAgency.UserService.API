using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypeQuery;
public sealed class GetNotificationTypeQueryValidator : AbstractValidator<GetNotificationTypeQuery>
{
    public GetNotificationTypeQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
