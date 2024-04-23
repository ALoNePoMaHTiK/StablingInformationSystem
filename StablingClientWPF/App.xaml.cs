using Microsoft.Extensions.DependencyInjection;
using StablingApiClient;
using StablingClientWPF.ViewModels;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace StablingClientWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
            services.AddSingleton<ClientsHttpClient>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new ClientsHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton<TrainersHttpClient>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new TrainersHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton<TrainingsHttpClient>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new TrainingsHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton<TrainingTypesHttpClient>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new TrainingTypesHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton<HorsesHttpClient>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new HorsesHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton<MoneyTransactionsHttpClient>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new MoneyTransactionsHttpClient(ApiUrl, client);
                return httpClient;
            });
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ClientsViewModel>();
            services.AddSingleton<TrainingsViewModel>();
            services.AddSingleton<TrainingTypesViewModel>();
            services.AddSingleton<AdministrationViewModel>();
            services.AddSingleton<MoneyViewModel>();
            services.AddSingleton<MoneyTransactionsViewModel>();
            services.AddSingleton<BusinessOperationsViewModel>();
        } 
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }

}
