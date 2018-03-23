namespace BNE.Cryptography
{
    public interface ICryptography
    {
        string Encrypt(string plainText);
        string Decrypt(string plainText);
        string Version { get; }
    }
}
