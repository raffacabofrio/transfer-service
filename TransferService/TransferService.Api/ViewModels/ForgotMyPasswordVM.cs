using System.ComponentModel.DataAnnotations;

namespace TransferService.Api.ViewModels
{
    public class ForgotMyPasswordVM
    {
        [Required]
        public string Email { get; set; }
    }
}
