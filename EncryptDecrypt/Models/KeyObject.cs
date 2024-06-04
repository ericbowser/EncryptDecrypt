namespace EncryptDecrypt.Models
{
    public class KeyObject
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] EncryptedPassword { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
