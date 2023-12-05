namespace spotify_controller_project
{
    partial class TwitchApp
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            connectButton = new Button();
            live_chat_link = new TextBox();
            live_link_label = new Label();
            SuspendLayout();
            // 
            // connectButton start the code
            // 
            connectButton.Location = new Point(699, 283);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(100, 27);
            connectButton.TabIndex = 0;
            connectButton.Text = "Connect";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += button1_Click;
            // 
            // live_chat_link usr input
            // 
            live_chat_link.Location = new Point(213, 90);
            live_chat_link.Name = "live_chat_link";
            live_chat_link.Size = new Size(394, 23);
            live_chat_link.TabIndex = 1;
            // 
            // live_link_label description
            // 
            live_link_label.AutoSize = true;
            live_link_label.Location = new Point(308, 57);
            live_link_label.Name = "live_link_label";
            live_link_label.Size = new Size(205, 15);
            live_link_label.TabIndex = 2;
            live_link_label.Text = "Please enter your live stream chat link";
            live_link_label.Click += label1_Click;
            // 
            // TwitchApp Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(811, 322);
            Controls.Add(live_link_label);
            Controls.Add(live_chat_link);
            Controls.Add(connectButton);
            Name = "TwitchApp";
            Text = "TwitchApp";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button connectButton;
        private TextBox live_chat_link;
        private Label live_link_label;
    }
}
