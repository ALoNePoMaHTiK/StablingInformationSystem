namespace StablingApi.Models
{
    public class Abonement
    {
        public int AbonementId { get; set; }
        public int ClientId { get; set; }
        public int AbonementTypeId { get; set; }
        public int UsesCount { get; set; }
        public System.DateTime OpenDateTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}