using System.ComponentModel.DataAnnotations.Schema;

namespace StablingApi.Models
{
    public class Training
    {
        public int TrainingId { get; set; }
        public int TrainingTypeId { get; set; }
        public int TrainerId { get; set; }
        public int HorseId { get; set; }
        public int ClientId { get; set; }
        public System.DateTime TrainingDateTime { get; set; }
        [Column(TypeName = "decimal(2,1)")]
        public decimal Duration { get; set; }
    }
}