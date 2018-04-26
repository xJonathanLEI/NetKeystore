using Newtonsoft.Json;
using NetKeystore.Converters;

namespace NetKeystore.Structures
{
    public class Keystore
    {
        public int version { get; set; }
        public string id { get; set; }
        [JsonConverter(typeof(ByteArrayHexConverter))]
        public byte[] address { get; set; }
        public CryptoSpecs Crypto { get; set; }
    }

    public class CryptoSpecs
    {
        [JsonConverter(typeof(ByteArrayHexConverter))]
        public byte[] ciphertext { get; set; }
        public CipherParams cipherparams { get; set; }
        public string cipher { get; set; }
        public string kdf { get; set; }
        public KdfParams kdfparams { get; set; }
        [JsonConverter(typeof(ByteArrayHexConverter))]
        public byte[] mac { get; set; }
    }

    public class CipherParams
    {
        [JsonConverter(typeof(ByteArrayHexConverter))]
        public byte[] iv { get; set; }
    }

    public class KdfParams
    {
        public int dklen { get; set; }
        [JsonConverter(typeof(ByteArrayHexConverter))]
        public byte[] salt { get; set; }
        public int n { get; set; }
        public int r { get; set; }
        public int p { get; set; }
    }
}