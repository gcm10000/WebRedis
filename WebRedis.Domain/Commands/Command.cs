using FluentValidation.Results;
using MediatR;
using System;

namespace WebRedis.Domain.Commands
{
    public class Command : IRequest<ValidationResult>
    {
        public Guid Id { get; set; }

        public string User { get; set; }

        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            ValidationResult = new ValidationResult();
        }

        public virtual bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
