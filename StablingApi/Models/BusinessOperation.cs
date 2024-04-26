using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace StablingApi.Models
{
    public class BusinessOperation
    {
        public int BusinessOperationId { get; set; }
        public int OperationTypeId { get; set; }
        public byte MoneyAccountId { get; set; }
        [Column(TypeName = "SMALLMONEY")]
        public decimal Amount { get; set; }
        public DateTime OperationDateTime { get; set; }
        public string? Comment { get; set; }
    }
}