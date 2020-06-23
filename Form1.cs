using System;
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
using System.Security.AccessControl;
using Microsoft.VisualBasic.Devices;

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

        //搜索的文件名字
        private string searchFileName = "";

        //存放搜索结果的List
        List<SearchInfo> searchInfoList = new List<SearchInfo>();

        //是否移动文件
        private bool isMove = false;

        //待复制并粘贴的文件\文件夹的源路径
        private string[] copyFilesSourcePaths = new string[200];

        //主窗口显示
        public FileManage()
        {
            InitializeComponent();
            //System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        //界面初始显示状态
        private void FileManage_Load(object sender, EventArgs e)
        {
            //界面显示
            TreeViewShow treeViewShow = new TreeViewShow();
            treeViewShow.InitDisplay(ref this.deviceTreeView);
            //设置本地磁盘C为默认点击状态
            deviceTreeView.SelectedNode = deviceTreeView.Nodes[1];
        }
        //窗口大小变化时间响应
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

        //上方菜单栏事件，查看状态栏信息栏显示
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

        //大图标
        private void bigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetViewChecks();
            bigToolStripMenuItem.Checked = true;
            cbigToolStripMenuItem.Checked = true;
            fileListView.View = View.LargeIcon;
        }
        //小图标
        private void smallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetViewChecks();
            smallToolStripMenuItem.Checked = true;
            csmallToolStripMenuItem.Checked = true;
            fileListView.View = View.SmallIcon;

        }

        //列表查看
        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetViewChecks();
            listToolStripMenuItem.Checked = true;
            clistToolStripMenuItem.Checked = true;
            fileListView.View = View.List;

        }

        //详细信息
        private void moreInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetViewChecks();
            moreInfoToolStripMenuItem.Checked = true;
            cmoreToolStripMenuItem.Checked = true;
            fileListView.View = View.Details;

        }
        //新建文件夹
        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //菜单栏新建文件夹
            CreateFolder();
        }

        //创建文件
        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFile();
        }

        //树节点展开
        private void deviceTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.Expand();
        }
        //树节点打开前导入子节点
        private void deviceTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeViewShow.LoadChildNodes(e.Node);
        }

        //选定树节点
        private void deviceTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //第一次初始化,显示C盘的文件夹在ListView中
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
                //由于获取系统历史文件访问目录，使用同步方法，会造成界面阻塞，所以在点击历史访问时候使用异步操作

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
        //搜索按钮事件处理
        private void tscbSearch_Enter(object sender, EventArgs e)
        {
            tscbSearch.Text = "";
        }

        private void tscbSearch_Leave(object sender, EventArgs e)
        {
            tscbSearch.Text = "快速搜索";
        }

        private void tscbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            //开始文件搜索

            //得到回车输入的文件名
            if (e.KeyCode == Keys.Enter)
            {
                searchInfoList.Clear();
                string enterFilename = tscbSearch.Text;
                //判空
                if (string.IsNullOrEmpty(enterFilename))
                {
                    //弹出对话框
                    MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;
                    DialogResult dialog = MessageBox.Show("正确输入搜索文件名", "提示", messageBoxButtons);
                    return;

                }

                searchFileName = enterFilename;
                Search(curFilePath);

                ShowSearchRes();

            }
        }

        //双击文件/文件夹时
        private void fileListView_ItemActivate(object sender, EventArgs e)
        {
            this.OpenDictionaryFile();
        }

        private void ownerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //显示权限管理
            ShowPrivilegeForm();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //复制文件
            CopyOperator();
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //粘贴
            PastOperator();
        }
        private void shearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //剪切
            CutOperator();
        }
        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //删除
            DeleteOperator();
        }

        //Listview按钮操作
        //刷新
        private void crefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFilesList(curFilePath, false);
        }

        private void copenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDictionaryFile();
        }

        private void crenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //文件重命名
            RenameFile();
        }

        //显示文件到Listview中
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

                //遍历，将文件显示到ListView，使用异步委托进行处理
                Action action = new Action(() =>
                {
                    foreach (var file in recentFile)
                    {
                        if (File.Exists(file))
                        {
                            //当为文件时
                            FileInfo fileInfo = new FileInfo(file);

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
                            //显示文件相关信息

                            item.Tag = fileInfo.FullName;
                            item.SubItems.Add(fileInfo.LastWriteTime.ToString());
                            item.SubItems.Add(fileInfo.Extension + "文件");
                            item.SubItems.Add(FileDetailInfoForm.ShowFileSize(fileInfo.Length).Split('(')[0]);
                        }
                        else if (Directory.Exists(file))
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
                });

                fileListView.BeginInvoke(action);
               
            }
            else
            {
                //得到硬盘目录下的文件夹及文件。
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

            }    
            //更新当前路径
            curFilePath = path;

            //更新地址栏
            tscbAddress.Text = curFilePath;

            //更新状态栏
            belowTsslbNum.Text = fileListView.Items.Count + " 个项目";

            //结束数据更新
            fileListView.EndUpdate();

        }


        //打开文件或文件夹
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


        private void ShowSearchRes()
        {
            //开始数据更新
            fileListView.BeginUpdate();

            //清空ListView
            fileListView.Items.Clear();

            lock (searchInfoList)
            {
                //foreach (var info in searchInfoList)
                for(int i = 0; i < searchInfoList.Count; i++)
                {
                    Console.WriteLine("searchInfoList:{0}", searchInfoList[i]);
                    //是文件的话
                    if (searchInfoList[i].IsFile)
                    {
                        FileInfo fileInfo = new FileInfo(searchInfoList[i].Name);

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
                        item.SubItems.Add(fileInfo.LastWriteTimeUtc.ToString());
                        item.SubItems.Add(fileInfo.Extension + "文件");
                        item.SubItems.Add(FileDetailInfoForm.ShowFileSize(fileInfo.Length).Split('(')[0]);

                    }
                    //是文件夹
                    else
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(searchInfoList[i].Name);
                        ListViewItem item = fileListView.Items.Add(dirInfo.Name, (int)IconsIndexes.Folder);
                        item.Tag = dirInfo.FullName;
                        item.SubItems.Add(dirInfo.LastWriteTimeUtc.ToString());
                        item.SubItems.Add("文件夹");
                        item.SubItems.Add("");
                    }
                    fileListView.EndUpdate();
                }
            }

        }


        //搜索委托函数
        public void Search(Object obj)
        {
            string path = obj.ToString();
            try
            {
                //待搜索路径下的目录
                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                //待搜索路径下的文件
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                //搜索文件
                if (fileInfos.Length > 0)
                {
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        try
                        {
                            //匹配文件名
                            if (fileInfo.Name.Split('.')[0].Contains(searchFileName))
                            {
                                SearchInfo searchInfo = new SearchInfo(fileInfo.FullName, true);
                                searchInfoList.Add(searchInfo);
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }
                }
                //待搜索路径下的子文件夹
                DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();

                //搜索文件夹
                if (directoryInfos.Length > 0)
                {
                    foreach (DirectoryInfo dirInfo in directoryInfos)
                    {
                        try
                        {
                            if (dirInfo.Name.Contains(searchFileName))
                            {
                                SearchInfo searchInfo = new SearchInfo(dirInfo.FullName, false);
                                searchInfoList.Add(searchInfo);
                            }
                            else
                            {
                                //继续回调搜索文件
                                ThreadPool.QueueUserWorkItem(new WaitCallback(Search), dirInfo.FullName);
                            }
                        }
                        catch (Exception e)
                        { }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("{0}", e.Message);
            }
        }

 

        private void CreateFolder()
        {
            if (curFilePath == @"最近访问")
            {
                MessageBox.Show("不能在当前路径下新建文件夹！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int num = 1;
                string path = Path.Combine(curFilePath, "新建文件夹");
                string newFolderPath = path;

                while (Directory.Exists(newFolderPath))
                {
                    newFolderPath = path + "(" + num + ")";
                    num++;
                }

                Directory.CreateDirectory(newFolderPath);

                ListViewItem item = fileListView.Items.Add("新建文件夹" + (num == 1 ? "" : "(" + (num - 1) + ")"), (int)IconsIndexes.Folder);

                //真正的路径
                item.Tag = newFolderPath;

                //刷新左边的目录树
                TreeViewShow.LoadChildNodes(curSelectedNode);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //创建文件
        private void CreateFile()
        {
            //最近访问不可以增加新文件
            if(curFilePath == @"最近访问")
            {
                MessageBox.Show("不能在当前路径下新建文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //创建文件窗口显示
            NewFileForm newFileForm = new NewFileForm(curFilePath, this);
            newFileForm.Show();

        }

        //显示权限管理窗口
        private void ShowPrivilegeForm()
        {
            //右边窗体中没有文件/文件夹被选中
            if (fileListView.SelectedItems.Count == 0)
            {
                if (curFilePath == "最近访问")
                {
                    MessageBox.Show("不能查看当前路径的权限管理！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                PrivilegeForm privilegeForm = new PrivilegeForm(curFilePath);

                //显示对于当前文件夹的权限管理界面
                privilegeForm.Show();
            }
            //右边窗体中有文件/文件夹被选中
            else
            {
                //显示被选中的第一个文件/文件夹的权限管理界面
                PrivilegeForm privilegeForm = new PrivilegeForm(fileListView.SelectedItems[0].Tag.ToString());

                privilegeForm.Show();
            }

        }

        private void propertyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //没有选中文件/文件夹
            if(fileListView.SelectedItems.Count == 0)
            {
                if (curFilePath == "最近访问")
                {
                    MessageBox.Show("不能查看当前路径的属性！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                FileDetailInfoForm fileDetail = new FileDetailInfoForm(curFilePath);

                fileDetail.Show();
            }
            else
            {
                //显示文件的详细信息
                FileDetailInfoForm fileDetail = new FileDetailInfoForm(fileListView.SelectedItems[0].Tag.ToString());
                fileDetail.Show();

            }
        }

       
        //复制操作
        private void CopyOperator()
        {
            SetCopyFilesSourcePaths();
        }

        //获得待复制文件的源路径
        private void SetCopyFilesSourcePaths()
        {
            if (fileListView.SelectedItems.Count > 0)
            {
                int i = 0;

                foreach (ListViewItem item in fileListView.SelectedItems)
                {
                    copyFilesSourcePaths[i++] = item.Tag.ToString();
                }

                isMove = false;
            }
        }



        //粘贴操作
        private void PastOperator()
        {
            //没有待粘贴的文件
            if (copyFilesSourcePaths[0] == null)
            {
                return;
            }

            //当前路径无效
            if (!Directory.Exists(curFilePath))
            {
                return;
            }

            if (curFilePath == "最近访问")
            {
                MessageBox.Show("不能在当前路径下进行粘贴操作！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; copyFilesSourcePaths[i] != null; i++)
            {
                //如果是文件
                if (File.Exists(copyFilesSourcePaths[i]))
                {
                    //执行文件的“移动到”或“复制到”
                    MoveToOrCopyToFileBySourcePath(copyFilesSourcePaths[i]);
                }
                //如果是文件夹
                else if (Directory.Exists(copyFilesSourcePaths[i]))
                {
                    //执行文件夹的“移动到”或“复制到”
                    MoveToOrCopyToDirectoryBySourcePath(copyFilesSourcePaths[i]);
                }

            }

            //在右边窗体显示文件列表
            ShowFilesList(curFilePath, false);

            //刷新左边的目录树
            TreeViewShow.LoadChildNodes(curSelectedNode);

            //置空
            copyFilesSourcePaths = new string[200];

        }

        //执行文件的“移动到”或“复制到”
        private void MoveToOrCopyToFileBySourcePath(string sourcePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(sourcePath);

                //获取目的路径
                string destPath = Path.Combine(curFilePath, fileInfo.Name);

                //如果目的路径和源路径相同，则不执行任何操作
                if (destPath == sourcePath)
                {
                    return;
                }

                //移动文件到目的路径（当前是在执行“剪切+粘贴”操作）
                if (isMove)
                {
                    fileInfo.MoveTo(destPath);
                }
                //粘贴文件到目的路径（当前是在执行“复制+粘贴”操作）
                else
                {
                    fileInfo.CopyTo(destPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //通过递归，复制并粘贴文件夹（包含文件夹下的所有文件）
        private void CopyAndPasteDirectory(DirectoryInfo sourceDirInfo, DirectoryInfo destDirInfo)
        {
            //判断目标文件夹是否是源文件夹的子目录，是则给出错误提示，不进行任何操作
            for (DirectoryInfo dirInfo = destDirInfo.Parent; dirInfo != null; dirInfo = dirInfo.Parent)
            {
                if (dirInfo.FullName == sourceDirInfo.FullName)
                {
                    MessageBox.Show("无法复制！目标文件夹是源文件夹的子目录！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //创建目标文件夹
            if (!Directory.Exists(destDirInfo.FullName))
            {
                Directory.CreateDirectory(destDirInfo.FullName);
            }

            //复制文件并将文件粘贴到目标文件夹下
            foreach (FileInfo fileInfo in sourceDirInfo.GetFiles())
            {
                fileInfo.CopyTo(Path.Combine(destDirInfo.FullName, fileInfo.Name));
            }

            //递归复制并将子文件夹粘贴到目标文件夹下
            foreach (DirectoryInfo sourceSubDirInfo in sourceDirInfo.GetDirectories())
            {
                DirectoryInfo destSubDirInfo = destDirInfo.CreateSubdirectory(sourceSubDirInfo.Name);
                CopyAndPasteDirectory(sourceSubDirInfo, destSubDirInfo);
            }
        }

        //执行文件夹的“移动到”或“复制到”
        private void MoveToOrCopyToDirectoryBySourcePath(string sourcePath)
        {
            try
            {
                DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(sourcePath);

                //获取目的路径
                string destPath = Path.Combine(curFilePath, sourceDirectoryInfo.Name);

                //如果目的路径和源路径相同，则不执行任何操作
                if (destPath == sourcePath)
                {
                    return;
                }

                //移动文件夹到目的路径（当前是在执行“剪切+粘贴”操作）
                if (isMove)
                {
                    //若使用sourceDirectoryInfo.MoveTo(destPath)，则不支持跨磁盘移动文件夹

                    //通过递归，复制并粘贴文件夹（包含文件夹下的所有文件）
                    CopyAndPasteDirectory(sourceDirectoryInfo, new DirectoryInfo(destPath));

                    //删除源文件夹
                    Directory.Delete(sourcePath, true);

                }
                //粘贴文件夹到目的路径（当前是在执行“复制+粘贴”操作）
                else
                {
                    //通过递归，复制并粘贴文件夹（包含文件夹下的所有文件）
                    CopyAndPasteDirectory(sourceDirectoryInfo, new DirectoryInfo(destPath));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //剪切操作
        private void CutOperator()
        {
            //获得待复制文件的源路径
            SetCopyFilesSourcePaths();

            //准备移动
            isMove = true;
        }


        //删除操作
        private void DeleteOperator()
        {
            if(fileListView.SelectedItems.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("确定要删除吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                else
                {
                    try
                    {
                        foreach (ListViewItem item in fileListView.SelectedItems)
                        {
                            string path = item.Tag.ToString();

                            //如果是文件
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                            //如果是文件夹
                            else if (Directory.Exists(path))
                            {
                                Directory.Delete(path, true);
                            }

                            fileListView.Items.Remove(item);
                        }

                        //刷新左边的目录树
                       TreeViewShow.LoadChildNodes(curSelectedNode);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            
        }

        //初始化相关“查看”选项
        private void InitViewChecks()
        {
            //默认右边窗体显示的是详细信息视图
            toolToolStripMenuItem.Checked = true;
            stateToolStripMenuItem.Checked = true;
        }

        //重置相关“查看”选项
        private void ResetViewChecks()
        {
            bigToolStripMenuItem.Checked = false;
            smallToolStripMenuItem.Checked = false;
            listToolStripMenuItem.Checked = false;
            moreInfoToolStripMenuItem.Checked = false;
        }



        private void RenameFile()
        {
            if (fileListView.SelectedItems.Count > 0)
            {
                //模拟进行编辑标签，实质是为了通过代码触发LabelEdit事件
                fileListView.SelectedItems[0].BeginEdit();
            }
        }
        //文件重命名
        private void fileListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            //获取新的名字
            string name = e.Label;

            //获得选中项
            ListViewItem selectedItem = fileListView.SelectedItems[0];

            if(string.IsNullOrEmpty(name))
            {
                MessageBox.Show("文件名不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //显示时，恢复原来的标签
                e.CancelEdit = true;
            }
            //标签没有改动
            else if (name == null)
            {
                return;
            }
            //标签改动了，但是最终还是和原来一样
            else if (name == selectedItem.Text)
            {
                return;
            }
            //文件名不合法
            else if (!IsValidFileName(name))
            {
                MessageBox.Show("文件名不能包含下列任何字符:\r\n" + "\t\\/:*?\"<>|", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //显示时，恢复原来的标签
                e.CancelEdit = true;
            }
            else
            {
                //开始重命名
                //Computr操纵文件系统
                Computer myComputer = new Computer();

                //重名的是文件
                if(File.Exists(selectedItem.Tag.ToString()))
                {
                    //若包含重复文件, 则添加后缀
                    if(File.Exists(Path.Combine(curFilePath, name)))
                    {
                        int num = 1;
                        string newName = Path.Combine(curFilePath, name);
                        string reName = name;
                        while(File.Exists(newName))
                        {
                            newName = newName + "(" + num + ")";
                            reName = name + "(" + num + ")";
                            num++;
                        }
                        name = reName;
                    }
                    //重命名文件
                    myComputer.FileSystem.RenameFile(selectedItem.Tag.ToString(), name);
                    //更新显示
                    FileInfo fileInfo = new FileInfo(selectedItem.Tag.ToString());
                    string parentPath = Path.GetDirectoryName(fileInfo.FullName);
                    string newPath = Path.Combine(parentPath, name);

                    //更新选中项的Tag
                    selectedItem.Tag = newPath;

                    //刷新左边的目录树
                    TreeViewShow.LoadChildNodes(curSelectedNode);
                }
                else if(Directory.Exists(selectedItem.Tag.ToString()))
                {
                    //重命名的是文件夹
                    //存在重复名情况
                    if (Directory.Exists(Path.Combine(curFilePath, name)))
                    {
                        int num = 1;
                        string newName = Path.Combine(curFilePath, name);
                        string reName = name;
                        while (File.Exists(newName))
                        {
                            newName = newName + "(" + num + ")";
                            reName = name + "(" + num + ")";
                            num++;
                        }
                        name = reName;
                    }
                    
                    //更新名字
                    myComputer.FileSystem.RenameDirectory(selectedItem.Tag.ToString(), name);

                    DirectoryInfo directoryInfo = new DirectoryInfo(selectedItem.Tag.ToString());
                    string parentPath = directoryInfo.Parent.FullName;
                    string newPath = Path.Combine(parentPath, name);

                    //更新选中项的Tag
                    selectedItem.Tag = newPath;

                    //刷新左边的目录树
                    TreeViewShow.LoadChildNodes(curSelectedNode);

                }
            }
            //刷新显示
            ShowFilesList(curFilePath, false);
        }

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
