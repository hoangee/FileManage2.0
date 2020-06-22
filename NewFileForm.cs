using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FileManager
{
    public partial class NewFileForm : Form
    {

        private string curFilePath;

        private FileManage mainForm;

        //以当前路径以及创建主窗体引用来构造新建文件
        public NewFileForm(string path, FileManage mainForm)
        {
            InitializeComponent();
            this.curFilePath = path;
            this.mainForm = mainForm;
        }

        //取消按钮
        private void newFileNb_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //确定按钮
        private void newFileYb_Click(object sender, EventArgs e)
        {
            string newFileName = newFiletbx.Text;
            
            if(string.IsNullOrEmpty(newFileName))
            {
                //弹出对话框
                MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;
                DialogResult dialog = MessageBox.Show("正确输入搜索文件名", "提示", messageBoxButtons);
                return;
            }
            //路径
            string newFilePath = Path.Combine(curFilePath, newFileName);

            //文件名不合法
            if (!IsValidFileName(newFileName))
            {
                MessageBox.Show("文件名不能包含下列任何字符:\r\n" + "\t\\/:*?\"<>|", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (File.Exists(newFilePath))
            {
                //存在同名文件多加一个计数
                int num = 1;
                string samePath = newFilePath;
                while (File.Exists(samePath))
                {
                    samePath = newFilePath + "(" + num + ")";
                    num++; 
                }
                File.Create(samePath);

                //更新文件列表
                mainForm.ShowFilesList(curFilePath, false);

                this.Close();

            }
            else
            {
                File.Create(newFilePath);

                //更新文件列表
                mainForm.ShowFilesList(curFilePath, false);

                this.Close();
            }
        }

        //判断
        private bool IsValidFileName(string fileName)
        {
            bool isValid = true;

            //非法字符
            string errChar = "\\/:*?\"<>|";

            for (int i = 0; i < errChar.Length; i++)
            {
                if (fileName.Contains(errChar[i].ToString()))
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }
    }
}
