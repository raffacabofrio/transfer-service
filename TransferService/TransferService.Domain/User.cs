﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TransferService.Domain.Common;
using TransferService.Domain.Enums;

namespace TransferService.Domain
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string HashCodePassword { get; set; }
        public DateTime HashCodePasswordExpiryDate { get; set; }
        public DateTime LastLogin { get; set; } = DateTime.Now;
        public string Linkedin { get; set; }
        public  string Phone{ get; set; }
        public Profile Profile { get;  set; } = Profile.User;
        public bool Active { get; set; } = true;
        public bool AllowSendingEmail { get; set; } = true;
        public virtual Address Address { get; set; }
        public virtual BankAccount BankAccount { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }

        public bool PasswordIsStrong()
        {
            Regex rgx = new Regex(@"(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])[A-Za-z0-9\d$@$!%_*_?&#.,-_:;]{8,}");
            if (string.IsNullOrEmpty(Password) || !rgx.IsMatch(Password)) return false;

            return true;
        }

        public User Cleanup()
        {
            this.Password = string.Empty;
            this.PasswordSalt = string.Empty;
            return this;
        }

        public void GenerateHashCodePassword()
        {
            this.HashCodePassword =  Guid.NewGuid().ToString();
            this.HashCodePasswordExpiryDate = DateTime.Now.AddDays(1); 
        }

        public bool HashCodePasswordIsValid(string hashCodePassword)
             => hashCodePassword == this.HashCodePassword 
                && (this.HashCodePasswordExpiryDate.Date == DateTime.Now.AddDays(1).Date
                   || this.HashCodePasswordExpiryDate.Date == DateTime.Now.Date);

        public void Change(string email, string name, string linkedin, string phone, bool AllowSendingEmail)
        {
            this.Email = email;
            this.Name = name;
            this.Linkedin = linkedin;
            this.Phone = phone;
            this.AllowSendingEmail = AllowSendingEmail;
        }

        public void ChangeAddress(Address address)
        {
            var AddressIdCopy = this.Address.Id;
            this.Address = address;

            this.Address.UserId = Id;
            this.Address.Id = AddressIdCopy;
        }

        public void ChangePassword(string password)
        {
            this.Password = password;
        }

        public bool IsBruteForceLogin()
        {
            var refDate = DateTime.Now.AddSeconds(-30);
            return LastLogin > refDate;
        }

        public string Location() => Address.City + "-" + Address.State;

    }
}
