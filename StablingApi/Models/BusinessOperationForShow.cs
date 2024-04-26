namespace StablingApi.Models
{
    public class BusinessOperationForShow
    {
        public int BusinessOperationId { get; set; }
        public string AccountName { get; set; }
        public string TypeName { get; set; }
        public DateTime OperationDateTime { get; set; }
        public decimal Amount { get; set; }
        public bool IsIncome { get; set; }
        public string? Comment { get; set; }
    }
}