namespace StablingApi.Models
{
    public class AbonementForShow
    {
        public int AbonementId { get; set; }
        public string TypeName { get; set; }
        public string FullName { get; set; }
        public int MaxUses { get; set; }
        public int UsesCount { get; set; }
        public bool IsAvailable { get; set; }
    }
}
