namespace NBrzId.Validator.DI.UnitTests.Mocks
{
    internal sealed class ExternalIdentifierValidator : IBrzIdentifierValidator<ExternalIdentifier>
    {
        public bool ApplyValidation(string value, string auxiliaryValue = null, bool removeFormatters = true, bool pad = false) => true;
    }
}
