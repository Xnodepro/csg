﻿using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSMONEY
{
    class TSF
    {
        IWebDriver driver;
        int ID = 0;
        dynamic ITEMS;
        List<System.Net.Cookie> cook;
        List<System.Net.Cookie> cookAll;
        public struct Data
        {

            public List<inData> response { get; set; }
            public bool success { get; set; }
        }
        public struct inData
        {
            public int b { get; set; }
            public string m { get; set; }
            public double p { get; set; }

            public string a { get; set; }
        }
        public TSF(TextBox Log, int id)
        {
            ID = id;
        }

        public void INI()
        {
            var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
            driverService.HideCommandPromptWindow = true;                    //консоли
            driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl("https://ru.tradeskinsfast.com/");
            MessageBox.Show("Введите все данные , после этого программа продолжит работу!");

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            

            driver.Manage().Window.Position = new Point(5000, 5000);
            driver.Navigate().GoToUrl("https://steamcommunity.com/id/me/tradeoffers/privacy#trade_offer_access_url");
            IWebElement offer = driver.FindElement(By.Id("trade_offer_access_url"));

            if (offer.GetAttribute("value") != Properties.Settings.Default.CsTSF)
            {
                MessageBox.Show("Выберите аккаунт к которому привязана программа!");
                driver.Quit();
                return;
            }
            driver.Navigate().GoToUrl("https://ru.tradeskinsfast.com/");
            driver.Manage().Window.Position = new Point(0, 0);
            var _cookies = driver.Manage().Cookies.AllCookies;
            foreach (var item in _cookies)
            {
                handler.CookieContainer.Add(new System.Net.Cookie(item.Name, item.Value) { Domain = item.Domain });
            }
            for (int i = 0; i < 1; i++)
            {
                new System.Threading.Thread(delegate () {
                    Get(handler, i);
                }).Start();
                //Thread.Sleep(500);
            }
            start();
        }

        public void start()
        {
            var firstFull = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
            while (true)
            {
                while (Program.pauseTSF == true)
                {
                    Thread.Sleep(200);
                }
                var res = ClickItem(ITEMS);
                if (res == true)
                {
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
                        if (s[0].ToString() == Properties.Settings.Default.name && s[1].ToString() == Properties.Settings.Default.CsTSF)
                        {
                            if (s[3].ToString() != Properties.Settings.Default.CsTSFVersion)
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
                   // RefreshBotInventory();
                    Program.MessTSF.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "{Правило 200сек.}Обновил инвентари!");
                }
                Thread.Sleep(Program.sleepMCsTrade);
            }
        }
        private void Get(HttpClientHandler handler, int id)
        {
            HttpClientHandler handler1 = handler;
            HttpClient client = null;

            client = new HttpClient(handler1);
            client.Timeout = TimeSpan.FromSeconds(60);
            client.DefaultRequestHeaders.Add("User-Agent",
 "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
            client.DefaultRequestHeaders.Add("Origin", "https://ru.tradeskinsfast.com");
            client.DefaultRequestHeaders.Add("Referer", "https://ru.tradeskinsfast.com/");
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
     //       client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            while (true)
            {
                try
                {
                    var stringContent = new StringContent("", Encoding.UTF8, "application/json");
                //    stringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue() "application/json";
                    var response = client.PostAsync("https://ru.tradeskinsfast.com/ajax/botsinventory", stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        ITEMS = JsonConvert.DeserializeObject<dynamic>(responseString);

                        Program.MessTSF.Enqueue("БОТ[" + id + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов:" + ITEMS.response.Count);
                    }

                    Random random = new Random();

                    //  Thread.Sleep(random.Next(50, 700));
                }
                catch (Exception ex) { }
            }
            // return new Data();
        }
        private bool ClickItem(dynamic Items)
        {
            try
            {
                if (Items.response != null)
                {
                    foreach (var item in Items.response)
                    {
                        foreach (var name in Program.DataTSF)
                        {

                            if (item.m.Value.Replace(" ", "") == (name.Name).Replace(" ", "") && item.v.Value <= name.Price)
                            {
                                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                                string query = "var xhr = new XMLHttpRequest();var body = \"id="+ item.b.Value + "&u=&b="+ item.a.Value + "\"; xhr.open(\"POST\", 'https://ru.tradeskinsfast.com/ajax/trade', true); xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8'); xhr.setRequestHeader('x-requested-with', 'XMLHttpRequest'); xhr.setRequestHeader('accept', '*/*'); xhr.send(body); ";
                                string title = (string)js.ExecuteScript(query);
                                Program.MessTSF.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет :" + item.m.Value + "|Цена_Сайта:" + item.v.Value + "|Цена_Наша:" + name.Price);
                                Thread.Sleep(Program.sleepITSF);
                                //string trade = (string)js.ExecuteScript("document.getElementById(\"main-trade-button\").click();");
                                //Program.MessCsTrade.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нажал ОБМЕН");
                                //Thread.Sleep(5000);
                                //driver.Navigate().Refresh();
                                //Program.MessCsTrade.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Обновил страницу.");
                                return true;//main-trade-button
                            }
                        }

                    }
                }
            }
            catch (Exception ex) { }
            return false;
        }
        private void RefreshBotInventory()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            Program.MessTSF.Enqueue("Запрос[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Обновил инвентарь бота");
            //   string title = (string)js.ExecuteScript("document.getElementById('UpdateBotInv').click();");
        }
    }
}
