using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using HeroWarzLauncher.Koggames;
using HeroWarzLauncher.Settings;
using HeroWarzLauncher.Extensions;

namespace HeroWarzLauncher
{
    public partial class MainForm : Form
    {
        private const string HeroWarzExecutable = "MCLauncher.exe";

        private KoggamesAPI _koggames = null;
        private KoggamesAPI Koggames
        {
            get
            {
                return _koggames;
            }

            set
            {
                _koggames = value;
            }
        }

        private SettingsWrapper Settings = new SettingsWrapper();

        public MainForm()
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));

            if (!File.Exists(HeroWarzExecutable))
            {
                Koggames_OnError(new Exception(string.Format("Game executable file (\"{0}\") could not be found in the current directory.\nPlease make sure the launcher is in the game directory.", HeroWarzExecutable)), KoggamesAPI.ExceptionType.None);
                Exit();
            }

            Koggames = new KoggamesAPI();
            Koggames.OnError += Koggames_OnError;
            Koggames.OnInitialization += Koggames_OnInitialization;
            Koggames.OnLogin += Koggames_OnLogin;
            Koggames.OnLoginHeroWarz += Koggames_OnLoginHeroWarz;
            Koggames.OnHeroWarzCmd += Koggames_OnHeroWarzCmd;

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();

            UsernameTextbox.SetPlaceholderText("Username...");
            PasswordTextbox.SetPlaceholderText("Password...");

            UsernameTextbox.Text = Settings.Username;
            SaveUsernameCheckbox.Checked = Settings.SaveUsername;

            PasswordTextbox.Text = Settings.Password;
            SavePasswordCheckbox.Checked = Settings.SavePassword;
        }

        private void UsernameTextbox_TextChanged(object sender, EventArgs e)
        {
            if (SaveUsernameCheckbox.Checked)
                Settings.Username = UsernameTextbox.Text;
        }

        private void SaveUsernameCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SaveUsername = SaveUsernameCheckbox.Checked;
            if (SaveUsernameCheckbox.Checked)
                Settings.Username = UsernameTextbox.Text;
            else
                Settings.Username = string.Empty;
        }

        private void PasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            if (SavePasswordCheckbox.Checked)
                Settings.Password = PasswordTextbox.Text;
        }

        private void SavePasswordCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SavePassword = SavePasswordCheckbox.Checked;
            if (SavePasswordCheckbox.Checked)
                Settings.Password = PasswordTextbox.Text;
            else
                Settings.Password = string.Empty;

        }

        private void LaunchButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTextbox.Text) || string.IsNullOrEmpty(PasswordTextbox.Text))
                Koggames_OnError(new Exception("Invalid username and/or password."), KoggamesAPI.ExceptionType.None);
            else
            {
                EnableControls(false);
                Koggames.Initialize();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Exit();
        }

        private void Koggames_OnError(Exception exception, KoggamesAPI.ExceptionType type)
        {
            if (type != KoggamesAPI.ExceptionType.None)
            {
                if (exception != null)
                    MessageBox.Show(this, string.Format("An error has occurred in function \"{0}\":\n{1}", type.GetEnumDescription(), exception.Message), "HeroWarzLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(this, string.Format("An error has occurred in function \"{0}\".", type.GetEnumDescription()), "HeroWarzLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (exception != null)
                MessageBox.Show(this, string.Format("{0}", exception.Message), "HeroWarzLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(this, "An unknown error has occurred.", "HeroWarzLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Koggames_OnInitialization(bool success)
        {
            if (success)
                Koggames.Login(UsernameTextbox.Text, PasswordTextbox.Text);
            else
                EnableControls(true);
        }

        private void Koggames_OnLogin(bool success)
        {
            if (success)
                Koggames.LoginHeroWarz();
            else
                EnableControls(true);
        }

        private void Koggames_OnLoginHeroWarz(bool success)
        {
            if (success)
                Koggames.GetHeroWarzCmd();
            else
                EnableControls(true);
        }

        private void Koggames_OnHeroWarzCmd(string commandLine)
        {
            if (!string.IsNullOrEmpty(commandLine))
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(HeroWarzExecutable, commandLine);
                processStartInfo.UseShellExecute = true;
                Process.Start(processStartInfo);
                Exit();
            }
            else
                EnableControls(true);
        }

        private void EnableControls(bool enabled)
        {
            EnableControls(this, enabled);
        }
        private void EnableControls(Control parent, bool enabled)
        {
            if (parent.InvokeRequired)
            {
                parent.Invoke((MethodInvoker)delegate
                {
                    EnableControls(parent, enabled);
                });
            }
            else
            {
                foreach (Control c in parent.Controls)
                {
                    c.Enabled = enabled;
                    EnableControls(c, enabled);
                }
            }
        }

        private void Exit()
        {
            Environment.Exit(0);
        }
    }
}
