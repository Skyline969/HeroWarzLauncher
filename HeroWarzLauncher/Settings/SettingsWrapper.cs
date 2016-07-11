using System.IO;

namespace HeroWarzLauncher.Settings
{
    internal class SettingsWrapper
    {
        private const string SettingsSectionName = "Settings";

        private IniFile _iniFile = null;
        private IniFile IniFile
        {
            get
            {
                return _iniFile;
            }

            set
            {
                _iniFile = value;
            }
        }

        internal string Username
        {
            get
            {
                return IniFile.ReadKey(SettingsSectionName, "Username", string.Empty, true);
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    IniFile.DeleteKey("Username", SettingsSectionName);
                else
                    IniFile.WriteKey(SettingsSectionName, "Username", value, true);
            }
        }

        internal string Password
        {
            get
            {
                return IniFile.ReadKey(SettingsSectionName, "Password", string.Empty, true);
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    IniFile.DeleteKey("Password", SettingsSectionName);
                else
                    IniFile.WriteKey(SettingsSectionName, "Password", value, true);
            }
        }

        internal bool SaveUsername
        {
            get
            {
                return IniFile.ReadKey(SettingsSectionName, "SaveUsername", "0", false) == "1";
            }

            set
            {
                if (!value)
                    IniFile.DeleteKey("SaveUsername", SettingsSectionName);
                else
                    IniFile.WriteKey(SettingsSectionName, "SaveUsername", value ? "1" : "0", false);
            }
        }

        internal bool SavePassword
        {
            get
            {
                return IniFile.ReadKey(SettingsSectionName, "SavePassword", "0", false) == "1";
            }

            set
            {
                if (!value)
                    IniFile.DeleteKey("SavePassword", SettingsSectionName);
                else
                    IniFile.WriteKey(SettingsSectionName, "SavePassword", value ? "1" : "0", false);
            }
        }

        internal bool AutoLogin
        {
            get
            {
                return IniFile.ReadKey(SettingsSectionName, "AutoLogin", "0", false) == "1";
            }

            set
            {
                if (!value)
                    IniFile.DeleteKey("AutoLogin", SettingsSectionName);
                else
                    IniFile.WriteKey(SettingsSectionName, "AutoLogin", value ? "1" : "0", false);
            }
        }

        internal SettingsWrapper()
        {
            IniFile = new IniFile(string.Format("{0}/HeroWarzLauncher.ini", Directory.GetCurrentDirectory()));
        }
    }
}
