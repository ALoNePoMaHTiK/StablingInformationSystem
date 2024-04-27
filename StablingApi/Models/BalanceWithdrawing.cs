namespace StablingApi.Models
{
    public class BalanceWithdrawing
    {
        public Guid BalanceWithdrawingId { get; set; }
        public int ClientId { get; set; }
        public DateTime WithdrawingDate { get; set; }
        public double Amount { get; set; }
        public string WithdrawingCause { get; set; }
    }
}