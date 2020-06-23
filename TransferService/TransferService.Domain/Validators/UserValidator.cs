﻿using FluentValidation;
using System.Text.RegularExpressions;

namespace TransferService.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        #region Messages
        public const string Email = "O email é obrigatório";
        public const string EmailFormat = "O formato do email está inválido";
        public const string Password = "A senha é obrigatória.";
        public const string Name = "O nome é obrigatório";
        public const string PostalCode = "O cep é obrigatório";
        public const string PostalCodeInvalid = "O formato do cep está inválido";
        public const string Linkedin = "O seu endereço do linkedin é obrigatório";
        #endregion

        public UserValidator()
        {
            RuleFor(u => u.Email)
               .EmailAddress()
               .WithMessage(EmailFormat)
               .NotEmpty()
               .WithMessage(Email);

            RuleFor(u => u.Name)
                .NotEmpty()
                .WithMessage(Name);

            RuleFor(u => u.Password)
              .NotEmpty()
              .WithMessage(Password)
              .Must(x => x != null && x.Length >= 6 && x.Length <= 32)
              .WithMessage("Senha deve ter entre 6 e 32 letras.");
                
        }

        private bool PostalCodeIsValid(string postalCode)
        {
            Regex Rgx = new Regex(@"^\d{5}-\d{3}$");
            if (string.IsNullOrEmpty(postalCode) || !Rgx.IsMatch(postalCode)) return false;

            return true;
        }
    }
}
