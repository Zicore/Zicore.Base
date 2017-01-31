using System.IO;
using System.Security.Cryptography;

namespace Zicore.Base.Cryptography
{
    public class RijndaelBase
    {
        private int _passwordIterations = 1000;

        

        private byte[] _salt;
        private byte[] _initVector;
        private int _keySize = 256;

        public RijndaelBase(byte[] salt, byte[] initVector, int keySize)
        {
            _salt = salt;
            _initVector = initVector;
            _keySize = keySize;
        }

        public byte[] Encrypt(byte[] password, byte[] buffer)
        {
            byte[] plainTextBytes = buffer;

            var passwordDerivedBytes = new Rfc2898DeriveBytes(password, _salt, _passwordIterations);

            byte[] keyBytes = passwordDerivedBytes.GetBytes(_keySize / 8);

            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, _initVector);
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] cipherTextBytes = memoryStream.ToArray();
                    return cipherTextBytes;
                }
            }
        }

        public byte[] Decrypt(byte[] password, byte[] buffer)
        {
            byte[] cipherTextBytes = buffer;

            var passwordDerivedBytes = new Rfc2898DeriveBytes(password, _salt, _passwordIterations);

            byte[] keyBytes = passwordDerivedBytes.GetBytes(_keySize / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, _initVector);
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    var plainTextBytes = new byte[cipherTextBytes.Length];
                    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    byte[] plainFile = plainTextBytes;
                    return plainFile;
                }
            }
        }
    }
}