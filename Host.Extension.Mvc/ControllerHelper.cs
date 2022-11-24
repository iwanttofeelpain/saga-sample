using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Host.Extension.Mvc;

public static class ControllerHelper
{
    public static IServiceCollection AddControllersFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        var mvcBuilder = services.AddControllers();

        foreach (var assembly in assemblies)
        {
            mvcBuilder.AddApplicationPart(assembly);
        }

        return mvcBuilder.Services;
    }
}