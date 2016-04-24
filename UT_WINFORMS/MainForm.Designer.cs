namespace UT_WINFORMS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.GetOAuth_Button = new System.Windows.Forms.Button();
            this.TwitchAPI_Output = new System.Windows.Forms.TextBox();
            this.StartUnitTests_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GetOAuth_Button
            // 
            this.GetOAuth_Button.Location = new System.Drawing.Point(12, 12);
            this.GetOAuth_Button.Name = "GetOAuth_Button";
            this.GetOAuth_Button.Size = new System.Drawing.Size(195, 31);
            this.GetOAuth_Button.TabIndex = 0;
            this.GetOAuth_Button.Text = "GET Authentication Token";
            this.GetOAuth_Button.UseVisualStyleBackColor = true;
            this.GetOAuth_Button.Click += new System.EventHandler(this.GetOAuth_Button_Click);
            // 
            // TwitchAPI_Output
            // 
            this.TwitchAPI_Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TwitchAPI_Output.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TwitchAPI_Output.Location = new System.Drawing.Point(12, 49);
            this.TwitchAPI_Output.Multiline = true;
            this.TwitchAPI_Output.Name = "TwitchAPI_Output";
            this.TwitchAPI_Output.ReadOnly = true;
            this.TwitchAPI_Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TwitchAPI_Output.Size = new System.Drawing.Size(760, 300);
            this.TwitchAPI_Output.TabIndex = 2;
            this.TwitchAPI_Output.WordWrap = false;
            // 
            // StartUnitTests_Button
            // 
            this.StartUnitTests_Button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartUnitTests_Button.BackColor = System.Drawing.Color.MintCream;
            this.StartUnitTests_Button.Location = new System.Drawing.Point(213, 12);
            this.StartUnitTests_Button.Name = "StartUnitTests_Button";
            this.StartUnitTests_Button.Size = new System.Drawing.Size(559, 31);
            this.StartUnitTests_Button.TabIndex = 1;
            this.StartUnitTests_Button.Text = "START";
            this.StartUnitTests_Button.UseVisualStyleBackColor = false;
            this.StartUnitTests_Button.Click += new System.EventHandler(this.StartUnitTests_Button_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.StartUnitTests_Button);
            this.Controls.Add(this.TwitchAPI_Output);
            this.Controls.Add(this.GetOAuth_Button);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CGL TwitchAPIv3 Unit Tests";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetOAuth_Button;
        private System.Windows.Forms.TextBox TwitchAPI_Output;
        private System.Windows.Forms.Button StartUnitTests_Button;
    }
}

