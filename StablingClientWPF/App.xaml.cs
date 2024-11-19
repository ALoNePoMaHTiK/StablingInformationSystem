using Microsoft.Extensions.DependencyInjection;
using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.ViewModels;
using StablingClientWPF.Views;
using System.Net.Http;
using System.Windows;

namespace StablingClientWPF
{
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddHttpClient();
            const string ApiUrl = "http://localhost:5000";
            services.AddSingleton(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new ClientsHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new TrainersHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new TrainingsHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new HorsesHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new MoneyAccountsHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new MoneyTransactionsHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new BusinessOperationsHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new BalanceReplenishmentsHttpClient(ApiUrl, client);
                return httpClient;
            });

            services.AddSingleton(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new BalanceWithdrawingsHttpClient(ApiUrl, client);
                return httpClient;
            });

            services.AddSingleton(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new AbonementsHttpClient(ApiUrl, client);
                return httpClient;
            });

            services.AddSingleton<Mediator>();
            services.AddSingleton<DialogManager>();


            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ClientsViewModel>();
            services.AddSingleton<TrainingsViewModel>();
            services.AddSingleton<TrainingTypesViewModel>();
            services.AddSingleton<AdministrationViewModel>();
            services.AddSingleton<AbonementsViewModel>();

            services.AddSingleton<DayOperationsViewModel>();
            services.AddSingleton<MoneyTransactionsViewModel>();
            services.AddSingleton<BusinessOperationsViewModel>();
            services.AddSingleton<BalanceReplenishmentsViewModel>();
            services.AddSingleton<BalanceWithdrawingsViewModel>();
        } 
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
