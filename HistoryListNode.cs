using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    //保存历史访问路径
    public class HistoryListNode
    {
        //保存的路径
        public string Path { set; get; }
        public HistoryListNode PreNode { set; get; }
        public HistoryListNode NextNode { set; get; }

        public HistoryListNode()
        {
            Path = null;
            PreNode = null;
            NextNode = null;
        }
    }
}
