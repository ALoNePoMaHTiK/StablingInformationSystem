using StablingApiClient;

namespace StablingClientWPF.Helpers
{
    public class Mediator
    {
        //Обновление суточных данных при обновлении даты
        public event Action<DateTime> OnDayOperationsDateUpdated;
        public void UpdateDayOperationsDate(DateTime date)
            => OnDayOperationsDateUpdated?.Invoke(date);

        public event Action<int> GetClientInfo;
        public void ShowClientInfo(int clientId)
            => GetClientInfo?.Invoke(clientId);

        //Обновление суммы списаний по тренировке
        public event Action<int> OnTrainingFundsUpdated;
        public void UpdateTrainingFunds(int trainingId)
            => OnTrainingFundsUpdated?.Invoke(trainingId);

        //Обновление суммы списаний по абонементу
        public event Action<int> OnAbonementFundsUpdated;
        public void UpdateAbonementFunds(int trainingId)
            => OnAbonementFundsUpdated?.Invoke(trainingId);
        

        public event Action<BalanceReplenishment> OnReplenishmentByTrainingCreateNotified;
        public void NotifyReplenishmentByTrainingCreate(BalanceReplenishment replenishment)
            => OnReplenishmentByTrainingCreateNotified?.Invoke(replenishment);

        //old
        public event Action<Training> NeedToCreateTrainingWithdrawing;
        public void CreateTrainingWithdrawing(Training trining)
            => NeedToCreateTrainingWithdrawing?.Invoke(trining);
    }
}