namespace StablingApi.Models
{
    public class TrainingWithdrawing
    {
        public int TrainingId { get; set; }
        public Guid BalanceWithdrawingId { get; set; }

        public TrainingWithdrawing(int trainingId, Guid balanceWithdrawingId)
        {
            TrainingId = trainingId;
            BalanceWithdrawingId = balanceWithdrawingId;
        }
    }
}