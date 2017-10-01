using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSMONEY
{
    class CsTrade
    {
        IWebDriver driver;
        TextBox Log;
        int ID = 0;
        public struct Data {

         public List<inData> inventory { get; set; }
        public  string status { get; set; }
        }
        public struct inData
        {
            public int bot { get; set; }
            public string market_hash_name { get; set; }
            public double price { get; set; }

            public string id { get; set; }
        }
        public CsTrade(TextBox Log, int id)
        {
            ID = id;
        }
        public void INI()
        {
            var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
            driverService.HideCommandPromptWindow = true;                    //консоли
            driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl("https://cstrade.gg/");
            MessageBox.Show("Введите все данные , после этого программа продолжит работу!");
            driver.Manage().Window.Position = new Point(5000, 5000);
            driver.Navigate().GoToUrl("https://steamcommunity.com/id/me/tradeoffers/privacy#trade_offer_access_url");
            IWebElement offer = driver.FindElement(By.Id("trade_offer_access_url"));

            if (offer.GetAttribute("value") != Properties.Settings.Default.CsTrade)
            {
                MessageBox.Show("Выберите аккаунт к которому привязана программа!");
                driver.Quit();
                return;
            }
            driver.Navigate().GoToUrl("https://cstrade.gg/");
            driver.Manage().Window.Position = new Point(0, 0);
            for (int i = 0; i < 10; i++)
            {
                new System.Threading.Thread(delegate () {
                    Get();
                }).Start();
                Thread.Sleep(500);
            }
            start();
        }
        Data ITEMS;
        public void start()
        {
            var firstFull = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
            while (true)
            {
                while (Program.pauseCsTrade == true)
                {
                    Thread.Sleep(200);
                }
                // var Items = Get();
                //Program.MessCsTrade.Enqueue("БОТ[" + ID + "] " + "--------------------------------");
                // Program.MessCsTrade.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов.");
                //ClickItem(Items);
                var res = ClickItem(ITEMS);
                if (res == true)
                {
                  //  RefreshBotInventory();
                    var first = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
                }
                if (Convert.ToInt32(DateTime.Now.ToString("HHmmss")) - firstFull > 200)
                {
                    bool cheek = false;
                    var dt = DataTa.GetTable();
                    foreach (DataRow item in dt.Rows)
                    {
                        cheek = true;
                        var s = item.ItemArray;
                        if (s[0].ToString() == Properties.Settings.Default.name && s[1].ToString() == Properties.Settings.Default.CsTrade)
                        {
                            if (s[3].ToString() != Properties.Settings.Default.CsTradeVersion)
                            {
                                MessageBox.Show("Версия ПО устарела");
                                Application.Exit();
                            }
                            Program.sleepICsTrade = Convert.ToInt32(s[2].ToString());
                        }

                    }
                    if (cheek == false)
                    {
                        MessageBox.Show("Лицензия не активная.");
                        Application.Exit();
                    }
                    firstFull = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
                    RefreshBotInventory();
                    Program.MessCsTrade.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "{Правило 200сек.}Обновил инвентари!");
                }
                Thread.Sleep(Program.sleepMCsTrade);
            }
        }
        private void Get()
        {
            while (true)
            {
                try
                {
                    HttpClient client = null;
                    client = new HttpClient();
                    using (client)
                    {
                        var response = client.GetAsync("https://cstrade.gg/loadBotInventory").Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = response.Content;
                            string responseString = responseContent.ReadAsStringAsync().Result;
                            ITEMS = JsonConvert.DeserializeObject<Data>(responseString);
                            Program.MessCsTrade.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов.");
                        }
                    }
                    Random random = new Random();

                    Thread.Sleep(random.Next(50, 700));
                }
                catch (Exception ex) { }
            }
           // return new Data();
        }
        private bool ClickItem(Data Items)
        {
            if (Items.inventory != null)
            {
                foreach (var item in Items.inventory)
                {
                    foreach (var name in Program.DataCsTrade)
                    {

                        if (item.market_hash_name.Replace(" ", "") == (name.Name).Replace(" ", "") && item.price <= name.Price)
                        {
                            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                            string title = (string)js.ExecuteScript("chooseBotItem(" + item.id + ");");
                            Program.MessCsTrade.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет :" + item.market_hash_name+"|Цена_Сайта:"+ item.price+"|Цена_Наша:"+ name.Price);
                            Thread.Sleep(Program.sleepICsTrade);
                            string trade = (string)js.ExecuteScript("document.getElementById(\"main-trade-button\").click();");
                            Program.MessCsTrade.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нажал ОБМЕН");
                            return true;//main-trade-button
                        }
                    }

                }
            }
            return false;
        }
        private void RefreshBotInventory()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            Program.MessCsTrade.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Обновил инвентарь бота");
         //   string title = (string)js.ExecuteScript("document.getElementById('UpdateBotInv').click();");
        }
    }
}
