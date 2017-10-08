﻿using Newtonsoft.Json;
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
            string factory = "";
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if (!checkBox1.Checked)
            {
                string tms = textBox2.Text.Split('(')[1].Replace(")", "").Replace("(", "").Replace(" ", "");
                switch (tms)
                {
                    case "MinimalWear": factory = "MW"; break;
                    case "FactoryNew": factory = "FN"; break;
                    case "Field-Tested": factory = "FT"; break;
                    case "Battle-Scarred": factory = "BS"; break;
                    case "Well-Worn": factory = "WW"; break;
                }
                var item = new Program.Dat
                {
                    Id = unixTimestamp,
                    Name = textBox2.Text.Split('(')[0],
                    Factory = factory,
                    Price = Convert.ToDouble(textBox3.Text.Replace(".", ","))
                };
                DialogResult result = MessageBox.Show("Вы добавляете СТАНДАРТНЫЙ предмет","", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) //Если нажал нет
                {
                    MessageBox.Show("Не добавил!");
                }

                if (result == DialogResult.Yes) //Если нажал Да
                {
                    Program.Data.Add(item);
                    RefreshGrid();
                    string json = JsonConvert.SerializeObject(Program.Data);
                    File.WriteAllText("data.txt", json);
                    MessageBox.Show("Добавил!");
                }
                
            }
            else {
                var item = new Program.Dat
                {
                    Id = unixTimestamp,
                    Name = textBox2.Text,
                    Factory = factory,
                    Price = Convert.ToDouble(textBox3.Text.Replace(".", ","))
                };
                DialogResult result = MessageBox.Show("Вы добавляете СТИКЕР или ДРУГОЙ предмет", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) //Если нажал нет
                {
                    MessageBox.Show("Не добавил!");
                }

                if (result == DialogResult.Yes) //Если нажал Да
                {
                    Program.Data.Add(item);
                    RefreshGrid();
                    string json = JsonConvert.SerializeObject(Program.Data);
                    File.WriteAllText("data.txt", json);
                    MessageBox.Show("Добавил!");
                }
                
            }
            
        }
        private void RefreshGrid()
        {
            DGV.Rows.Clear();
            foreach (var item in Program.Data)
            {
                int rowId = DGV.Rows.Add();
                DataGridViewRow row = DGV.Rows[rowId];
                row.Cells["id2"].Value = item.Id;
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

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           textBox2.Text = Clipboard.GetText();
        }
    }
}
