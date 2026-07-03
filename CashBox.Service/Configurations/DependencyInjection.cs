using MediatR;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace CashBox.Service.Configurations
{
    public static class DependencyInjection
    {
        public static void ConfigureServiceApplication(this IHostApplicationBuilder builder)
        {
            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
