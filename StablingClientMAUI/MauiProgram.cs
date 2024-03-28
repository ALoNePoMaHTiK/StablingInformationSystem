using Microsoft.Extensions.Logging;
using StablingApiClient;
using StablingClientMAUI.ViewModels;
using StablingClientMAUI.Views;
using System.Net.Http;

namespace StablingClientMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            const string ApiUrl = "http://localhost:5000";
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<ClientsHttpClient>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new ClientsHttpClient(ApiUrl, client);
                return httpClient;
            });
            builder.Services.AddSingleton<TrainingTypesHttpClient>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient();
                var httpClient = new TrainingTypesHttpClient(ApiUrl, client);
                return httpClient;
            });


            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ClientsPage>();
            return builder.Build();
        }
    }
}
