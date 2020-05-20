namespace FileManager
{
    partial class FileDetailInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileDetailInfoForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.textFilename = new System.Windows.Forms.TextBox();
            this.lbFileType = new System.Windows.Forms.Label();
            this.lbFileName = new System.Windows.Forms.Label();
            this.lbLocation = new System.Windows.Forms.Label();
            this.lbFileSize = new System.Windows.Forms.Label();
            this.textFileType = new System.Windows.Forms.TextBox();
            this.textFileSize = new System.Windows.Forms.TextBox();
            this.textLocation = new System.Windows.Forms.TextBox();
            this.textChangeTime = new System.Windows.Forms.TextBox();
            this.textCreateTime = new System.Windows.Forms.TextBox();
            this.lbCreate = new System.Windows.Forms.Label();
            this.lbChange = new System.Windows.Forms.Label();
            this.textVisitTime = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textFileType);
            this.panel1.Controls.Add(this.textFilename);
            this.panel1.Controls.Add(this.lbFileType);
            this.panel1.Controls.Add(this.lbFileName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(442, 146);
            this.panel1.TabIndex = 0;
            // 
            // textFilename
            // 
            this.textFilename.Location = new System.Drawing.Point(182, 35);
            this.textFilename.Name = "textFilename";
            this.textFilename.Size = new System.Drawing.Size(200, 25);
            this.textFilename.TabIndex = 3;
            // 
            // lbFileType
            // 
            this.lbFileType.Location = new System.Drawing.Point(51, 101);
            this.lbFileType.Name = "lbFileType";
            this.lbFileType.Size = new System.Drawing.Size(80, 20);
            this.lbFileType.TabIndex = 2;
            this.lbFileType.Text = "文件类型:";
            // 
            // lbFileName
            // 
            this.lbFileName.Location = new System.Drawing.Point(51, 35);
            this.lbFileName.Name = "lbFileName";
            this.lbFileName.Size = new System.Drawing.Size(80, 20);
            this.lbFileName.TabIndex = 1;
            this.lbFileName.Text = "文件名称:";
            // 
            // lbLocation
            // 
            this.lbLocation.Location = new System.Drawing.Point(51, 250);
            this.lbLocation.Name = "lbLocation";
            this.lbLocation.Size = new System.Drawing.Size(80, 20);
            this.lbLocation.TabIndex = 3;
            this.lbLocation.Text = "存储位置:";
            // 
            // lbFileSize
            // 
            this.lbFileSize.Location = new System.Drawing.Point(51, 182);
            this.lbFileSize.Name = "lbFileSize";
            this.lbFileSize.Size = new System.Drawing.Size(80, 20);
            this.lbFileSize.TabIndex = 4;
            this.lbFileSize.Text = "文件大小:";
            // 
            // textFileType
            // 
            this.textFileType.Enabled = false;
            this.textFileType.Location = new System.Drawing.Point(182, 101);
            this.textFileType.Name = "textFileType";
            this.textFileType.Size = new System.Drawing.Size(200, 25);
            this.textFileType.TabIndex = 4;
            // 
            // textFileSize
            // 
            this.textFileSize.Enabled = false;
            this.textFileSize.Location = new System.Drawing.Point(182, 177);
            this.textFileSize.Name = "textFileSize";
            this.textFileSize.Size = new System.Drawing.Size(200, 25);
            this.textFileSize.TabIndex = 5;
            // 
            // textLocation
            // 
            this.textLocation.Enabled = false;
            this.textLocation.Location = new System.Drawing.Point(182, 245);
            this.textLocation.Name = "textLocation";
            this.textLocation.Size = new System.Drawing.Size(200, 25);
            this.textLocation.TabIndex = 6;
            // 
            // textChangeTime
            // 
            this.textChangeTime.Enabled = false;
            this.textChangeTime.Location = new System.Drawing.Point(182, 387);
            this.textChangeTime.Name = "textChangeTime";
            this.textChangeTime.Size = new System.Drawing.Size(200, 25);
            this.textChangeTime.TabIndex = 10;
            // 
            // textCreateTime
            // 
            this.textCreateTime.Enabled = false;
            this.textCreateTime.Location = new System.Drawing.Point(182, 319);
            this.textCreateTime.Name = "textCreateTime";
            this.textCreateTime.Size = new System.Drawing.Size(200, 25);
            this.textCreateTime.TabIndex = 9;
            // 
            // lbCreate
            // 
            this.lbCreate.Location = new System.Drawing.Point(51, 324);
            this.lbCreate.Name = "lbCreate";
            this.lbCreate.Size = new System.Drawing.Size(80, 20);
            this.lbCreate.TabIndex = 8;
            this.lbCreate.Text = "创建时间:";
            // 
            // lbChange
            // 
            this.lbChange.Location = new System.Drawing.Point(51, 392);
            this.lbChange.Name = "lbChange";
            this.lbChange.Size = new System.Drawing.Size(80, 20);
            this.lbChange.TabIndex = 7;
            this.lbChange.Text = "修改时间:";
            // 
            // textVisitTime
            // 
            this.textVisitTime.Enabled = false;
            this.textVisitTime.Location = new System.Drawing.Point(182, 460);
            this.textVisitTime.Name = "textVisitTime";
            this.textVisitTime.Size = new System.Drawing.Size(200, 25);
            this.textVisitTime.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(51, 460);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "访问时间:";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(164, 523);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(88, 30);
            this.btnConfirm.TabIndex = 13;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // FileDetailInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 580);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.textVisitTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textChangeTime);
            this.Controls.Add(this.textCreateTime);
            this.Controls.Add(this.lbCreate);
            this.Controls.Add(this.lbChange);
            this.Controls.Add(this.textLocation);
            this.Controls.Add(this.textFileSize);
            this.Controls.Add(this.lbFileSize);
            this.Controls.Add(this.lbLocation);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileDetailInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "详细信息";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbFileName;
        private System.Windows.Forms.Label lbFileType;
        private System.Windows.Forms.TextBox textFilename;
        private System.Windows.Forms.Label lbLocation;
        private System.Windows.Forms.Label lbFileSize;
        private System.Windows.Forms.TextBox textFileType;
        private System.Windows.Forms.TextBox textFileSize;
        private System.Windows.Forms.TextBox textLocation;
        private System.Windows.Forms.TextBox textChangeTime;
        private System.Windows.Forms.TextBox textCreateTime;
        private System.Windows.Forms.Label lbCreate;
        private System.Windows.Forms.Label lbChange;
        private System.Windows.Forms.TextBox textVisitTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConfirm;
    }
}