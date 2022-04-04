using DataAbstraction.Models;
using FluentValidation;

namespace DataValidationService
{
    public sealed class BookRequestValidation : AbstractValidator<BookRequest>
    {
        public BookRequestValidation()
        {
            RuleFor(x => x.Name)
                .Length(3, 30)
                    .WithMessage("M_101.1 Имя книги '{PropertyValue}' длинна должна быть от трех до 30 символов")
                .Matches("^[a-zA-ZА-Яа-я0-9._@#$%\\- ]+$")
                    .WithMessage("M_101.2 Имя книги {PropertyValue} должно состоять только из символов латинского или русского алфавита, цифр, символов ._@#$%- ");

            RuleFor(x => x.Category)
                .MaximumLength(30)
                    .WithMessage("M_102.1 Категория '{PropertyValue}' длинна должна быть не более 30 символов")
                .Matches("^[a-zA-ZА-Яа-я0-9._@#$%\\- ]+$")
                    .WithMessage("M_102.2 Категория {PropertyValue} должно состоять только из символов латинского или русского алфавита, цифр, символов ._@#$% ");

            RuleFor(x => x.Author)
                .Length(3, 30)
                    .WithMessage("M_103.1 Имя автора '{PropertyValue}' длинна должна быть от трех до 30 символов")
                .Matches("^[a-zA-ZА-Яа-я- ]+$")
                    .WithMessage("M_103.2 Имя автора {PropertyValue} должно состоять только из символов латинского или русского алфавита и символа -");

            RuleFor(x => x.Description)
                .MaximumLength(60)
                    .WithMessage("M_104.1 Описание - длинна должна быть не более 60 символов")
                .Matches("^[a-zA-ZА-Яа-я0-9._@#$%\\- ]+$")
                    .WithMessage("M_104.2 Описание должно состоять только из символов латинского или русского алфавита, цифр, символов ._@#$% ");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("M_105.1 Цена должна быть не указана или 0 или положительным числом");
        }
    }
}