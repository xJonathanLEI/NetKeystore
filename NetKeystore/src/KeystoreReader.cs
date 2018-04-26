using System;
using System.IO;
using System.Text;
using System.Linq;
using CryptSharp;
using Newtonsoft.Json;
using NetKeystore.Cryptography;
using NetKeystore.Exceptions;
using NetKeystore.Structures;

namespace NetKeystore
{
    public class KeystoreReader
    {
        public Keystore Keystore { get; }

        public KeystoreReader(Keystore keystore)
        {
            this.Keystore = keystore;
        }

        public static KeystoreReader FromKeystore(Keystore keystore)
        {
            return new KeystoreReader(keystore);
        }

        public static KeystoreReader FromString(string keystoreContent)
        {
            return new KeystoreReader(JsonConvert.DeserializeObject<Keystore>(keystoreContent));
        }

        public static KeystoreReader FromFile(string fileName)
        {
            return new KeystoreReader(JsonConvert.DeserializeObject<Keystore>(File.ReadAllText(fileName, Encoding.UTF8)));
        }

        public bool TryDecrypt(string passphase, out byte[] privateKey)
        {
            // Computes decryption key
            byte[] decryptionKey;

            switch (Keystore.Crypto.kdf)
            {
                case "scrypt":
                    decryptionKey = ApplyScrypt(passphase);
                    break;
                default:
                    throw new KdfNotSupportedException();
            }

            // Verifies decryption key
            byte[] data = new byte[16 + Keystore.Crypto.ciphertext.Length];
            Array.Copy(decryptionKey, 16, data, 0, 16);
            Array.Copy(Keystore.Crypto.ciphertext, 0, data, 16, Keystore.Crypto.ciphertext.Length);
            using (var keccak = new KeccakUnmanaged(256))
            {
                if (!keccak.ComputeHash(data).SequenceEqual(Keystore.Crypto.mac))
                {
                    privateKey = null;
                    return false;
                }
            }

            // Decrypts private key
            switch (Keystore.Crypto.cipher)
            {
                case "aes-128-ctr":
                    privateKey = ApplyAes128Ctr(decryptionKey);
                    break;
                default:
                    throw new CipherNotSupportedException();
            }

            return true;
        }

        private byte[] ApplyScrypt(string passphase)
        {
            byte[] output = new byte[Keystore.Crypto.kdfparams.dklen];
            CryptSharp.Utility.SCrypt.ComputeKey(
                Encoding.UTF8.GetBytes(passphase),
                Keystore.Crypto.kdfparams.salt,
                Keystore.Crypto.kdfparams.n,
                Keystore.Crypto.kdfparams.r,
                Keystore.Crypto.kdfparams.p,
                null,
                output);
            return output;
        }

        private byte[] ApplyAes128Ctr(byte[] decryptionKey)
        {
            byte[] result = new byte[32];
            var am = new Aes128CounterMode(Keystore.Crypto.cipherparams.iv);

            byte[] slicedKey = new byte[16];
            Array.Copy(decryptionKey, 0, slicedKey, 0, 16);

            var ict = am.CreateDecryptor(slicedKey, null);
            ict.TransformBlock(Keystore.Crypto.ciphertext, 0, Keystore.Crypto.ciphertext.Length, result, 0);

            return result;
        }
    }
}
