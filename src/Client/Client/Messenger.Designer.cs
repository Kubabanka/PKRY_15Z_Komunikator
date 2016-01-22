namespace Client
{
    partial class Messenger
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
            this.MessengerPage = new System.Windows.Forms.TabPage();
            this.SendMessageButton = new System.Windows.Forms.Button();
            this.StartConversationButton = new System.Windows.Forms.Button();
            this.MessageInputTextBox = new System.Windows.Forms.TextBox();
            this.messengerRichTextBox = new System.Windows.Forms.RichTextBox();
            this.usersOnlineLabel = new System.Windows.Forms.Label();
            this.onlineUsersListBox = new System.Windows.Forms.ListBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.loginPage = new System.Windows.Forms.TabPage();
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.registerRadioButton = new System.Windows.Forms.RadioButton();
            this.loginRadioButton = new System.Windows.Forms.RadioButton();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.loginLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.loginTextBox = new System.Windows.Forms.TextBox();
            this.MessengerPage.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.loginPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // MessengerPage
            // 
            this.MessengerPage.Controls.Add(this.SendMessageButton);
            this.MessengerPage.Controls.Add(this.StartConversationButton);
            this.MessengerPage.Controls.Add(this.MessageInputTextBox);
            this.MessengerPage.Controls.Add(this.messengerRichTextBox);
            this.MessengerPage.Controls.Add(this.usersOnlineLabel);
            this.MessengerPage.Controls.Add(this.onlineUsersListBox);
            this.MessengerPage.Location = new System.Drawing.Point(4, 22);
            this.MessengerPage.Name = "MessengerPage";
            this.MessengerPage.Padding = new System.Windows.Forms.Padding(3);
            this.MessengerPage.Size = new System.Drawing.Size(729, 315);
            this.MessengerPage.TabIndex = 1;
            this.MessengerPage.Text = "Messenger";
            this.MessengerPage.UseVisualStyleBackColor = true;
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Location = new System.Drawing.Point(610, 258);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(75, 23);
            this.SendMessageButton.TabIndex = 6;
            this.SendMessageButton.Text = "Send";
            this.SendMessageButton.UseVisualStyleBackColor = true;
            this.SendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // StartConversationButton
            // 
            this.StartConversationButton.Location = new System.Drawing.Point(586, 13);
            this.StartConversationButton.Name = "StartConversationButton";
            this.StartConversationButton.Size = new System.Drawing.Size(123, 23);
            this.StartConversationButton.TabIndex = 4;
            this.StartConversationButton.Text = "Start conversation";
            this.StartConversationButton.UseVisualStyleBackColor = true;
            this.StartConversationButton.Click += new System.EventHandler(this.StartConversationButton_Click);
            // 
            // MessageInputTextBox
            // 
            this.MessageInputTextBox.Enabled = false;
            this.MessageInputTextBox.Location = new System.Drawing.Point(171, 233);
            this.MessageInputTextBox.Multiline = true;
            this.MessageInputTextBox.Name = "MessageInputTextBox";
            this.MessageInputTextBox.Size = new System.Drawing.Size(397, 70);
            this.MessageInputTextBox.TabIndex = 3;
            this.MessageInputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MessageInputTextBox_KeyPress);
            this.MessageInputTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MessageInputTextBox_KeyUp);
            // 
            // messengerRichTextBox
            // 
            this.messengerRichTextBox.Location = new System.Drawing.Point(171, 15);
            this.messengerRichTextBox.Name = "messengerRichTextBox";
            this.messengerRichTextBox.ReadOnly = true;
            this.messengerRichTextBox.Size = new System.Drawing.Size(397, 205);
            this.messengerRichTextBox.TabIndex = 2;
            this.messengerRichTextBox.Text = "";
            // 
            // usersOnlineLabel
            // 
            this.usersOnlineLabel.AutoSize = true;
            this.usersOnlineLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.usersOnlineLabel.Location = new System.Drawing.Point(15, 15);
            this.usersOnlineLabel.Name = "usersOnlineLabel";
            this.usersOnlineLabel.Size = new System.Drawing.Size(97, 20);
            this.usersOnlineLabel.TabIndex = 1;
            this.usersOnlineLabel.Text = "Users online";
            // 
            // onlineUsersListBox
            // 
            this.onlineUsersListBox.FormattingEnabled = true;
            this.onlineUsersListBox.Location = new System.Drawing.Point(19, 51);
            this.onlineUsersListBox.Name = "onlineUsersListBox";
            this.onlineUsersListBox.Size = new System.Drawing.Size(83, 212);
            this.onlineUsersListBox.TabIndex = 0;
            this.onlineUsersListBox.SelectedIndexChanged += new System.EventHandler(this.onlineUsersListBox_SelectedIndexChanged);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.loginPage);
            this.tabControl.Controls.Add(this.MessengerPage);
            this.tabControl.Location = new System.Drawing.Point(2, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(737, 341);
            this.tabControl.TabIndex = 0;
            // 
            // loginPage
            // 
            this.loginPage.Controls.Add(this.welcomeLabel);
            this.loginPage.Controls.Add(this.loginButton);
            this.loginPage.Controls.Add(this.registerRadioButton);
            this.loginPage.Controls.Add(this.loginRadioButton);
            this.loginPage.Controls.Add(this.passwordLabel);
            this.loginPage.Controls.Add(this.loginLabel);
            this.loginPage.Controls.Add(this.passwordTextBox);
            this.loginPage.Controls.Add(this.loginTextBox);
            this.loginPage.Location = new System.Drawing.Point(4, 22);
            this.loginPage.Name = "loginPage";
            this.loginPage.Padding = new System.Windows.Forms.Padding(3);
            this.loginPage.Size = new System.Drawing.Size(729, 315);
            this.loginPage.TabIndex = 0;
            this.loginPage.Text = "Login";
            this.loginPage.UseVisualStyleBackColor = true;
            // 
            // welcomeLabel
            // 
            this.welcomeLabel.AutoSize = true;
            this.welcomeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.welcomeLabel.Location = new System.Drawing.Point(288, 49);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(151, 24);
            this.welcomeLabel.TabIndex = 7;
            this.welcomeLabel.Text = "Log In/Register";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(335, 179);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 6;
            this.loginButton.Text = "Log in";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // registerRadioButton
            // 
            this.registerRadioButton.AutoSize = true;
            this.registerRadioButton.Checked = true;
            this.registerRadioButton.Location = new System.Drawing.Point(375, 156);
            this.registerRadioButton.Name = "registerRadioButton";
            this.registerRadioButton.Size = new System.Drawing.Size(64, 17);
            this.registerRadioButton.TabIndex = 5;
            this.registerRadioButton.TabStop = true;
            this.registerRadioButton.Text = "Register";
            this.registerRadioButton.UseVisualStyleBackColor = true;
            // 
            // loginRadioButton
            // 
            this.loginRadioButton.AutoSize = true;
            this.loginRadioButton.Location = new System.Drawing.Point(312, 156);
            this.loginRadioButton.Name = "loginRadioButton";
            this.loginRadioButton.Size = new System.Drawing.Size(54, 17);
            this.loginRadioButton.TabIndex = 4;
            this.loginRadioButton.Text = "Log in";
            this.loginRadioButton.UseVisualStyleBackColor = true;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(261, 126);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(56, 13);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Password:";
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new System.Drawing.Point(263, 97);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(58, 13);
            this.loginLabel.TabIndex = 2;
            this.loginLabel.Text = "Username:";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(323, 123);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(100, 20);
            this.passwordTextBox.TabIndex = 1;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // loginTextBox
            // 
            this.loginTextBox.Location = new System.Drawing.Point(323, 90);
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.Size = new System.Drawing.Size(100, 20);
            this.loginTextBox.TabIndex = 0;
            // 
            // Messenger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 339);
            this.Controls.Add(this.tabControl);
            this.Name = "Messenger";
            this.Text = "Messenger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.MessengerPage.ResumeLayout(false);
            this.MessengerPage.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.loginPage.ResumeLayout(false);
            this.loginPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox loginTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.RadioButton loginRadioButton;
        private System.Windows.Forms.RadioButton registerRadioButton;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.TabPage loginPage;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ListBox onlineUsersListBox;
        private System.Windows.Forms.Label usersOnlineLabel;
        private System.Windows.Forms.RichTextBox messengerRichTextBox;
        private System.Windows.Forms.TextBox MessageInputTextBox;
        private System.Windows.Forms.Button StartConversationButton;
        private System.Windows.Forms.Button SendMessageButton;
        private System.Windows.Forms.TabPage MessengerPage;
    }
}

