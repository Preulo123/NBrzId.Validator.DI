using System;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

using NBrzId.Common;

using NBrzId.Validator.DependencyInjection;
using NBrzId.Validator.DI.UnitTests.Mocks;

namespace NBrzId.Validator.DI.UnitTests
{
    public sealed class StartupTests
    {
        private readonly Type brzValidatorType;
        private readonly Type cnpjValidatorImplType;
        private readonly Type cpfValidatorImplType;

        public StartupTests()
        {
            var nBrzIdValidatorAssembly = typeof(IBrzValidator);

            brzValidatorType = nBrzIdValidatorAssembly.Assembly.GetType("NBrzId.Validator.Implementation.BrzValidator")!;
            cnpjValidatorImplType = nBrzIdValidatorAssembly.Assembly.GetType("NBrzId.Validator.Implementation.CnpjValidatorImpl")!;
            cpfValidatorImplType = nBrzIdValidatorAssembly.Assembly.GetType("NBrzId.Validator.Implementation.CpfValidatorImpl")!;
        }

        [Fact]
        public void UseBrzValidators_ShouldRegister_BrzValidator()
        {
            var services = new ServiceCollection();
            services.UseBrzValidators();

            var provider = services.BuildServiceProvider();

            var validator = provider.GetService<IBrzValidator>();
            Assert.NotNull(validator);
            Assert.IsType(brzValidatorType, validator);

            // deve ser singleton
            var v1 = provider.GetService<IBrzValidator>();
            var v2 = provider.GetService<IBrzValidator>();
            Assert.Same(v1, v2);
        }

        [Fact]
        public void UseBrzValidators_WithInternalValidators_ShouldRegisterCnpjAndCpfValidators()
        {
            var services = new ServiceCollection();
            services.UseBrzValidators(useInternalValidatorImplementations: true);

            var provider = services.BuildServiceProvider();

            var cnpjValidator = provider.GetService<IBrzIdentifierValidator<Cnpj>>();
            Assert.NotNull(cnpjValidator);
            Assert.IsType(cnpjValidatorImplType, cnpjValidator);

            var cpfValidator = provider.GetService<IBrzIdentifierValidator<Cpf>>();
            Assert.NotNull(cpfValidator);
            Assert.IsType(cpfValidatorImplType, cpfValidator);

            var genericCnpj = provider.GetServices<IBrzIdentifierValidator<IBrzIdentifier>>();
            Assert.Contains(genericCnpj, v => cnpjValidatorImplType.IsAssignableFrom(v.GetType()));

            var genericCpf = provider.GetServices<IBrzIdentifierValidator<IBrzIdentifier>>();
            Assert.Contains(genericCpf, v => cpfValidatorImplType.IsAssignableFrom(v.GetType()));
        }

        [Fact]
        public void UseBrzValidators_WithoutInternalValidators_ShouldNotRegisterCnpjAndCpfValidators()
        {
            var services = new ServiceCollection();
            services.UseBrzValidators(useInternalValidatorImplementations: false);

            var provider = services.BuildServiceProvider();

            Assert.Null(provider.GetService<IBrzIdentifierValidator<Cnpj>>());
            Assert.Null(provider.GetService<IBrzIdentifierValidator<Cpf>>());
        }

        [Fact]
        public void UseValidator_ShouldRegisterIdentifierAndValidator()
        {
            var services = new ServiceCollection();
            services.UseValidator<ExternalIdentifier, ExternalIdentifierValidator>(new ExternalIdentifier());

            var provider = services.BuildServiceProvider();

            var identifier = provider.GetService<ExternalIdentifier>();
            Assert.NotNull(identifier);

            var validator = provider.GetService<IBrzIdentifierValidator<ExternalIdentifier>>();
            Assert.NotNull(validator);
            Assert.IsType<ExternalIdentifierValidator>(validator);

            var generic = provider.GetService<IBrzIdentifierValidator<IBrzIdentifier>>();
            Assert.NotNull(generic);
            Assert.IsType<ExternalIdentifierValidator>(generic);
        }
    }
}
