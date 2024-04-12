namespace StablingApi.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }
        public string FullName { get; set; }
        public string Color { get; set; }
        public int SalaryRate { get; set; }
        public bool IsAvailable { get; set; }
    }
}