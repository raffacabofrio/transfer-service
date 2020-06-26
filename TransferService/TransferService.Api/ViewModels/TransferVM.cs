using System.ComponentModel.DataAnnotations;

namespace TransferService.Api.ViewModels
{
    public class TransferVM : BaseViewModel
    {
        [Required(ErrorMessage = "A conta de origem é obrigatória e deve ser um número.")]
        public int AccountNumberOrigin { get; set; }

        [Required(ErrorMessage = "A conta de destino é obrigatória e deve ser um número.")]
        public int AccountNumberDestination { get; set; }

        [Required(ErrorMessage = "O Valor é obrigatório.")]
        public decimal Value { get; set; }
    }
}
