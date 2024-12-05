using Microsoft.Extensions.DependencyInjection;
using TwitterClone.Infrastructure.Repositories.Implementations;
using TwitterClone.Infrastructure.Repositories.Interfaces;

namespace ELearningApi.Infrustructure;

public static class ModuleInfrastructureDependencies
{
    public static void AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    }
}