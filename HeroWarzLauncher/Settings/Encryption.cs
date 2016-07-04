using System;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace HeroWarzLauncher.Settings
{
    internal class Encryption
    {
        private string _rfcPassword = string.Empty;
        private string RfcPassword
        {
            get
            {
                return _rfcPassword;
            }

            set
            {
                _rfcPassword = value;
            }
        }

        private byte[] _rfcSalt = null;
        private byte[] RfcSalt
        {
            get
            {
                return _rfcSalt;
            }

            set
            {
                _rfcSalt = value;
            }
        }

        internal Encryption()
        {
            // Password
            ManagementObjectCollection managementObjects = null;
            ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("SELECT ProcessorID FROM Win32_Processor");
            managementObjects = objectSearcher.Get();
            string processorId = string.Empty;
            try
            {
                foreach (ManagementObject mo in managementObjects)
                {
                    try
                    {
                        processorId = mo["ProcessorID"].ToString();
                        break;
                    }
                    catch { }
                }
            }
            catch { }

            RfcPassword = processorId;
            for (int i = RfcPassword.Length; i < 16; i++)
                RfcPassword += "0";
            RfcPassword = RfcPassword.Substring(0, 16);

            // Salt
            managementObjects = null;
            objectSearcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_PhysicalMedia");
            managementObjects = objectSearcher.Get();
            string driveSerial = string.Empty;
            try
            {
                foreach (ManagementObject mo in managementObjects)
                {
                    try
                    {
                        processorId = mo["SerialNumber"].ToString();
                        break;
                    }
                    catch { }
                }
                if (!string.IsNullOrEmpty(processorId))
                    driveSerial = processorId;
            }
            catch { }

            for (int i = driveSerial.Length; i < 14; i++)
                driveSerial += "0";
            driveSerial = driveSerial.Substring(0, 14);
            RfcSalt = Encoding.ASCII.GetBytes(driveSerial);
        }

        public string EncryptString(string text)
        {
            string encryptedString = null;
            RijndaelManaged aes = null;
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(RfcPassword, RfcSalt);
                aes = new RijndaelManaged();
                aes.Key = key.GetBytes(aes.KeySize / 8);
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(BitConverter.GetBytes(aes.IV.Length), 0, sizeof(int));
                    memoryStream.Write(aes.IV, 0, aes.IV.Length);
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(text);
                        }
                    }
                    encryptedString = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            finally
            {
                if (aes != null)
                    aes.Clear();
            }
            return encryptedString;
        }
        public string DecryptString(string encryptedString)
        {
            RijndaelManaged aes = null;
            string text = string.Empty;
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(RfcPassword, RfcSalt);
                byte[] bytes = Convert.FromBase64String(encryptedString);
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    aes = new RijndaelManaged();
                    aes.Key = key.GetBytes(aes.KeySize / 8);

                    byte[] length = new byte[sizeof(int)];
                    if (memoryStream.Read(length, 0, length.Length) != length.Length)
                        return string.Empty;
                    byte[] buffer = new byte[BitConverter.ToInt32(length, 0)];
                    if (memoryStream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        return string.Empty;


                    aes.IV = buffer;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(cryptoStream))
                            text = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aes != null)
                    aes.Clear();
            }
            return text;
        }
    }
}
