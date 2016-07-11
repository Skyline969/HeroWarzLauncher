using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using HeroWarzLauncher.Koggames;
using HeroWarzLauncher.Settings;
using HeroWarzLauncher.Extensions;
using System.Threading.Tasks;

/**
 *  HeroWarzLauncher
 *  Written by Jeedify/Oriya
 *  Modified by Skyline969 on 2016-07-10
 */
namespace HeroWarzLauncher
{
    public partial class MainForm : Form
    {
        // The filename for the HeroWarz executable that is called.
        private const string HeroWarzExecutable = "MCLauncher.exe";
        // The auto login timer value in seconds.
        private const int AutoLoginTimerWait = 3;
        // Instantiating the auto login timer.
        private int AutoLoginTimerCount = AutoLoginTimerWait;
        Timer AutoLoginTimer = new Timer();

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
            // Get the current directory and check for the HWz executable.
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));

            if (!File.Exists(HeroWarzExecutable))
            {
                Koggames_OnError(new Exception(string.Format("Game executable file (\"{0}\") could not be found in the current directory.\nPlease make sure the launcher is in the game directory.", HeroWarzExecutable)), KoggamesAPI.ExceptionType.None);
                Exit();
            }

            // Instantiate the API event handlers.
            Koggames = new KoggamesAPI();
            Koggames.OnError += Koggames_OnError;
            Koggames.OnInitialization += Koggames_OnInitialization;
            Koggames.OnLogin += Koggames_OnLogin;
            Koggames.OnLoginHeroWarz += Koggames_OnLoginHeroWarz;
            Koggames.OnHeroWarzCmd += Koggames_OnHeroWarzCmd;

            // Set the icon and set up the form.
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();

            AutoLoginTimerLabel.Text = "";

            UsernameTextbox.SetPlaceholderText("Username...");
            PasswordTextbox.SetPlaceholderText("Password...");

            UsernameTextbox.Text = Settings.Username;
            SaveUsernameCheckbox.Checked = Settings.SaveUsername;

            PasswordTextbox.Text = Settings.Password;
            SavePasswordCheckbox.Checked = Settings.SavePassword;

            AutoLoginCheckbox.Checked = Settings.AutoLogin;

            // Kick off the auto login timer if auto login has been set from the config file.
            if (Settings.AutoLogin)
            {
                AutoLoginTimer.Tick += new EventHandler(BeginAutoLogin);
                AutoLoginTimer.Interval = 1000;
                AutoLoginTimer.Start();
            }
        }

        /*
         *  If the save password checkbox is checked, typing in the password field writes the new
         *  password value into the config file.
         */
        private void UsernameTextbox_TextChanged(object sender, EventArgs e)
        {
            if (SaveUsernameCheckbox.Checked)
                Settings.Username = UsernameTextbox.Text;
        }

        /*
         *  Writes the save username config value as well as the current username entered.
         */
        private void SaveUsernameCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SaveUsername = SaveUsernameCheckbox.Checked;
            if (SaveUsernameCheckbox.Checked)
                Settings.Username = UsernameTextbox.Text;
            else
                Settings.Username = string.Empty;
        }

        /*
         *  If the save password checkbox is checked, typing in the password field writes the new
         *  password value into the config file.
         */
        private void PasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            if (SavePasswordCheckbox.Checked)
                Settings.Password = PasswordTextbox.Text;
        }

        /*
         *  Writes the save password config value as well as the current password entered.
         */
        private void SavePasswordCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SavePassword = SavePasswordCheckbox.Checked;
            if (SavePasswordCheckbox.Checked)
                Settings.Password = PasswordTextbox.Text;
            else
                Settings.Password = string.Empty;
        }

        /*
         *  Writes the auto login config value and stops the login timer (if stopping an auto login).
         */
        private void AutoLoginCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.AutoLogin = AutoLoginCheckbox.Checked;
            if (AutoLoginCheckbox.Checked == false)
            {
                AutoLoginTimer.Stop();
                AutoLoginTimerCount = AutoLoginTimerWait;
                AutoLoginTimerLabel.Text = "";
            }
        }

        /*
         *  Launches the login initiation function when the login button is clicked.
         */
        private void LaunchButton_Click(object sender, EventArgs e)
        {
            BeginLogin();
        }

        /*
         *  Handler to gracefully exit the program on close.
         */
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Exit();
        }

        /*
         *  Error handler for login issues.
         */
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

        /*
         *  
         */
        private void Koggames_OnInitialization(bool success)
        {
            if (success)
                Koggames.Login(UsernameTextbox.Text, PasswordTextbox.Text);
            else
                EnableControls(true);
        }

        /*
         *  
         */
        private void Koggames_OnLogin(bool success)
        {
            if (success)
                Koggames.LoginHeroWarz();
            else
                EnableControls(true);
        }

        /*
         *  
         */
        private void Koggames_OnLoginHeroWarz(bool success)
        {
            if (success)
                Koggames.GetHeroWarzCmd();
            else
                EnableControls(true);
        }

        /*
         *  
         */
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

        /*
         *  Re-enables an individual control. Used by following EnableControls function.
         */
        private void EnableControls(bool enabled)
        {
            EnableControls(this, enabled);
        }

        /*
         *  Re-enables the controls on the form.
         */
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

        /*
         *   Performs a graceful exit when the form is closed.
         */
        private void Exit()
        {
            AutoLoginTimer.Stop();
            Environment.Exit(0);
        }

        #region Key Press Handlers
        private void UsernameTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalKeyPressListener(sender, e);
        }

        private void PasswordTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalKeyPressListener(sender, e);
        }

        private void SaveUsernameCheckbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalKeyPressListener(sender, e);
        }

        private void SavePasswordCheckbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalKeyPressListener(sender, e);
        }

        private void AutoLoginCheckbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalKeyPressListener(sender, e);
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalKeyPressListener(sender, e);
        }

        /*
         *  Just check to see if the enter key was pressed. If so, login.
         */
        private void GlobalKeyPressListener(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BeginLogin();
            }
        }
        #endregion

        #region Initial Login Events
        /*
         *  Stops the auto login timer, performs error checking, and logs in if that passes. 
         */
        private void BeginLogin()
        {
            AutoLoginTimer.Stop();
            if (string.IsNullOrEmpty(UsernameTextbox.Text) || string.IsNullOrEmpty(PasswordTextbox.Text))
                Koggames_OnError(new Exception("Invalid username and/or password."), KoggamesAPI.ExceptionType.None);
            else
            {
                EnableControls(false);
                Koggames.Initialize();
            }
        }

        /*
         *  Ticks down the timer and initiates the login if the timer has expired.
         */
        private void BeginAutoLogin(Object sender, EventArgs args)
        {
            if (AutoLoginTimerCount > 0)
            {
                AutoLoginTimerLabel.Text = "Login in " + AutoLoginTimerCount + "....";
                AutoLoginTimerCount--;
            }
            else
            {
                AutoLoginTimerLabel.Text = "Logging in!";
                BeginLogin();
            }
        }
        #endregion
    }
}
