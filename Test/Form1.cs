﻿using FrontWork;
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
            string[] a = { "1","2"};
            object[] c = a;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.model1.AddRows(new Dictionary<string,object>[]
            {
               new Dictionary<string, object> { { "姓名","小明"} ,{ "科目","语文"},{ "成绩",100} },
               new Dictionary<string, object> { { "姓名","小明"} ,{ "科目","数学"},{ "成绩",100} },
               new Dictionary<string, object> { { "姓名","小红"} ,{ "科目","语文"},{ "成绩",100} },
               new Dictionary<string, object> { { "姓名","小红"} ,{ "科目","数学"},{ "成绩",100} },
            });
        }

        private void TestEditEnded([Model]Model model,[Row]int row,[Data] int data)
        {
            Console.WriteLine();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void reoGridView3_Load(object sender, EventArgs e)
        {

        }
    }
}
