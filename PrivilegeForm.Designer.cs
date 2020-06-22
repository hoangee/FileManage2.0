namespace FileManager
{
    partial class PrivilegeForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrivilegeForm));
            this.labelObj = new System.Windows.Forms.Label();
            this.labelObjName = new System.Windows.Forms.Label();
            this.labelGroup = new System.Windows.Forms.Label();
            this.lvGroup = new System.Windows.Forms.ListView();
            this.privilegeIistIco = new System.Windows.Forms.ImageList(this.components);
            this.labelPrivilege = new System.Windows.Forms.Label();
            this.lvPrivilge = new System.Windows.Forms.ListView();
            this.buttonPrivilegeY = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelObj
            // 
            this.labelObj.AutoSize = true;
            this.labelObj.Location = new System.Drawing.Point(13, 13);
            this.labelObj.Name = "labelObj";
            this.labelObj.Size = new System.Drawing.Size(67, 15);
            this.labelObj.TabIndex = 0;
            this.labelObj.Text = "对象名称";
            // 
            // labelObjName
            // 
            this.labelObjName.AutoSize = true;
            this.labelObjName.Location = new System.Drawing.Point(86, 13);
            this.labelObjName.Name = "labelObjName";
            this.labelObjName.Size = new System.Drawing.Size(55, 15);
            this.labelObjName.TabIndex = 1;
            this.labelObjName.Text = "label2";
            // 
            // labelGroup
            // 
            this.labelGroup.AutoSize = true;
            this.labelGroup.Location = new System.Drawing.Point(13, 54);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(97, 15);
            this.labelGroup.TabIndex = 2;
            this.labelGroup.Text = "组或用户名：";
            // 
            // lvGroup
            // 
            this.lvGroup.HideSelection = false;
            this.lvGroup.LargeImageList = this.privilegeIistIco;
            this.lvGroup.Location = new System.Drawing.Point(16, 83);
            this.lvGroup.Name = "lvGroup";
            this.lvGroup.Size = new System.Drawing.Size(267, 250);
            this.lvGroup.SmallImageList = this.privilegeIistIco;
            this.lvGroup.TabIndex = 3;
            this.lvGroup.UseCompatibleStateImageBehavior = false;
            this.lvGroup.View = System.Windows.Forms.View.List;
            this.lvGroup.SelectedIndexChanged += new System.EventHandler(this.lvGroup_SelectedIndexChanged);
            // 
            // privilegeIistIco
            // 
            this.privilegeIistIco.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("privilegeIistIco.ImageStream")));
            this.privilegeIistIco.TransparentColor = System.Drawing.Color.Transparent;
            this.privilegeIistIco.Images.SetKeyName(0, "group_or_user.png");
            this.privilegeIistIco.Images.SetKeyName(1, "privilege.png");
            // 
            // labelPrivilege
            // 
            this.labelPrivilege.AutoSize = true;
            this.labelPrivilege.Location = new System.Drawing.Point(304, 109);
            this.labelPrivilege.Name = "labelPrivilege";
            this.labelPrivilege.Size = new System.Drawing.Size(55, 15);
            this.labelPrivilege.TabIndex = 4;
            this.labelPrivilege.Text = "label1";
            // 
            // lvPrivilge
            // 
            this.lvPrivilge.CheckBoxes = true;
            this.lvPrivilge.HideSelection = false;
            this.lvPrivilge.LargeImageList = this.privilegeIistIco;
            this.lvPrivilge.Location = new System.Drawing.Point(307, 146);
            this.lvPrivilge.Name = "lvPrivilge";
            this.lvPrivilge.Size = new System.Drawing.Size(220, 187);
            this.lvPrivilge.SmallImageList = this.privilegeIistIco;
            this.lvPrivilge.TabIndex = 5;
            this.lvPrivilge.UseCompatibleStateImageBehavior = false;
            this.lvPrivilge.View = System.Windows.Forms.View.List;
            this.lvPrivilge.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvPrivilge_ItemChecked);
            // 
            // buttonPrivilegeY
            // 
            this.buttonPrivilegeY.Location = new System.Drawing.Point(429, 339);
            this.buttonPrivilegeY.Name = "buttonPrivilegeY";
            this.buttonPrivilegeY.Size = new System.Drawing.Size(79, 28);
            this.buttonPrivilegeY.TabIndex = 6;
            this.buttonPrivilegeY.Text = "确定";
            this.buttonPrivilegeY.UseVisualStyleBackColor = true;
            this.buttonPrivilegeY.Click += new System.EventHandler(this.buttonPrivilegeY_Click);
            // 
            // PrivilegeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 418);
            this.Controls.Add(this.buttonPrivilegeY);
            this.Controls.Add(this.lvPrivilge);
            this.Controls.Add(this.labelPrivilege);
            this.Controls.Add(this.lvGroup);
            this.Controls.Add(this.labelGroup);
            this.Controls.Add(this.labelObjName);
            this.Controls.Add(this.labelObj);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrivilegeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PrivilegeForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelObj;
        private System.Windows.Forms.Label labelObjName;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.ListView lvGroup;
        private System.Windows.Forms.Label labelPrivilege;
        private System.Windows.Forms.ListView lvPrivilge;
        private System.Windows.Forms.Button buttonPrivilegeY;
        private System.Windows.Forms.ImageList privilegeIistIco;
    }
}