using System.ComponentModel.DataAnnotations;
using System.Runtime.Loader;

namespace MoneyService.Models.Transactions
{
    public class TransferModel
    {
        public long SenderAccountId { get; set; }
        
        [Required]
        public long RecipientAccountId { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}