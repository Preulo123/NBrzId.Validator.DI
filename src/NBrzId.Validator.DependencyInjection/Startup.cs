using Microsoft.Extensions.DependencyInjection;

using NBrzId.Common;

using NBrzId.Validator.Implementation;

namespace NBrzId.Validator.DependencyInjection
{
    public static class Startup
    {
        /// <summary>
        /// Registers an instance of <see cref="NBrzId.Validator.IBrzValidator"/> as a singleton to the service collection,
        /// and optionally registers the default validator implementations for CNPJ and CPF.
        /// </summary>
        /// <param name="serviceDescriptors">The service collection to which the types are registered.</param>
        /// <param name="useInternalValidatorImplementations"><c>true</c> to the default validator implementations for CNPJ and CPF; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance, to allow method chaining.</returns>
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
        /// <summary>
        /// Registers an identifier and its validator as singletons to the service collection.
        /// </summary>
        /// <typeparam name="T">The identifier type that implements <see cref="NBrzId.Common.IBrzIdentifier"/>.</typeparam>
        /// <typeparam name="TValidator">
        /// The validator type that implements both <see cref="NBrzId.Validator.IBrzIdentifierValidator{T}"/> 
        /// and <see cref="NBrzId.Validator.IBrzIdentifierValidator{IBrzIdentifier}"/>.
        /// </typeparam>
        /// <param name="serviceDescriptors">The service collection to which the types are registered.</param>
        /// <param name="identifier">The identifier instance to register as a singleton.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance, to allow method chaining.</returns>
        public static IServiceCollection UseValidator<T, TValidator>(this IServiceCollection serviceDescriptors, T identifier)
            where T : class, IBrzIdentifier
            where TValidator : class, IBrzIdentifierValidator<T>, IBrzIdentifierValidator<IBrzIdentifier>
        {
            serviceDescriptors.AddSingleton(identifier);
            serviceDescriptors.AddSingleton<IBrzIdentifierValidator<T>, TValidator>();
            serviceDescriptors.AddSingleton<IBrzIdentifierValidator<IBrzIdentifier>, TValidator>();

            return serviceDescriptors;
        }
    }
}
