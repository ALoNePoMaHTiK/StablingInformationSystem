using System.ComponentModel.DataAnnotations.Schema;

namespace StablingApi.Models
{
    public class TrainingType
    {
        public int TrainingTypeId { get; set; }
        public string TypeName { get; set; }
        public int TypePrice { get; set; }
        [Column(TypeName = "decimal(2,1)")]
        public decimal TypeDuration { get; set; }
        public bool IsAvailable { get; set; }
    }
}