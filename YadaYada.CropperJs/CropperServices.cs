using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace YadaYada.CropperJs;

public static class CropperServices
{
    public static IServiceCollection AddCropperServices(this IServiceCollection services)
    {
        services.TryAddSingleton<CropperFactory>();
        return services;
    }
}