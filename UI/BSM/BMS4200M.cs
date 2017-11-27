﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;

namespace WMS.UI.BSM
{
    public partial class BMS4200M : Form
    {
        public BMS4200M()
        {
            InitializeComponent();
        }

        private void BMS4200M_Load(object sender, EventArgs e)
        {

            //reoGridControl1 grid = new reoGridControl1();
            //grid.SetCellData(new ReoGridPos(2, 1), "hello world");
            var grid = new unvell.ReoGrid.ReoGridControl()
            {
                Dock = DockStyle.Fill,
            };

            this.Controls.Add(grid);  // add to form or panel
        }
    }
}
