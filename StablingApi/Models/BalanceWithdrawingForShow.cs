namespace StablingApi.Models
{
    public class BalanceWithdrawingForShow
    {
        public Guid BalanceWithdrawingId { get; set; }
        public string TrainerName { get; set; }
        public string ClientName { get; set; }
        public double Amount { get; set; }
        public DateTime WithdrawingDate { get; set; }
    }
}
