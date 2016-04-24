namespace UT_WINFORMS
{
    partial class TwitchLogin
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
            this.TwitchBrowser = new System.Windows.Forms.WebBrowser();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TwitchBrowser
            // 
            this.TwitchBrowser.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TwitchBrowser.Location = new System.Drawing.Point(0, 29);
            this.TwitchBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.TwitchBrowser.Name = "TwitchBrowser";
            this.TwitchBrowser.ScriptErrorsSuppressed = true;
            this.TwitchBrowser.Size = new System.Drawing.Size(364, 606);
            this.TwitchBrowser.TabIndex = 0;
            this.TwitchBrowser.WebBrowserShortcutsEnabled = false;
            this.TwitchBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.TwitchBrowser_Navigated);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CloseButton.Location = new System.Drawing.Point(120, 1);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(125, 25);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close Window";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // TwitchLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(364, 635);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.TwitchBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TwitchLogin";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Twitch Login";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser TwitchBrowser;
        private System.Windows.Forms.Button CloseButton;
    }
}