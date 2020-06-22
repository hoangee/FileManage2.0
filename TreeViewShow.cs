
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FileManager
{
    public interface ITreeViewShow
    {
        void InitDisplay(ref TreeView deviceTreeView);

    }
    public enum IconsIndexes
    {
        Folder = 0,                  //文件夹图标
        File = 1,                   //文件图标
        FixedDrive = 2,             //固定磁盘
        RemovableDisk = 2,          //可移动磁盘
        RecentFiles = 5,              //最近访问
        CDRom = 6,                   //光驱         
    }
    public class TreeViewShow:ITreeViewShow
    {
        public void InitDisplay(ref TreeView deviceTreeView)
        {
            deviceTreeView.Nodes.Clear();

            //添加最近访问
            TreeNode recentFilesNode = deviceTreeView.Nodes.Add("最近访问");
            recentFilesNode.Tag = "最近访问";
            recentFilesNode.ImageIndex = (int)IconsIndexes.RecentFiles;
            recentFilesNode.SelectedImageIndex = (int)IconsIndexes.RecentFiles;

            //获取驱动信息
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            //遍历得到硬盘驱动等信息
            foreach (DriveInfo info in driveInfos)
            {
                TreeNode driveNode = null;

                switch (info.DriveType)
                {

                    //固定磁盘
                    case DriveType.Fixed:

                        //显示的名称
                        driveNode = deviceTreeView.Nodes.Add("本地磁盘(" + info.Name.Split('\\')[0] + ")");

                        //真正的路径
                        driveNode.Tag = info.Name;

                        driveNode.ImageIndex = (int)IconsIndexes.FixedDrive;
                        driveNode.SelectedImageIndex = (int)IconsIndexes.FixedDrive;

                        break;

                    //光驱
                    case DriveType.CDRom:

                        //显示的名称
                        driveNode = deviceTreeView.Nodes.Add("光驱(" + info.Name.Split('\\')[0] + ")");

                        //真正的路径
                        driveNode.Tag = info.Name;

                        driveNode.ImageIndex = (int)IconsIndexes.CDRom;
                        driveNode.SelectedImageIndex = (int)IconsIndexes.CDRom;

                        break;

                    //可移动磁盘
                    case DriveType.Removable:

                        //显示的名称
                        driveNode = deviceTreeView.Nodes.Add("可移动磁盘(" + info.Name.Split('\\')[0] + ")");

                        //真正的路径
                        driveNode.Tag = info.Name;

                        driveNode.ImageIndex = (int)IconsIndexes.RemovableDisk;
                        driveNode.SelectedImageIndex = (int)IconsIndexes.RemovableDisk;

                        break;
                }
            }

            //加载每个磁盘下的子目录
            foreach (TreeNode node in deviceTreeView.Nodes)
            {
                LoadChildNodes(node);
            }
        }

        public static void LoadChildNodes(TreeNode node)
        {
            try
            {
                //清除空节点，然后才加载子节点
                node.Nodes.Clear();

                if (node.Tag.ToString() == "最近访问")
                {
                    return;
                }
                else
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(node.Tag.ToString());
                    DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();

                    foreach (DirectoryInfo info in directoryInfos)
                    {
                        //显示的名称
                        TreeNode childNode = node.Nodes.Add(info.Name);

                        //真正的路径
                        childNode.Tag = info.FullName;

                        childNode.ImageIndex = (int)IconsIndexes.Folder;
                        childNode.SelectedImageIndex = (int)IconsIndexes.Folder;

                        //加载空节点，以实现“+”号
                        childNode.Nodes.Add("");
                    }
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
