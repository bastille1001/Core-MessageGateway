using Application.Commands;
using FluentValidation;

namespace Application.Validation
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            RuleFor(v => v.Source)
                .NotNull()
                .NotEmpty();
            
            RuleFor(v => v.Tag)
                .NotNull()
                .NotEmpty();
            
            RuleFor(v => v.Destination)
                .NotNull()
                .NotEmpty()
                .Must(s => s.Length == 12).WithMessage("'msisdn' should be 12 digit long")
                .Must(s => s.StartsWith("994")).WithMessage("'msisdn' should start with 994")
                .Matches(@"[0-9]{12}").WithMessage("'msisdn' can only contain numbers");

            RuleFor(v => v.Text)
                .NotNull()
                .NotEmpty()
                .MaximumLength(480).WithMessage("'text' cannot exceed 480 characters");
        }
    }
}