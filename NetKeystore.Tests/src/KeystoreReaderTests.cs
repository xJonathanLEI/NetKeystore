using System;
using System.IO;
using System.Text;
using Xunit;

namespace NetKeystore.Tests
{
    public class KeystoreReaderTests
    {
        [Fact]
        public void ScrytpAes128CtrTest()
        {
            TestFilesInFolder("../../../data/scrypt-aes-128-ctr");
        }

        private void TestFilesInFolder(string folder)
        {
            string dir = Directory.GetCurrentDirectory();
            foreach (string file in Directory.GetFiles(folder))
            {
                string privateKeyString = file.Substring(file.Replace("\\", "/").LastIndexOf("/") + 1);
                privateKeyString = privateKeyString.Remove(privateKeyString.IndexOf(".")).ToLower();

                var reader = KeystoreReader.FromFile(file);
                // this is the wrong key
                Assert.False(reader.TryDecrypt("ABCD12345", out _));
                // this is the correct key
                Assert.True(reader.TryDecrypt("ASDF12345", out var privateKey));

                var hex = new StringBuilder();
                foreach (byte b in privateKey)
                    hex.Append(b.ToString("x2"));

                Assert.Equal(privateKeyString, hex.ToString());
            }
        }
    }
}
