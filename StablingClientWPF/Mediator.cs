using StablingApiClient;

namespace StablingClientWPF
{
    public class Mediator
    {
        public event Action<DateTime> GetDayOperationsDate;
        public void UpdateDayOperationsDate(DateTime date)
            => GetDayOperationsDate?.Invoke(date);

        public event Action<int> GetClientInfo;
        public void ShowClientInfo(int clientId)
            => GetClientInfo?.Invoke(clientId);

        public event Action<Training> NeedToCreateTrainingWithdrawing;
        public void CreateTrainingWithdrawing(Training trining)
            => NeedToCreateTrainingWithdrawing?.Invoke(trining);
    }
}