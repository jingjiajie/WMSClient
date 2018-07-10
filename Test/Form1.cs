using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void TestEditEnded([Model]Model model,[Row]int row,[Data] int data)
        {
            Console.WriteLine();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            sourceModel.AddRows(new Dictionary<string, object>[]
            {
                new Dictionary<string, object>{{"姓名","1" },{ "科目1","语文"},{ "科目2","期中"},{ "成绩1",100},{ "成绩2",80} },
                new Dictionary<string, object>{{"姓名","1" },{ "科目1","数学"}, { "科目2", "期中" },{ "成绩1",80},{ "成绩2",90} },
                new Dictionary<string, object>{{"姓名","1" },{ "科目1","语文"},{ "科目2","期末"},{ "成绩1",100},{ "成绩2",80} },
                new Dictionary<string, object>{{"姓名","1" },{ "科目1","数学"}, { "科目2", "期末" },{ "成绩1",80},{ "成绩2",90} }
            });
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            sourceModel.InsertRows(new int[] { 0,1,2,3 }, new IDictionary<string, object>[]
            {
                new Dictionary<string, object>{{"姓名","2" },{ "科目1","语文"},{ "科目2","期中"},{ "成绩1",100},{ "成绩2",80} },
                new Dictionary<string, object>{{"姓名","2" },{ "科目1", "数学" }, { "科目2", "期中" },{ "成绩1",80},{ "成绩2",90} },
                new Dictionary<string, object>{{"姓名","2" },{ "科目1", "语文" },{ "科目2","期末"},{ "成绩1",100},{ "成绩2",80} },
                new Dictionary<string, object>{{"姓名","2" },{ "科目1", "数学" }, { "科目2", "期末" },{ "成绩1",80},{ "成绩2",90} },
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("姓名"),
                new DataColumn("科目1"),
                new DataColumn("科目2"),
                new DataColumn("成绩1"),
                new DataColumn("成绩2"),
            });
            //dataTable.Rows.Add("小明", "语文", "期中", 100, 60);
            ////ModelRefreshArgs args = new ModelRefreshArgs(dataTable, null);
            //this.sourceModel.Refresh(args);
            this.sourceModel.AddRow(null);
        }
    }
}
