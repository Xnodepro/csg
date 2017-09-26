using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSMONEY
{
    public partial class Add : Form
    {
        DataGridView DGV;
        public Add(DataGridView dgv)
        {
            DGV = dgv;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            // textBox1.Text = driver.PageSource;
            var item = new Program.Dat
            {
                Id = unixTimestamp,
                Name = textBox2.Text,
                Factory = comboBox1.Text,
                Price = Convert.ToDouble(textBox3.Text.Replace(".", ","))
            };
            Program.Data.Add(item);
            RefreshGrid();
            string json = JsonConvert.SerializeObject(Program.Data);
            File.WriteAllText("data.txt", json);
        }
        private void RefreshGrid()
        {
            DGV.Rows.Clear();
            foreach (var item in Program.Data)
            {
                int rowId = DGV.Rows.Add();
                DataGridViewRow row = DGV.Rows[rowId];
                row.Cells["Id"].Value = item.Id;
                row.Cells["Name"].Value = item.Name;
                row.Cells["Factor"].Value = item.Factory;
                row.Cells["Price"].Value = item.Price;
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
