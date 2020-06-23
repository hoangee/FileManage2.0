using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FileManager
{
    public partial class FileDetailInfoForm : Form
    {
        public FileDetailInfoForm(string filePath)
        {
            InitializeComponent();

            //初始化界面
            InitDisplay(filePath);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //初始化界面
        private void InitDisplay(string filePath)
        {
            //如果filePath是文件的路径
            if (File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);

                this.textFilename.Text = fileInfo.Name;
                this.textFileType.Text = fileInfo.Extension;
                this.textLocation.Text = (fileInfo.DirectoryName != null) ? fileInfo.DirectoryName : null;
                this.textFileSize.Text = ShowFileSize(fileInfo.Length);
                this.textCreateTime.Text = fileInfo.CreationTime.ToString();
                this.textChangeTime.Text = fileInfo.LastWriteTime.ToString();
                this.textVisitTime.Text = fileInfo.LastAccessTime.ToString();
            }
            //如果filePath是文件夹的路径
            else if (Directory.Exists(filePath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

                textFilename.Text = directoryInfo.Name;
                textFileType.Text = "文件夹";
                textLocation.Text = (directoryInfo.Parent != null) ? directoryInfo.Parent.FullName : null;
                textFileSize.Text = ShowFileSize(GetDirectoryLength(filePath));
                textCreateTime.Text = directoryInfo.CreationTime.ToString();
                textChangeTime.Text = directoryInfo.LastWriteTime.ToString();
                textVisitTime.Text = directoryInfo.LastAccessTime.ToString();
            }
        }

        //获取目录的大小
        private long GetDirectoryLength(string dirPath)
        {
            long length = 0;
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);


            //获取目录下所有文件的大小
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            if (fileInfos.Length > 0)
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    length += fileInfo.Length;
                }
            }


            //递归获取目录下所有文件夹的大小
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            if (directoryInfos.Length > 0)
            {
                foreach (DirectoryInfo dirInfo in directoryInfos)
                {
                    length += GetDirectoryLength(dirInfo.FullName);
                }
            }

            return length;
        }

        //以一定格式显示文件的大小
        //Math.Round(num,2,MidpointRounding.AwayFromZero)，中国式的四舍五入，num保留2位小数
        public static string ShowFileSize(long fileSize)
        {
            string fileSizeStr = "";

            if (fileSize < 1024)
            {
                fileSizeStr = fileSize + " 字节";
            }
            else if (fileSize >= 1024 && fileSize < 1024 * 1024)
            {
                fileSizeStr = Math.Round(fileSize * 1.0 / 1024, 2, MidpointRounding.AwayFromZero) + " KB(" + fileSize + "字节)";
            }
            else if (fileSize >= 1024 * 1024 && fileSize < 1024 * 1024 * 1024)
            {
                fileSizeStr = Math.Round(fileSize * 1.0 / (1024 * 1024), 2, MidpointRounding.AwayFromZero) + " MB(" + fileSize + "字节)";
            }
            else if (fileSize >= 1024 * 1024 * 1024)
            {
                fileSizeStr = Math.Round(fileSize * 1.0 / (1024 * 1024 * 1024), 2, MidpointRounding.AwayFromZero) + " GB(" + fileSize + "字节)";
            }

            return fileSizeStr;
        }
    }
}
