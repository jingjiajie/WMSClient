using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;

namespace WMS.UI
{
    public partial class FormSelectPackage : Form
    {
        string selectedItem = null;
        private FormSelectPackage()
        {
            InitializeComponent();
        }

        private void FormSelectCategory_Load(object sender, EventArgs e)
        {

        }

        private void Search()
        {
            using (WMSEntities wmsEntities = new WMSEntities())
            {
                string[] categories = wmsEntities.Database.SqlQuery<string>
                    (string.Format(@"SELECT DISTINCT Package 
                       FROM Supply
                       WHERE ProjectID = {0}
                        AND WarehouseID = {1}
                        AND len(Package) > 0 ", GlobalData.ProjectID, GlobalData.WarehouseID)).ToArray();
                this.listBox.Items.Clear();
                this.listBox.Items.AddRange(categories);
            }
        }

        public static string SelectCategory()
        {
            FormSelectPackage form = new FormSelectPackage();
            form.Search();
            form.ShowDialog();
            return form.selectedItem;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(this.listBox.SelectedItem == null)
            {
                this.selectedItem = null;
            }
            else
            {
                this.selectedItem = this.listBox.SelectedItem.ToString();
            }
            this.Close();
        }
    }
}
