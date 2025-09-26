using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

using NBrzId.Common;

using NBrzId.Validator.Implementation;

namespace NBrzId.Validator.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class Startup
    {
        public static IServiceCollection UseBrzValidators(this IServiceCollection serviceDescriptors, bool useInternalValidatorImplementations = true)
        {
            if (useInternalValidatorImplementations)
            {
                serviceDescriptors.UseValidator<Cnpj, CnpjValidatorImpl>(BrzIdentifier.Cnpj);
                serviceDescriptors.UseValidator<Cpf,  CpfValidatorImpl>(BrzIdentifier.Cpf);
            }

            serviceDescriptors.AddSingleton<IBrzValidator, BrzValidator>();

            return serviceDescriptors;
        }

        public static IServiceCollection UseValidator<T, TValidator>(this IServiceCollection services, T identifier)
            where T : class, IBrzIdentifier
            where TValidator : class, IBrzIdentifierValidator<T>, IBrzIdentifierValidator<IBrzIdentifier>
        {
            services.AddSingleton(identifier);
            services.AddSingleton<IBrzIdentifierValidator<T>, TValidator>();
            services.AddSingleton<IBrzIdentifierValidator<IBrzIdentifier>, TValidator>();

            return services;
        }
    }
}
