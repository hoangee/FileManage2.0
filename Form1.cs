﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace FileManager
{
    public partial class FileManage : Form
    {
        //当前路径
        private string curFilePath = "";

        //是否第一次初始化目录树,用于最开始显示那个盘的文件
        private bool isInitializeDeviceTreeView = true;
        

        //用户访问的第一个节点，(用链表保存用户历史访问，从而实现前进后退按钮操作)
        private HistoryListNode firstVisistPathNode = new HistoryListNode();
        //当前路径节点
        private HistoryListNode curPathNode = null;

        //当前选中的树节点（目录节点）
        private TreeNode curSelectedNode = null;

        //主窗口显示
        public FileManage()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        //界面导入显示
        private void FileManage_Load(object sender, EventArgs e)
        {
            //界面显示
            TreeViewShow treeViewShow = new TreeViewShow();
            treeViewShow.InitDisplay(ref this.deviceTreeView);
            //设置本地磁盘C为默认点击状态
            deviceTreeView.SelectedNode = deviceTreeView.Nodes[1];
        }

        private void FileManage_SizeChanged(object sender, EventArgs e)
        {
            tscbAddress.Width = this.Width - 350;
            toolStripOperator.Width = this.Width - 10;
        }

        private void FileManage_Resize(object sender, EventArgs e)
        {
            tscbAddress.Width = this.Width - 350;
            toolStripOperator.Width = this.Width - 10;
        }

        private void toolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //设置工具栏是否可见
            toolStripOperator.Visible = !toolStripOperator.Visible;
            //改变选中状态
            toolToolStripMenuItem.Checked = !toolToolStripMenuItem.Checked;
        }

        private void stateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //下方状态栏是否可见。
            belowStatusStrip.Visible = !belowStatusStrip.Visible;
            stateToolStripMenuItem.Checked = !stateToolStripMenuItem.Checked;
        }

        //树节点展开
        private void deviceTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.Expand();
        }
        //树节点打开前导入子节点
        private void deviceTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeViewShow treeViewShow = new TreeViewShow();
            treeViewShow.LoadChildNodes(e.Node);
        }

        //选定树节点
        private void deviceTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //第一次初始化,首先显示C盘的文件夹在ListView中
            if (this.isInitializeDeviceTreeView)
            {
                curFilePath = @"C:\";
                tscbAddress.Text = curFilePath;

                //保存用户的历史路径的第一个路径
                firstVisistPathNode.Path = curFilePath;
                curPathNode = firstVisistPathNode;

                curSelectedNode = e.Node;
                
                isInitializeDeviceTreeView = false;
                ShowFilesList(curFilePath, true);
            }
            else
            {
                //显示硬盘的目录在ListView
                curSelectedNode = e.Node;
                ShowFilesList(e.Node.Tag.ToString(), true);
            }
        }

        //后退按钮事件处理
        private void tsbBack_Click(object sender, EventArgs e)
        {
            //当前路径节点为顶层时
            if (curPathNode == firstVisistPathNode)
            {
                return;
            }
            else
            {
                string prePath = curPathNode.PreNode.Path;

                ShowFilesList(prePath, false);
                curPathNode = curPathNode.PreNode;
            }

        }
        //前进按钮事件处理
        private void tsbPrev_Click(object sender, EventArgs e)
        {
            //未进行任何操作时
            if(curPathNode.NextNode == null)
            {
                return;
            }
            else
            {
                //当前访问路径的下一个节点不为空
                string nextPath = curPathNode.NextNode.Path;
                ShowFilesList(nextPath, false);
                curPathNode = curPathNode.NextNode;
            }
        }
        //刷新按钮
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            ShowFilesList(curFilePath, false);
        }

        //返回上一级事件处理
        private void tsbUp_Click(object sender, EventArgs e)
        {
            //当前路径为空
            if (curFilePath == "")
            {
                return;
            }

            //得到当前路径
            DirectoryInfo directoryInfo = new DirectoryInfo(curFilePath);
            if(directoryInfo.Parent != null)
            {
                //未达到根目录,显示
                ShowFilesList(directoryInfo.Parent.FullName, true);
            }
            else
            {
                //到达根目录直接函数结束
                return;
            }
        }




        public void ShowFilesList(string path, bool isRecord)
        {
            
            if (isRecord)
            {
                //保存用户的历史访问路径
                HistoryListNode newNode = new HistoryListNode();
                newNode.Path = path;
                curPathNode.NextNode = newNode;
                newNode.PreNode = curPathNode;

                curPathNode = newNode;
            }

            //开始数据更新
            fileListView.BeginUpdate();

            //清空ListView
            fileListView.Items.Clear();

            //如果当前为历史访问时
            if(path == @"最近访问")
            {
                
                //得到最近访问文件/夹的枚举集合
                var recentFile = RecentFilesUtil.GetRecentFiles();

                //遍历，将文件显示到ListView
                foreach(var file in recentFile)
                {
                    if (File.Exists(file))
                    {
                        //当为文件时
                        FileInfo fileInfo = new FileInfo(file);
                        //显示文件相关信息
                        ListViewItem item = fileListView.Items.Add(fileInfo.Name);
                        item.Tag = fileInfo.FullName;
                        item.SubItems.Add(fileInfo.LastWriteTime.ToString());
                        item.SubItems.Add(fileInfo.Extension + "文件");
                        item.SubItems.Add(FileDetailInfoForm.ShowFileSize(fileInfo.Length).Split('(')[0]);
                    }
                    else if(Directory.Exists(file))
                    {
                        //为目录时
                        DirectoryInfo dirInfo = new DirectoryInfo(file);

                        ListViewItem item = fileListView.Items.Add(dirInfo.Name, (int)IconsIndexes.Folder);
                        item.Tag = dirInfo.FullName;
                        item.SubItems.Add(dirInfo.LastWriteTime.ToString());
                        item.SubItems.Add("文件夹");
                        item.SubItems.Add("");

                    }

                }
            }


            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
                FileInfo[] fileInfos = directoryInfo.GetFiles();

                //删除ilstIcons(ImageList)中的exe文件的图标，释放ilstIcons的空间
                foreach (ListViewItem item in fileListView.Items)
                {
                    if (item.Text.EndsWith(".exe"))
                    {
                        fileImageList.Images.RemoveByKey(item.Text);
                    }
                }



                //列出所有文件夹
                foreach (DirectoryInfo dirInfo in directoryInfos)
                {
                    ListViewItem item = fileListView.Items.Add(dirInfo.Name, (int)IconsIndexes.Folder);
                    item.Tag = dirInfo.FullName;
                    item.SubItems.Add(dirInfo.LastWriteTime.ToString());
                    item.SubItems.Add("文件夹");
                    item.SubItems.Add("");
                }

                //列出所有文件
                foreach (FileInfo fileInfo in fileInfos)
                {
                    ListViewItem item = fileListView.Items.Add(fileInfo.Name);

                    //为exe文件或无拓展名
                    if (fileInfo.Extension == ".exe" || fileInfo.Extension == "")
                    {
                        //通过当前系统获得文件相应图标
                        Icon fileIcon = GetSystemIcon.GetIconByFileName(fileInfo.FullName);

                        //因为不同的exe文件一般图标都不相同，所以不能按拓展名存取图标，应按文件名存取图标
                        fileImageList.Images.Add(fileInfo.Name, fileIcon);

                        item.ImageKey = fileInfo.Name;
                    }
                    //其他文件
                    else
                    {
                        if (!fileImageList.Images.ContainsKey(fileInfo.Extension))
                        {
                            Icon fileIcon = GetSystemIcon.GetIconByFileName(fileInfo.FullName);

                            //因为类型（除了exe）相同的文件，图标相同，所以可以按拓展名存取图标
                            fileImageList.Images.Add(fileInfo.Extension, fileIcon);
                        }

                        item.ImageKey = fileInfo.Extension;
                    }

                    item.Tag = fileInfo.FullName;
                    item.SubItems.Add(fileInfo.LastWriteTime.ToString());
                    item.SubItems.Add(fileInfo.Extension + "文件");
                    item.SubItems.Add(FileDetailInfoForm.ShowFileSize(fileInfo.Length).Split('(')[0]);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            //更新当前路径
            curFilePath = path;

            //更新地址栏
            tscbAddress.Text = curFilePath;

            //更新状态栏
            belowStatusStrip.Text = fileListView.Items.Count + " 个项目";

            //结束数据更新
            fileListView.EndUpdate();

        }

        //双击文件/文件夹时
        private void fileListView_ItemActivate(object sender, EventArgs e)
        {
            this.OpenDictionaryFile();
        }

        public void OpenDictionaryFile()
        {
            if (fileListView.SelectedItems.Count > 0)
            {
                //得到选中的路径
                string selectPath = fileListView.SelectedItems[0].Tag.ToString();
                try
                {
                    //如果选中的是文件夹
                    if (Directory.Exists(selectPath))
                    {
                        //打开文件夹
                        ShowFilesList(selectPath, true);
                    }
                    //如果选中的是文件
                    else
                    {
                        //打开文件
                        Process.Start(selectPath);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }


    }
}
