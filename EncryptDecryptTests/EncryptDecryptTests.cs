using EncryptDecrypt;

namespace EncryptDecryptTests
{
    public class EncryptDecryptTests
    {
        [Fact]
        public async Task Test1()
        {
            await Utils.EncryptPass();

        }
    }
}