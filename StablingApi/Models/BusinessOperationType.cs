using System.ComponentModel.DataAnnotations.Schema;

namespace StablingApi.Models
{
    public class BusinessOperationType
    {
        public int BusinessOperationTypeId { get; set; }
        public string TypeName { get; set; }
        [Column(TypeName = "SMALLMONEY")]
        public decimal TypeAmount { get; set; }
        public bool IsIncome { get; set; }
    }
}