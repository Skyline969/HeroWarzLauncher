namespace HeroWarzLauncher
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonLaunch = new System.Windows.Forms.Button();
            this.UsernameTextbox = new System.Windows.Forms.TextBox();
            this.PasswordTextbox = new System.Windows.Forms.TextBox();
            this.SaveUsernameCheckbox = new System.Windows.Forms.CheckBox();
            this.SavePasswordCheckbox = new System.Windows.Forms.CheckBox();
            this.AutoLoginCheckbox = new System.Windows.Forms.CheckBox();
            this.AutoLoginTimerLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonLaunch
            // 
            this.buttonLaunch.Location = new System.Drawing.Point(12, 87);
            this.buttonLaunch.Name = "buttonLaunch";
            this.buttonLaunch.Size = new System.Drawing.Size(262, 23);
            this.buttonLaunch.TabIndex = 0;
            this.buttonLaunch.Text = "Launch";
            this.buttonLaunch.UseVisualStyleBackColor = true;
            this.buttonLaunch.Click += new System.EventHandler(this.LaunchButton_Click);
            // 
            // UsernameTextbox
            // 
            this.UsernameTextbox.Location = new System.Drawing.Point(12, 12);
            this.UsernameTextbox.Name = "UsernameTextbox";
            this.UsernameTextbox.Size = new System.Drawing.Size(159, 20);
            this.UsernameTextbox.TabIndex = 1;
            this.UsernameTextbox.TextChanged += new System.EventHandler(this.UsernameTextbox_TextChanged);
            this.UsernameTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UsernameTextbox_KeyPress);
            // 
            // PasswordTextbox
            // 
            this.PasswordTextbox.Location = new System.Drawing.Point(12, 38);
            this.PasswordTextbox.Name = "PasswordTextbox";
            this.PasswordTextbox.PasswordChar = '*';
            this.PasswordTextbox.Size = new System.Drawing.Size(159, 20);
            this.PasswordTextbox.TabIndex = 2;
            this.PasswordTextbox.TextChanged += new System.EventHandler(this.PasswordTextbox_TextChanged);
            this.PasswordTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PasswordTextbox_KeyPress);
            // 
            // SaveUsernameCheckbox
            // 
            this.SaveUsernameCheckbox.AutoSize = true;
            this.SaveUsernameCheckbox.Location = new System.Drawing.Point(177, 15);
            this.SaveUsernameCheckbox.Name = "SaveUsernameCheckbox";
            this.SaveUsernameCheckbox.Size = new System.Drawing.Size(100, 17);
            this.SaveUsernameCheckbox.TabIndex = 3;
            this.SaveUsernameCheckbox.Text = "Save username";
            this.SaveUsernameCheckbox.UseVisualStyleBackColor = true;
            this.SaveUsernameCheckbox.CheckedChanged += new System.EventHandler(this.SaveUsernameCheckbox_CheckedChanged);
            this.SaveUsernameCheckbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SaveUsernameCheckbox_KeyPress);
            // 
            // SavePasswordCheckbox
            // 
            this.SavePasswordCheckbox.AutoSize = true;
            this.SavePasswordCheckbox.Location = new System.Drawing.Point(177, 40);
            this.SavePasswordCheckbox.Name = "SavePasswordCheckbox";
            this.SavePasswordCheckbox.Size = new System.Drawing.Size(99, 17);
            this.SavePasswordCheckbox.TabIndex = 4;
            this.SavePasswordCheckbox.Text = "Save password";
            this.SavePasswordCheckbox.UseVisualStyleBackColor = true;
            this.SavePasswordCheckbox.CheckedChanged += new System.EventHandler(this.SavePasswordCheckbox_CheckedChanged);
            this.SavePasswordCheckbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SavePasswordCheckbox_KeyPress);
            // 
            // AutoLoginCheckbox
            // 
            this.AutoLoginCheckbox.AutoSize = true;
            this.AutoLoginCheckbox.Location = new System.Drawing.Point(177, 64);
            this.AutoLoginCheckbox.Name = "AutoLoginCheckbox";
            this.AutoLoginCheckbox.Size = new System.Drawing.Size(77, 17);
            this.AutoLoginCheckbox.TabIndex = 5;
            this.AutoLoginCheckbox.Text = "Auto Login";
            this.AutoLoginCheckbox.UseVisualStyleBackColor = true;
            this.AutoLoginCheckbox.CheckedChanged += new System.EventHandler(this.AutoLoginCheckbox_CheckedChanged);
            this.AutoLoginCheckbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AutoLoginCheckbox_KeyPress);
            // 
            // AutoLoginTimerLabel
            // 
            this.AutoLoginTimerLabel.AutoSize = true;
            this.AutoLoginTimerLabel.Location = new System.Drawing.Point(12, 65);
            this.AutoLoginTimerLabel.Name = "AutoLoginTimerLabel";
            this.AutoLoginTimerLabel.Size = new System.Drawing.Size(73, 13);
            this.AutoLoginTimerLabel.TabIndex = 6;
            this.AutoLoginTimerLabel.Text = "No auto login.";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 119);
            this.Controls.Add(this.AutoLoginTimerLabel);
            this.Controls.Add(this.AutoLoginCheckbox);
            this.Controls.Add(this.SavePasswordCheckbox);
            this.Controls.Add(this.SaveUsernameCheckbox);
            this.Controls.Add(this.PasswordTextbox);
            this.Controls.Add(this.UsernameTextbox);
            this.Controls.Add(this.buttonLaunch);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HeroWarzLauncher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLaunch;
        private System.Windows.Forms.TextBox UsernameTextbox;
        private System.Windows.Forms.TextBox PasswordTextbox;
        private System.Windows.Forms.CheckBox SaveUsernameCheckbox;
        private System.Windows.Forms.CheckBox SavePasswordCheckbox;
        private System.Windows.Forms.CheckBox AutoLoginCheckbox;
        private System.Windows.Forms.Label AutoLoginTimerLabel;
    }
}

