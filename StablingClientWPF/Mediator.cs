namespace StablingClientWPF
{
    public class Mediator
    {
        public event Action<DateTime> GetDayOperationsDate;
        public void UpdateDayOperationsDate(DateTime date)
            => GetDayOperationsDate?.Invoke(date);
    }
}