namespace StablingApi.Models
{
    public class AbonementType
    {
        public int AbonementTypeId { get; set; }
        public string TypeName { get; set; }
        public int TypeUsesCount { get; set; }
        public int TypePrice { get; set; }
        public bool IsAvailable { get; set; }
    }
}