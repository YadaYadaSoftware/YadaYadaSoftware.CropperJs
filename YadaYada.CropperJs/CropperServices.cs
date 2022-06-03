using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace YadaYadaSoftware.CropperJs;

public static class CropperServices
{
    public static IServiceCollection AddCropperServices(this IServiceCollection services)
    {
        services.TryAddSingleton<CropperFactory>();
        return services;
    }
}