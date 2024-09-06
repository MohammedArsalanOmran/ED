using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OddEvenNumberApp
{
    internal class Program
    {

        static byte[] Encrypt(string Password, byte[] Key, byte[] iv)
        {
            byte[] CipherText;

            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, iv);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(Password);
                        }

                        CipherText = memoryStream.ToArray();
                    }
                }

            }
            return CipherText;
        }

        static string Decrypt(byte[] CipherPassword, byte[] key, byte[] iv)
        {
            string RealPassword = string.Empty;

            using(Aes aes=Aes.Create())
            {
                ICryptoTransform cryptoTransform = aes.CreateDecryptor(key,iv);
                using(MemoryStream memoryStream=new MemoryStream(CipherPassword))
                {
                    using(CryptoStream cryptoStream=new CryptoStream(memoryStream,cryptoTransform,CryptoStreamMode.Read))
                    {
                        using(StreamReader streamReader=new StreamReader(cryptoStream))
                        {
                            RealPassword = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return RealPassword;
        }
        static void Main(string[] args)
        {


            Console.Write("Enter Name :");
            string Name = Console.ReadLine();

            Console.Write("Enter Password :");
            string Password = Console.ReadLine();


            byte[] key = new byte[16];
            byte[] iv = new byte[16];

            using (RandomNumberGenerator rng=RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
                rng.GetBytes(iv);
            }

            byte[] encryptedPassword = Encrypt(Password, key, iv);
            string encryptedPasswordString = Convert.ToBase64String(encryptedPassword);

            Console.WriteLine("Encrypted Password : " + encryptedPasswordString);


            string DecryptedPassword = Decrypt(encryptedPassword, key, iv);
            Console.WriteLine("Decrypted Password : " + DecryptedPassword);












          
            Console.ReadLine();
        }
       
    }
}
