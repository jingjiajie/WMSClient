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
                new Dictionary<string, object>{{"姓名","小明" },{ "科目1","语文"},{ "科目2","期中"},{ "成绩1",100},{ "成绩2",80} },
                new Dictionary<string, object>{{"姓名", "小明" },{ "科目1","数学"}, { "科目2", "期中" },{ "成绩1",80},{ "成绩2",90} },
                new Dictionary<string, object>{{"姓名", "小明" },{ "科目1","语文"},{ "科目2","期末"},{ "成绩1",100},{ "成绩2",80} },
                new Dictionary<string, object>{{"姓名", "小明" },{ "科目1","数学"}, { "科目2", "期末" },{ "成绩1",80},{ "成绩2",90} },
                new Dictionary<string, object>{{"姓名","小红" },{ "科目1","语文"},{ "科目2","期中"},{ "成绩1",100},{ "成绩2",80} },
                new Dictionary<string, object>{{"姓名", "小红" },{ "科目1", "数学" }, { "科目2", "期中" },{ "成绩1",80},{ "成绩2",90} },
                new Dictionary<string, object>{{"姓名", "小红" },{ "科目1", "语文" },{ "科目2","期末"},{ "成绩1",100},{ "成绩2",80} },
                new Dictionary<string, object>{{"姓名", "小红" },{ "科目1", "数学" }, { "科目2", "期末" },{ "成绩1",80},{ "成绩2",90} }
            });
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
