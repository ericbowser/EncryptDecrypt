using System.Security.Cryptography;
using System.Text;

namespace EncryptDecrypt
{
    public static class Utils
    {
        public async static Task EncryptPass()
        {
            Console.WriteLine("Encrypt of Decrypt (1 or 2)");
            var encryptOrDecrypt = Console.ReadLine();

            if (encryptOrDecrypt == "2")
            {
                Console.WriteLine("Type userName to search records: ");
                var userToSearch = Console.ReadLine();
                await VerifyEncryption(userToSearch);
                return;
            }

            Console.WriteLine("Enter userName name: ");
            var userName = Console.ReadLine();

            Console.WriteLine("Enter string to encrypt: ");

            var password = new StringBuilder();
            ConsoleKeyInfo keyInfo;
            var read = Console.ReadKey();

            password.Append(read.Key.ToString());
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b"); // Erase the asterisk
                }
                else if (keyInfo.Key != ConsoleKey.Backspace)
                {
                    password.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }
            }
            var pass = password.ToString();
            if (!string.IsNullOrEmpty(pass))
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(pass);
                // Generate RSA keys and encrypt password
                using (RSA rsa = RSA.Create())
                {
                    // Set RSA key size
                    rsa.KeySize = 2048;

                    // Generate public and private keys
                    RSAParameters rsaKeyInfo = rsa.ExportParameters(true);
                    var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                    var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

                    // Encrypt the password
                    rsa.ImportParameters(rsaKeyInfo);
                    var encryptedPassword = rsa.Encrypt(passwordBytes, RSAEncryptionPadding.OaepSHA1);

                    var repo = new StoreStuffRespoitory();
                    await repo.SaveKey(userName, encryptedPassword, pass, privateKey);

                    var y = new StoreStuffRespoitory();
                    var key = await y.GetSavedKey(userName);

                    var decrypted = rsa.Decrypt(key.EncryptedPassword, RSAEncryptionPadding.OaepSHA1);
                    var str = Encoding.UTF8.GetString(decrypted);

                    Console.WriteLine(decrypted);
                }
            }
        }

        public async static Task VerifyEncryption(string userName)
        {
            Console.WriteLine("Verifying encrypted key");


            var repo = new StoreStuffRespoitory();
            var key = await repo.GetSavedKey(userName);

            try
            {
                if (key != null)
                {
                    using (RSA rsa = RSA.Create())
                    {
                        // Set RSA key size
                        rsa.KeySize = 2048;
                        // Generate public and private keys
                        RSAParameters rsaKeyInfo = rsa.ExportParameters(true);
                        var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                        var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

                     

                        var x = rsa.Decrypt(key.EncryptedPassword, RSAEncryptionPadding.OaepSHA1);
                        Console.WriteLine(x);
                    }
                }
                else
                {
                    Console.WriteLine("That userName was not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
