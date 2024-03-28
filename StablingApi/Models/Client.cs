namespace StablingApi.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public int TrainerId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public double Balance { get; set; }
        public string Email { get; set; }
        public bool IsAvailable { get; set; }
    }
}