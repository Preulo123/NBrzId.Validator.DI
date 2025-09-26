using NBrzId.Common;

namespace NBrzId.Validator.DI.UnitTests.Mocks
{
    internal sealed class ExternalIdentifier : IBrzIdentifier
    {
        public char[] FormattingCharacters => new char[] { '.' };

        public char PaddingCharacter => '0';

        public int Length => 5;

        public string Mask => "NNNN/N";
    }
}
