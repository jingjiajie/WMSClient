using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FromSalary
{
    public partial class FormSelectTax : Form
    {
        public FormSelectTax(int payNoteId,string payNoteNo)
        {
            InitializeComponent();
            this.model1[0]["payNoteId"] = payNoteId;
            this.model1[0]["payNoteN0"] = payNoteNo;
            FrontWork.Range[] range = new FrontWork.Range[] { new FrontWork.Range(0,0,1,1)};
            this.model1.SetSelectionRanges(range);
        }
    }
}
