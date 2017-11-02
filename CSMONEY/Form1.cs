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
using WebSocketSharp;
using WebSocket4Net;

using System.Net;
using WebSocketSharp.Server;
using System.Security.Cryptography.X509Certificates;

namespace CSMONEY
{

    public partial class Form1 : Form
    {
        List<string> ProxyList = new List<string>();
        int Id = 0;
        int IdLoot = 0;
        int IdCsTrade = 0;
        int IdTSF = 0;
        int IdDeals = 0;
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        } 

        private void button1_Click(object sender, EventArgs e)
        {
            Work w = new Work( Id);
            new System.Threading.Thread(delegate () {
                w.INI();
            }).Start();
            Id++;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            // textBox1.Text = driver.PageSource;
            var item = new Program.Dat {
                Id = unixTimestamp,
                Name = textBox2.Text,
             //   Factory = comboBox1.Text,
                Price = Convert.ToDouble(textBox3.Text.Replace(".",","))};
                Program.Data.Add(item);
            RefreshGrid();
            string json = JsonConvert.SerializeObject(Program.Data);
            File.WriteAllText("data.txt", json);
        }
        #region Refresh
        private void RefreshGrid()
        {
            dataGridView1.Rows.Clear();
            foreach (var item in Program.Data)
            {
                int rowId = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[rowId];
                row.Cells["id2"].Value = item.Id;
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
                    //  row.Cells["factor1"].Value = item.Factory;
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
                    //  row.Cells["factory3"].Value = item.Factory;
                    row.Cells["price3"].Value = item.Price;
                }
            }
            catch (Exception ex) { }

        }
        private void RefreshGridCsTSF()
        {
            dataGridView4.Rows.Clear();
            try
            {
                foreach (var item in Program.DataTSF)
                {
                    int rowId = dataGridView4.Rows.Add();
                    DataGridViewRow row = dataGridView4.Rows[rowId];
                    row.Cells["id4"].Value = item.Id;
                    row.Cells["name4"].Value = item.Name;
                    //  row.Cells["factory3"].Value = item.Factory;
                    row.Cells["price4"].Value = item.Price;
                }
            }
            catch (Exception ex) { }

        }
        private void RefreshGridDeals()
        {
            dataGridView5.Rows.Clear();
            try
            {
                foreach (var item in Program.DataDeals)
                {
                    int rowId = dataGridView5.Rows.Add();
                    DataGridViewRow row = dataGridView5.Rows[rowId];
                    row.Cells["id5"].Value = item.Id;
                    row.Cells["name5"].Value = item.Name;
                    //  row.Cells["factory3"].Value = item.Factory;
                    row.Cells["price5"].Value = item.Price;
                }
            }
            catch (Exception ex) { }

        }
        #endregion
        #region rTimer
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
                        listBox2.Items.Insert(0, Program.MessLoot.Dequeue());

                    }
                }
                if (Program.MessCsTrade.Count != 0)
                {
                    for (int i = 0; i < Program.MessCsTrade.Count; i++)
                    {
                        listBox3.Items.Insert(0, Program.MessCsTrade.Dequeue());
                    }
                }
                if (Program.MessTSF.Count != 0)
                {
                    for (int i = 0; i < Program.MessTSF.Count; i++)
                    {
                        listBox4.Items.Insert(0, Program.MessTSF.Dequeue());
                    }
                }
                if (Program.MessDeals.Count != 0)
                {
                    for (int i = 0; i < Program.MessDeals.Count; i++)
                    {
                        listBox5.Items.Insert(0, Program.MessDeals.Dequeue());
                    }
                }
                //if (Program.MessTelegram.Count != 0)
                //{
                //    for (int i = 0; i < Program.MessTelegram.Count; i++)
                //    {
                //        SendTelegram(Program.MessTelegram.Dequeue());
                //       // textBox9.Text = Program.MessTelegram.Dequeue() + Environment.NewLine + textBox9.Text;
                //    }
                //}

            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            string tmp = "";
            foreach (var item in listBox1.Items)
            {
                tmp = tmp + item + Environment.NewLine;
            }
            File.WriteAllText("./logCsMoney/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".txt", tmp);
            listBox1.Items.Clear();

            tmp = "";
            foreach (var item in listBox2.Items)
            {
                tmp = tmp + item + Environment.NewLine;
            }
            File.WriteAllText("./logLoot/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".txt", tmp);
            listBox2.Items.Clear();

            tmp = "";
            foreach (var item in listBox3.Items)
            {
                tmp = tmp + item + Environment.NewLine;
            }
            File.WriteAllText("./logCsTrade/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".txt", tmp);
            listBox3.Items.Clear();


            tmp = "";
            foreach (var item in listBox4.Items)
            {
                tmp = tmp + item + Environment.NewLine;
            }
            File.WriteAllText("./logTSF/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".txt", tmp);
            listBox4.Items.Clear();

            tmp = "";
            foreach (var item in listBox5.Items)
            {
                tmp = tmp + item + Environment.NewLine;
            }
            File.WriteAllText("./logDeals/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".txt", tmp);
            listBox5.Items.Clear();
        }
        #endregion
        #region Enebl
        private void enableCSMoney()
        {
            if (Properties.Settings.Default.csmoney != "")
            {
                //  var a = Properties.Settings.Default.lootfarm;
                button1.Enabled = true;
                button4.Enabled = true;
                button11.Enabled = true;
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
                button12.Enabled = true;
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
        private void enableCsTSF()
        {
            if (Properties.Settings.Default.CsTSF != "")
            {
                //  var a = Properties.Settings.Default.lootfarm;
                button15.Enabled = true;
                button14.Enabled = true;
                textBox12.Enabled = true;
            }
        }
        private void enableDeals()
        {
            if (Properties.Settings.Default.Deals != "")
            {
                //  var a = Properties.Settings.Default.lootfarm;
                button18.Enabled = true;
                button17.Enabled = true;
                textBox15.Enabled = true;
            }
        }
        #endregion
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Program.sleepMSecond = Convert.ToInt32(textBox4.Text);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox13.Text = Properties.Settings.Default.ApiKey;
            enableCSMoney();
            enableLoot();
            enableCsTrade();
            enableCsTSF();
            enableDeals();
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
            string lq = File.ReadAllText("dataCsTSF.txt");
            if (lq != "")
            {
                Program.DataTSF = JsonConvert.DeserializeObject<List<Program.Dat>>(lq);
            }
            string lw = File.ReadAllText("dataCsDeals.txt");
            if (lq != "")
            {
                Program.DataDeals = JsonConvert.DeserializeObject<List<Program.Dat>>(lw);
            }
            RefreshGrid();
            RefreshGridLootFarm();
            RefreshGridCsTrade();
            RefreshGridCsTSF();
            RefreshGridDeals();
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
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows.RemoveAt(index);
            Program.Data.RemoveAt(index);
            string json = JsonConvert.SerializeObject(Program.Data);
            File.WriteAllText("data.txt", json);
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);

        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int index = dataGridView2.CurrentCell.RowIndex;
            dataGridView2.Rows.RemoveAt(index);
            Program.DataLoot.RemoveAt(index);
            string json = JsonConvert.SerializeObject(Program.DataLoot);
            File.WriteAllText("dataLootFarm.txt", json);
            RefreshGridLootFarm();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if (!Program.pauseMoney)
            {
                Program.pauseMoney = true;
                button11.BackColor = Color.YellowGreen;
                Program.Mess.Enqueue( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Пауза установлена!");
            }
            else {
                Program.pauseMoney = false;
                button11.BackColor = Color.White;
                Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Пауза снята!");
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (!Program.pauseLoot)
            {
                Program.pauseLoot = true;
                button12.BackColor = Color.YellowGreen;
                Program.MessLoot.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Пауза установлена!");
            }
            else
            {
                Program.pauseLoot = false;
                button12.BackColor = Color.White;
                Program.MessLoot.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Пауза снята!");
            }
        }
        public struct Dat
        {
            public string Event { get; set; }
            data data { get; set; }
           
        }
        public struct data
        {
            List<Data> Data { get; set; }
        }
        public struct Data
        {
            public List<string> b { get; set; }
            public List<string> id { get; set; }
            public string m { get; set; }
            public string e { get; set; }
            public double p { get; set; }
            public Data(List<string> B, List<string> ID, string M ,string E, double P)
            {
                b = B;
                id = ID;
                m = M;
                e = E;
                p = P;


            }
            //  public string id { get; set; }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            string json = File.ReadAllText("jj.txt");
            var da = JsonConvert.DeserializeObject<dynamic>(json.Replace("event","Event"));
            var aa =da.Event.Value;
            foreach (var item in da.data)
            {
                var b = item.b[0].Value;
                var id = item.id[0].Value;
                var ee = item.e.Value;
                var m = item.m.Value;
                var p = item.p.Value;
            }

        }
      
        private void button14_Click_1(object sender, EventArgs e)
        {

            var cookies = new List<System.Net.Cookie>
            {
                new System.Net.Cookie("sessionid","0c91501fec9c30d03decded8",string.Empty, "steamcommunity.com"),
                new System.Net.Cookie("steamLogin","76561198405411962%7C%7C3611ED393F3D94D3FACFF1DFF911138A6B3C1947",string.Empty, "steamcommunity.com"),
                new System.Net.Cookie("steamLoginSecure","76561198405411962%7C%7CF4F79B0E885AAA0749E8BDFE14750218A41286A5",string.Empty,"steamcommunity.com")
            };
            string apiKey = "DBFC9AFAA97E27ADF7E4C6EF2E125D3F";
           
            //OfferSession newSteamSession = new OfferSession('yourApiKey', 'steamweb');

            //string convertedStringtradeId = String.Empty;
            //newSteamSession.
            //var isAccepted = newSteamSession.Accept(tradeOfferId, convertedStringtradeId);

            //if (isAccepted)
            //{
            //    //do more logic here if the offer was good
            //    //you can use the convertedStringtradeId if you need something


            //}
            //else
            //{
            //    //what happens when things go wrong
            //}
        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ApiKey = textBox13.Text;
            Properties.Settings.Default.Save();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            TSF w = new TSF(textBox9, IdTSF,Convert.ToInt32(textBox12.Text),textBox16.Text,ProxyList);
            new System.Threading.Thread(delegate () {
                w.INI();
            }).Start();
            IdTSF++;
        }

        private void button14_Click_2(object sender, EventArgs e)
        {
            AddCsTSF a = new AddCsTSF(dataGridView4);
            a.Show();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            AddDeals a = new AddDeals(dataGridView5);
            a.Show();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Deals w = new Deals(textBox9, IdDeals,Convert.ToInt32(textBox15.Text),textBox17.Text,ProxyList);
            new System.Threading.Thread(delegate () {
                w.INI();
            }).Start();
            IdDeals++;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox1.Text = "Отключить";
                Program.autoConfirm = true;
                textBox13.Enabled = true;
            }
            else {
                checkBox1.Text = "Включить";
                Program.autoConfirm = false;
                textBox13.Enabled = false;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            ViewBadPrice VBP = new ViewBadPrice();
            VBP.Show();
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            Program.Dat neww  = Program.Data[index];
            neww.Price = Convert.ToDouble(dataGridView1.Rows[index].Cells[3].Value.ToString());
            dataGridView1.Rows.RemoveAt(index);
            Program.Data.RemoveAt(index);
            Program.Data.Add(neww);
            string json = JsonConvert.SerializeObject(Program.Data);
            File.WriteAllText("data.txt", json);
            RefreshGrid();
            MessageBox.Show("Успешно изменено!");
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void изменитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int index = dataGridView2.CurrentCell.RowIndex;
            Program.Dat neww = Program.DataLoot[index];
            neww.Price = Convert.ToDouble(dataGridView2.Rows[index].Cells[3].Value.ToString());
            dataGridView2.Rows.RemoveAt(index);
            Program.DataLoot.RemoveAt(index);
            Program.DataLoot.Add(neww);
            string json = JsonConvert.SerializeObject(Program.DataLoot);
            File.WriteAllText("dataLootFarm.txt", json);
            RefreshGridLootFarm();
            MessageBox.Show("Успешно изменено!");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int index = dataGridView3.CurrentCell.RowIndex;
            dataGridView3.Rows.RemoveAt(index);
            Program.DataCsTrade.RemoveAt(index);
            string json = JsonConvert.SerializeObject(Program.DataCsTrade);
            File.WriteAllText("dataCsTrade.txt", json);
            RefreshGridCsTrade();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int index = dataGridView3.CurrentCell.RowIndex;
            Program.Dat neww = Program.DataCsTrade[index];
            neww.Price = Convert.ToDouble(dataGridView3.Rows[index].Cells[3].Value.ToString());
            dataGridView3.Rows.RemoveAt(index);
            Program.DataCsTrade.RemoveAt(index);
            Program.DataCsTrade.Add(neww);
            string json = JsonConvert.SerializeObject(Program.DataCsTrade);
            File.WriteAllText("dataCsTrade.txt", json);
            RefreshGridCsTrade();
            MessageBox.Show("Успешно изменено!");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            int index = dataGridView4.CurrentCell.RowIndex;
            dataGridView4.Rows.RemoveAt(index);
            Program.DataTSF.RemoveAt(index);
            string json = JsonConvert.SerializeObject(Program.DataTSF);
            File.WriteAllText("dataCsTSF.txt", json);
            RefreshGridCsTSF();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            int index = dataGridView4.CurrentCell.RowIndex;
            Program.Dat neww = Program.DataTSF[index];
            neww.Price = Convert.ToDouble(dataGridView4.Rows[index].Cells[3].Value.ToString());
            dataGridView4.Rows.RemoveAt(index);
            Program.DataTSF.RemoveAt(index);
            Program.DataTSF.Add(neww);
            string json = JsonConvert.SerializeObject(Program.DataTSF);
            File.WriteAllText("dataCsTSF.txt", json);
            RefreshGridCsTSF();
            MessageBox.Show("Успешно изменено!");
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            int index = dataGridView5.CurrentCell.RowIndex;
            dataGridView5.Rows.RemoveAt(index);
            Program.DataDeals.RemoveAt(index);
            string json = JsonConvert.SerializeObject(Program.DataDeals);
            File.WriteAllText("dataCsDeals.txt", json);
            RefreshGridDeals();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            int index = dataGridView5.CurrentCell.RowIndex;
            Program.Dat neww = Program.DataDeals[index];
            neww.Price = Convert.ToDouble(dataGridView5.Rows[index].Cells[3].Value.ToString());
            dataGridView5.Rows.RemoveAt(index);
            Program.DataDeals.RemoveAt(index);
            Program.DataDeals.Add(neww);
            string json = JsonConvert.SerializeObject(Program.DataDeals);
            File.WriteAllText("dataCsDeals.txt", json);
            RefreshGridDeals();
            MessageBox.Show("Успешно изменено!");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            ProxyList.Clear();
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            string[] proxy = System.IO.File.ReadAllLines(filename);
            foreach (var item in proxy)
            {
                ProxyList.Add(item);
            }
            MessageBox.Show("подгрузил прокси в количестве:" + ProxyList.Count);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            ProxyList.Clear();
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            string[] proxy = System.IO.File.ReadAllLines(filename);
            foreach (var item in proxy)
            {
                ProxyList.Add(item);
            }
            MessageBox.Show("подгрузил прокси в количестве:" + ProxyList.Count);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Program.BrowesrQuery = true;
            }
            else { Program.BrowesrQuery = false; }
        }



    }

}
