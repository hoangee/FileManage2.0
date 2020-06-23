using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class PrivilegeForm : Form
    {
        //访问规则集合
        private AuthorizationRule[] accessRulesArray = null;
        //当前文件的全路径名
        private string fileName;
        //操作权限
        private List<string> privilegeList = new List<string> { "完全控制", "修改", "读取和执行", "列出文件夹内容", "读取", "写入" };
        private List<string> privilegeFlagsList = new List<string> { "FullControl", "Modify", "ReadAndExecute", "ListDirectory", "Read", "Write" };

        private List<FileSystemRights> fileSystemRightsList = new List<FileSystemRights> { FileSystemRights.FullControl, FileSystemRights.Modify,
        FileSystemRights.ReadAndExecute, FileSystemRights.ListDirectory, FileSystemRights.Read, FileSystemRights.Write };

        //（程序本身，不是用户手动）是否正在更新右边权限列表（的CheckBoxes）
        private bool isUpdateCheckBoxes = false;

        //当前选中的组或用户名的索引
        private int curSelected = 0;

        public PrivilegeForm(string fileName)
        {
            InitializeComponent();
            this.fileName = fileName;

            //初始化界面
            InitDisplay(fileName);
        }

        //初始化界面
        private void InitDisplay(string fileName)
        {
            //显示正在修改权限的文件名/文件夹
            labelObjName.Text = fileName;

            AuthorizationRuleCollection tempAccessRulesCollection = null;

            //如果是文件
            if (File.Exists(fileName))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                //获取文件的ACL访问控制列表
                tempAccessRulesCollection = fileInfo.GetAccessControl().GetAccessRules(true, true,
                                    typeof(System.Security.Principal.SecurityIdentifier));
            }
            //如果是文件夹
            else if (Directory.Exists(fileName))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(fileName);
                //获取文件夹的ACL访问控制列表
                tempAccessRulesCollection = dirInfo.GetAccessControl().GetAccessRules(true, true,
                                    typeof(System.Security.Principal.SecurityIdentifier));
            }


            AuthorizationRule[] tempAccessRulesArray = new AuthorizationRule[tempAccessRulesCollection.Count];
            tempAccessRulesCollection.CopyTo(tempAccessRulesArray, 0);

            //去重
            accessRulesArray = UniqAccessRules(tempAccessRulesArray);


            lvGroup.Items.Clear();

            //显示组或用户名列表
            for (int i = 0; i < accessRulesArray.Length; i++)
            {
                ListViewItem item = lvGroup.Items.Add(accessRulesArray[i].IdentityReference.Translate(typeof(NTAccount)).ToString());
                item.Tag = i;
                item.ImageIndex = PrivilegeIconsIndexes.GroupOrUser;
            }

            //初始时默认当前选中的组或用户名为第一项
            lvGroup.HideSelection = false;
            lvGroup.Items[0].Selected = true;

            //显示当前选中的组或用户名对该文件/文件夹具有的权限列表
            ShowPrivilegeList();
        }

        //去除重复项
        private AuthorizationRule[] UniqAccessRules(AuthorizationRule[] accessRules)
        {
            string preAccount = "";
            string nextAccout;
            List<AuthorizationRule> authorizationRulesList = new List<AuthorizationRule>();

            for (int i = 0; i < accessRules.Length; i++)
            {
                nextAccout = accessRules[i].IdentityReference.Translate(typeof(NTAccount)).ToString();

                //没有发生重复
                if (nextAccout != preAccount)
                {
                    authorizationRulesList.Add(accessRules[i]);
                }

                preAccount = nextAccout;
            }

            return authorizationRulesList.ToArray();
        }

        //显示当前选中的组或用户名对该文件/文件夹具有的权限列表
        private void ShowPrivilegeList()
        {
            isUpdateCheckBoxes = true;
            //权限ListView上方显示对那个用户、组的权限
            labelPrivilege.Text = lvGroup.Items[curSelected].Text + " 的权限：";

            lvPrivilge.Items.Clear();

            //当前选中的组或用户名对该文件/文件夹具有的权限
            List<string> privileges = new List<string>((((FileSystemAccessRule)accessRulesArray[curSelected]).FileSystemRights + "").Split(','));

            //显示权限列表
            for (int i = 0; i < privilegeList.Count; i++)
            {
                ListViewItem item = lvPrivilge.Items.Add(privilegeList[i], PrivilegeIconsIndexes.Privilege);
                item.Tag = i;
            }

            //初始设置选中状态
            for (int i = 0; i < privilegeList.Count; i++)
            {
                if (privileges.Contains(privilegeFlagsList[i]))
                {
                    SetPrivilegeListChecked(privilegeFlagsList[i]);
                }
            }

            isUpdateCheckBoxes = false;
        }

        //根据传入的权限设置权限列表的选中状态
        private void SetPrivilegeListChecked(string privilege)
        {
            switch (privilege)
            {
                case "FullControl":

                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        item.Checked = true;
                    }

                    //如果是文件，则"列出文件夹内容"不选中
                    if (File.Exists(fileName))
                    {
                        lvPrivilge.Items[3].Checked = false;
                    }

                    break;

                case "Modify":
                    //修改选中三个
                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        if (item.Text == "修改" || item.Text == "读取和执行" || item.Text == "读取" || item.Text == "写入")
                        {
                            item.Checked = true;
                        }
                    }

                    //如果是文件夹, 则再选中"列出文件夹内容"
                    if (Directory.Exists(fileName))
                    {
                        lvPrivilge.Items[3].Checked = true;
                    }
                    break;

                case "ReadAndExecute":

                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        if (item.Text == "读取和执行" || item.Text == "读取")
                        {
                            item.Checked = true;
                        }
                    }

                    //如果是文件夹, 则再选中"列出文件夹内容"
                    if (Directory.Exists(fileName))
                    {
                        lvPrivilge.Items[3].Checked = true;
                    }
                    break;

                case "ListDirectory":

                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        if (item.Text == "列出文件夹内容")
                        {
                            item.Checked = true;
                        }
                    }
                    break;

                case "Read":

                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        if (item.Text == "读取")
                        {
                            item.Checked = true;
                        }
                    }

                    //如果是文件夹, 则再选中"列出文件夹内容"
                    if (Directory.Exists(fileName))
                    {
                        lvPrivilge.Items[3].Checked = true;
                    }

                    break;

                case "Write":

                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        if (item.Text == "写入")
                        {
                            item.Checked = true;
                        }
                    }
                    break;
            }
        }
        //点击确定按钮
        private void buttonPrivilegeY_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lvGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvGroup.SelectedItems.Count > 0)
            {
                curSelected = (int)lvGroup.SelectedItems[0].Tag;
                ShowPrivilegeList();
            }
        }

        private void lvPrivilge_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //当前的选中/取消选中操作不是更新操作造成的，是用户手动点击造成的
            if (!isUpdateCheckBoxes)
            {
                if (e.Item.Checked)
                {
                    //如果是文件，则即使选中"列出文件夹内容"，之后也不保存
                    if (File.Exists(fileName) && e.Item.Text == "列出文件夹内容")
                    {
                        MessageBox.Show("文件类型不支持此操作！此操作之后将不予以保存！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        isUpdateCheckBoxes = true;

                        //设置权限列表中当前选中项和其“包含”的选项的选中状态
                        SetPrivilegeListChecked(privilegeFlagsList[(int)e.Item.Tag]);

                        isUpdateCheckBoxes = false;

                        //修改权限
                        if (File.Exists(fileName))
                        {
                            
                            AddFileSecurity(fileName, accessRulesArray[curSelected].IdentityReference.Translate(typeof(NTAccount)).ToString(),
                                fileSystemRightsList[(int)e.Item.Tag], AccessControlType.Allow);
                        }
                        else if (Directory.Exists(fileName))
                        {
                            AddDirectorySecurity(fileName, accessRulesArray[curSelected].IdentityReference.Translate(typeof(NTAccount)).ToString(),
                                fileSystemRightsList[(int)e.Item.Tag], AccessControlType.Allow);
                        }
                    }
                }
                else
                {
                    //取消权限
                    isUpdateCheckBoxes = true;

                    //设置权限列表中当前选中项和其“包含”的选项的未选中状态
                    SetPrivilegeListUnChecked(privilegeFlagsList[(int)e.Item.Tag]);

                    isUpdateCheckBoxes = false;

                    if (File.Exists(fileName))
                    {
                        RemoveFileSecurity(fileName, accessRulesArray[curSelected].IdentityReference.Translate(typeof(NTAccount)).ToString(),
                            fileSystemRightsList[(int)e.Item.Tag], AccessControlType.Allow);
                    }
                    else if (Directory.Exists(fileName))
                    {
                        RemoveDirectorySecurity(fileName, accessRulesArray[curSelected].IdentityReference.Translate(typeof(NTAccount)).ToString(),
                            fileSystemRightsList[(int)e.Item.Tag], AccessControlType.Allow);
                    }
                }
            }
        }

        //根据传入的权限设置权限列表的未选中状态
        private void SetPrivilegeListUnChecked(string privilege)
        {
            switch (privilege)
            {
                case "FullControl":

                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        item.Checked = false;
                    }
                    break;

                case "Modify":

                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        if (item.Text == "修改" || item.Text == "读取和执行" || item.Text == "读取" || item.Text == "写入")
                        {
                            item.Checked = false;
                        }
                    }
                    break;

                case "ReadAndExecute":

                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        if (item.Text == "读取和执行" || item.Text == "读取")
                        {
                            item.Checked = false;
                        }
                    }
                    break;

                case "ListDirectory":

                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        if (item.Text == "列出文件夹内容")
                        {
                            item.Checked = false;
                        }
                    }
                    break;

                case "Read":

                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        if (item.Text == "读取")
                        {
                            item.Checked = false;
                        }
                    }
                    break;

                case "Write":

                    foreach (ListViewItem item in lvPrivilge.Items)
                    {
                        if (item.Text == "写入")
                        {
                            item.Checked = false;
                        }
                    }
                    break;
            }
        }

        //为指定的账户给指定的文件添加ACL项
        private void AddFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            FileSecurity fileSecurity = fileInfo.GetAccessControl();
            fileSecurity.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));
            fileInfo.SetAccessControl(fileSecurity);
        }

        //为指定的账户给指定的文件移除ACL项
        private void RemoveFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            FileSecurity fileSecurity = fileInfo.GetAccessControl();
            fileSecurity.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));
            fileInfo.SetAccessControl(fileSecurity);
        }


        //为指定的账户给指定的目录添加ACL项
        private void AddDirectorySecurity(string dirName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirName);
            DirectorySecurity dirSecurity = dirInfo.GetAccessControl(AccessControlSections.All);
            dirSecurity.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));
            dirInfo.SetAccessControl(dirSecurity);
        }


        //为指定的账户给指定的目录移除ACL项
        private void RemoveDirectorySecurity(string dirName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirName);
            DirectorySecurity dirSecurity = dirInfo.GetAccessControl();
            dirSecurity.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));
            dirInfo.SetAccessControl(dirSecurity);
        }
    }

    //图标索引
    class PrivilegeIconsIndexes
    {
        public const int GroupOrUser = 0; //组或用户
        public const int Privilege = 1; //权限
    }
}
