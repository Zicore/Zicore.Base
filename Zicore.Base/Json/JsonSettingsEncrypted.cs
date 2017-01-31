using System;
using System.Security.Cryptography;
using System.Text;
using Zicore.Base.Cryptography;

namespace Zicore.Base.Json
{
    public class JsonSettingsEncrypted : JsonSerializable
    {
        public JsonSettingsEncrypted(String key)
        {
            SetKey(key);
        }

        public JsonSettingsEncrypted()
        {

        }

        private readonly byte[] _initVector = new byte[]
        {
            2, 5, 6, 1, 51, 55, 5, 1, 66, 1, 5, 231, 5, 1, 7, 5
        };

        private readonly byte[] _saltBuffer = new byte[]
        {
            2, 5, 6, 1, 51, 55, 5, 1, 66, 1, 5, 231, 5, 66, 1, 1, 55, 55, 3, 5, 61, 5, 6, 15, 56, 2, 45, 139, 50, 24
        };

        private readonly SHA256 _sha256 = new SHA256Managed();
        private byte[] _key;

        public void SetKey(String stringKey)
        {
            if (String.IsNullOrWhiteSpace(stringKey))
                throw new ArgumentException("Argument must be not null and not whitespace", nameof(stringKey));
            stringKey = stringKey.Trim();

            _key = HashBase.HashWithIterations(256, _sha256, Encoding.UTF8.GetBytes(stringKey));
        }

        protected override byte[] LoadFilter(byte[] data)
        {
            if (_key == null || _key.Length == 0)
                throw new CryptographicException("Key not set");

            var aes = new RijndaelBase(_saltBuffer, _initVector, 256);
            data = aes.Decrypt(_key, data);
            return base.LoadFilter(data);
        }

        protected override byte[] SaveFilter(byte[] data)
        {
            if (_key == null || _key.Length == 0)
                throw new CryptographicException("Key not set");

            var aes = new RijndaelBase(_saltBuffer, _initVector, 256);
            data = aes.Encrypt(_key, data);
            return base.SaveFilter(data);
        }
    }
}
