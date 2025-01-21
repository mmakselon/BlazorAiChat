using BlazorAiChat.Components;
using BlazorAiChat.Models;

namespace BlazorAiChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var openAiApiKey = builder.Configuration["OpenAI:ApiKey"] ?? throw new Exception("Brak klucza API OpenAI w appsettings.json");

            builder.Services.AddHttpClient("OpenAI");
            builder.Services.AddScoped<OpenAiService>(sp =>
            {
                var httpClientFactory= sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("OpenAI");
                return new OpenAiService(httpClient,openAiApiKey);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
