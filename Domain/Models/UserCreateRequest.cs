﻿namespace Domain.Models
{
    public class UserCreateRequest
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
        public string Phone {  get; set; }
    }
}
