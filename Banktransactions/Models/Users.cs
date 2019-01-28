using System;
using System.ComponentModel.DataAnnotations;

namespace BankUser.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Email não pode ser nulo.")]
        public string email { get; set; }
        [Required(ErrorMessage = "O campo Passworld não pode ser nulo.")]
        public string pwd { get; set; }
    }
}
