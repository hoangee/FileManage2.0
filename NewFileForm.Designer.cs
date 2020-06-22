namespace FileManager
{
    partial class NewFileForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewFileForm));
            this.newFileLabel = new System.Windows.Forms.Label();
            this.newFiletbx = new System.Windows.Forms.TextBox();
            this.newFileYb = new System.Windows.Forms.Button();
            this.newFileNb = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // newFileLabel
            // 
            this.newFileLabel.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.newFileLabel.Location = new System.Drawing.Point(34, 50);
            this.newFileLabel.Name = "newFileLabel";
            this.newFileLabel.Size = new System.Drawing.Size(109, 21);
            this.newFileLabel.TabIndex = 0;
            this.newFileLabel.Text = "文件名:";
            // 
            // newFiletbx
            // 
            this.newFiletbx.Location = new System.Drawing.Point(127, 50);
            this.newFiletbx.Name = "newFiletbx";
            this.newFiletbx.Size = new System.Drawing.Size(245, 25);
            this.newFiletbx.TabIndex = 1;
            // 
            // newFileYb
            // 
            this.newFileYb.Location = new System.Drawing.Point(56, 125);
            this.newFileYb.Name = "newFileYb";
            this.newFileYb.Size = new System.Drawing.Size(77, 31);
            this.newFileYb.TabIndex = 2;
            this.newFileYb.Text = "确定";
            this.newFileYb.UseVisualStyleBackColor = true;
            this.newFileYb.Click += new System.EventHandler(this.newFileYb_Click);
            // 
            // newFileNb
            // 
            this.newFileNb.Location = new System.Drawing.Point(331, 125);
            this.newFileNb.Name = "newFileNb";
            this.newFileNb.Size = new System.Drawing.Size(74, 31);
            this.newFileNb.TabIndex = 3;
            this.newFileNb.Text = "取消";
            this.newFileNb.UseVisualStyleBackColor = true;
            this.newFileNb.Click += new System.EventHandler(this.newFileNb_Click);
            // 
            // NewFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 192);
            this.Controls.Add(this.newFileNb);
            this.Controls.Add(this.newFileYb);
            this.Controls.Add(this.newFiletbx);
            this.Controls.Add(this.newFileLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewFileForm";
            this.Text = "新建文件";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label newFileLabel;
        private System.Windows.Forms.TextBox newFiletbx;
        private System.Windows.Forms.Button newFileYb;
        private System.Windows.Forms.Button newFileNb;
    }
}