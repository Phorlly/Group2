
namespace Cafe_Manage_System
{
    partial class Form1
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
            this.tb_Data = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_Show = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_Data
            // 
            this.tb_Data.Font = new System.Drawing.Font("Khmer OS", 15.25F);
            this.tb_Data.Location = new System.Drawing.Point(56, 111);
            this.tb_Data.Multiline = true;
            this.tb_Data.Name = "tb_Data";
            this.tb_Data.Size = new System.Drawing.Size(689, 152);
            this.tb_Data.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Khmer OS", 15.25F);
            this.label1.Location = new System.Drawing.Point(235, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 42);
            this.label1.TabIndex = 1;
            this.label1.Text = "Test Connection with MySql";
            // 
            // bt_Show
            // 
            this.bt_Show.Font = new System.Drawing.Font("Khmer OS", 20.25F);
            this.bt_Show.Location = new System.Drawing.Point(300, 307);
            this.bt_Show.Name = "bt_Show";
            this.bt_Show.Size = new System.Drawing.Size(176, 67);
            this.bt_Show.TabIndex = 2;
            this.bt_Show.Text = "Show ";
            this.bt_Show.UseVisualStyleBackColor = true;
            this.bt_Show.Click += new System.EventHandler(this.bt_Show_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bt_Show);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_Data);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_Data;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_Show;
    }
}

