using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }
        int Id = 0;
        int IdLoot = 0;
        int IdCsTrade = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            Work w = new Work( Id);
            new System.Threading.Thread(delegate () {
                w.INI();
            }).Start();
            Id++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            // textBox1.Text = driver.PageSource;
            var item = new Program.Dat {
                Id = unixTimestamp,
                Name = textBox2.Text,
                Factory = comboBox1.Text,
                Price = Convert.ToDouble(textBox3.Text.Replace(".",","))};
                Program.Data.Add(item);
            RefreshGrid();
            string json = JsonConvert.SerializeObject(Program.Data);
            File.WriteAllText("data.txt", json);
        }
        private void RefreshGrid()
        {
            dataGridView1.Rows.Clear();
            foreach (var item in Program.Data)
            {      
                int rowId = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[rowId];
                row.Cells["Id"].Value = item.Id;
                row.Cells["Name"].Value = item.Name;
                row.Cells["Factor"].Value = item.Factory;
                row.Cells["Price"].Value = item.Price;
            }
           
        }
        private void RefreshGridLootFarm()
        {
            dataGridView2.Rows.Clear();
            try
            {
                foreach (var item in Program.DataLoot)
                {
                    int rowId = dataGridView2.Rows.Add();
                    DataGridViewRow row = dataGridView2.Rows[rowId];
                    row.Cells["id1"].Value = item.Id;
                    row.Cells["name1"].Value = item.Name;
                    row.Cells["factor1"].Value = item.Factory;
                    row.Cells["price1"].Value = item.Price;
                }
            }
            catch (Exception ex) { }

        }
        private void RefreshGridCsTrade()
        {
            dataGridView3.Rows.Clear();
            try
            {
                foreach (var item in Program.DataCsTrade)
                {
                    int rowId = dataGridView3.Rows.Add();
                    DataGridViewRow row = dataGridView3.Rows[rowId];
                    row.Cells["id3"].Value = item.Id;
                    row.Cells["name3"].Value = item.Name;
                    row.Cells["factory3"].Value = item.Factory;
                    row.Cells["price3"].Value = item.Price;
                }
            }
            catch (Exception ex) { }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Program.Mess.Count != 0)
                {
                    for (int i = 0; i < Program.Mess.Count; i++)
                    {
                        listBox1.Items.Insert(0, Program.Mess.Dequeue());
                       // textBox1.Text = Program.Mess.Dequeue() + Environment.NewLine + textBox1.Text;
                    }
                }
                if (Program.MessLoot.Count != 0)
                {
                    for (int i = 0; i < Program.MessLoot.Count; i++)
                    {
                        textBox5.Text = Program.MessLoot.Dequeue() + Environment.NewLine + textBox5.Text;
                    }
                }
                if (Program.MessCsTrade.Count != 0)
                {
                    for (int i = 0; i < Program.MessCsTrade.Count; i++)
                    {
                        textBox9.Text = Program.MessCsTrade.Dequeue() + Environment.NewLine + textBox9.Text;
                    }
                }
            }
            catch (Exception)
            {

            }
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Program.sleepMSecond = Convert.ToInt32(textBox4.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            enableCSMoney();
            enableLoot();
            enableCsTrade();
            timer2.Start();
            try
            {
                string jj = File.ReadAllText("data.txt");
                if (jj != "")
                {
                    Program.Data = JsonConvert.DeserializeObject<List<Program.Dat>>(jj);
                }
            }
            catch (Exception ex) { Program.Mess.Enqueue("" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + ex.Message); }
            string ll = File.ReadAllText("dataLootFarm.txt");
            if (ll != "")
            {
                Program.DataLoot = JsonConvert.DeserializeObject<List<Program.Dat>>(ll);
            }
            string lk = File.ReadAllText("dataCsTrade.txt");
            if (lk != "")
            {
                Program.DataCsTrade = JsonConvert.DeserializeObject<List<Program.Dat>>(lk);
            }
            RefreshGrid();
            RefreshGridLootFarm();
            RefreshGridCsTrade();
        }
        private void enableCSMoney()
        {
            if (Properties.Settings.Default.csmoney != "")
            {
              //  var a = Properties.Settings.Default.lootfarm;
                button1.Enabled = true;
                button4.Enabled = true;
                textBox4.Enabled = true;
            }
        }
        private void enableLoot()
        {
            if (Properties.Settings.Default.lootfarm != "")
            {
              //  var a = Properties.Settings.Default.lootfarm;
                button5.Enabled = true;
                button6.Enabled = true;
                textBox7.Enabled = true;
            }
        }
        private void enableCsTrade()
        {
            if (Properties.Settings.Default.CsTrade != "")
            {
                //  var a = Properties.Settings.Default.lootfarm;
                button9.Enabled = true;
                button8.Enabled = true;
                textBox11.Enabled = true;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            string tmp = "";
            foreach (var item in listBox1.Items)
            {
                tmp = tmp + item + Environment.NewLine;
            }
            File.WriteAllText("./logCsMoney/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff")+".txt", tmp);
            listBox1.Items.Clear();
            tmp = "";
            File.WriteAllText("./logLoot/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".txt", textBox5.Text);
            textBox5.Text = "";
            File.WriteAllText("./logCsTrade/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".txt", textBox9.Text);
            textBox9.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Add a = new Add(dataGridView1);
            a.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddLoot a = new AddLoot(dataGridView2);
            a.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LootFarm w = new LootFarm(textBox5, IdLoot);
            new System.Threading.Thread(delegate () {
                w.INI();
            }).Start();
            IdLoot++;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            Program.sleepMSecondLoot = Convert.ToInt32(textBox7.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // CsTrade trade = new CsTrade(textBox9, 1);
            CsTrade w = new CsTrade(textBox9, IdCsTrade);
            new System.Threading.Thread(delegate () {
                w.INI();
            }).Start();
            IdCsTrade++;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddCsTrade a = new AddCsTrade(dataGridView3);
            a.Show();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            Program.sleepMCsTrade = Convert.ToInt32(textBox11.Text);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
          
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows.RemoveAt(index);
            Program.Data.RemoveAt(index);
            string json = JsonConvert.SerializeObject(Program.Data);
            File.WriteAllText("data.txt", json);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
