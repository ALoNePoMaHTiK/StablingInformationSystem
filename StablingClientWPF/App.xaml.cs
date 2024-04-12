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
            services.AddSingleton<MainWindow>();
        } 
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }

}
