using System.Runtime.InteropServices;
using System.Text;

namespace HeroWarzLauncher.Settings
{
    internal class IniFile
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string value, string IniFilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder returnValue, int returnLength, string IniFilePath);

        private string _iniFilePath = string.Empty;
        private string IniFilePath
        {
            get
            {
                return _iniFilePath;
            }

            set
            {
                _iniFilePath = value;
            }
        }

        private Encryption _encryption = null;
        internal Encryption Encryption
        {
            get
            {
                return _encryption;
            }

            set
            {
                _encryption = value;
            }
        }

        internal IniFile(string iniFilePath)
        {
            IniFilePath = iniFilePath;
            Encryption = new Encryption();
        }

        internal string ReadKey(string section, string key, string defaultValue, bool decrypt)
        {
            StringBuilder stringBuilder = new StringBuilder(4096);
            GetPrivateProfileString(section, key, defaultValue, stringBuilder, stringBuilder.Capacity, IniFilePath);
            string value = stringBuilder.ToString();
            if (decrypt)
                value = Encryption.DecryptString(value);
            return value;
        }

        internal void WriteKey(string section, string key, string value, bool encrypt)
        {
            if (encrypt)
                value = Encryption.EncryptString(value);
            WritePrivateProfileString(section, key, value, IniFilePath);
        }

        internal void DeleteKey(string key, string section)
        {
            WriteKey(section, key, null, false);
        }

        internal void DeleteSection(string section)
        {
            WriteKey(section, null, null, false);
        }

        public bool IsKeyExists(string key, string section)
        {
            return ReadKey(key, section, string.Empty, false).Length > 0;
        }
    }
}
