using System.ComponentModel.DataAnnotations.Schema;

namespace StablingApi.Models
{
    public class TrainingForShow
    {
        public int TrainingId { get; set; }
        public int TrainingTypeId { get; set; }
        public string TypeName { get; set; }
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public int HorseId { get; set; }
        public string HorseName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime TrainingStart { get; set; }
        public DateTime TrainingFinish { get; set; }
    }
}
