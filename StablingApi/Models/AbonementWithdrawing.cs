namespace StablingApi.Models
{
    public class AbonementWithdrawing
    {
        public int AbonementId { get; set; }
        public Guid BalanceWithdrawingId { get; set; }

        public AbonementWithdrawing(int abonementId, Guid balanceWithdrawingId)
        {
            AbonementId = abonementId;
            BalanceWithdrawingId = balanceWithdrawingId;
        }
    }
}