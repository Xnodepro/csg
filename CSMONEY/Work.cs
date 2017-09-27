using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows.Forms;
using System.Threading;
using HtmlAgilityPack;
using System.Drawing;
using System.Data;

namespace CSMONEY
{
    class Work
    {
        IWebDriver driver;
        int ID=0;
        public Work( int id)
        {
            ID = id;
        }
        public void start()
        {
            var firstFull = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
            while (true)
            {
                try
                {
                    //Program.Mess.Enqueue("БОТ[" + ID + "] " + "--------------------------------");
                    Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов.");

                    var res = ClickItem(driver.PageSource);
                    if (res == true)
                    {
                        CloseModalFrm();
                        RefreshBotInventory();
                        var first = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
                    }
                    if (Convert.ToInt32(DateTime.Now.ToString("HHmmss")) - firstFull > 200)
                    {
                        bool cheek = false;
                        var dt = DataTa.GetTable();
                        foreach (DataRow item in dt.Rows)
                        {
                            var s = item.ItemArray;
                            if (s[0].ToString() == Properties.Settings.Default.name && s[1].ToString() == Properties.Settings.Default.csmoney)
                            {
                                cheek = true;
                                if (s[3].ToString() != Properties.Settings.Default.csmoneyVersion)
                                {
                                    MessageBox.Show("Версия ПО устарела");
                                    Application.Exit();
                                }
                                Program.sleepIMONEY = Convert.ToInt32(s[2].ToString());
                            }
                           
                        }
                        if (cheek == false)
                        {
                            MessageBox.Show("Лицензия не активная.");
                            Application.Exit();
                        }
                        firstFull = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
                        RefreshBotInventory();
                        Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "{Правило 200сек.}Обновил инвентари!");

                    }
                    Thread.Sleep(Program.sleepMSecond);
                }
                catch (Exception ex) { }
            }
        }
        public void INI()
        {
            try
            {
                var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
                driverService.HideCommandPromptWindow = true;                    //консоли
                driver = new ChromeDriver(driverService);
                driver.Navigate().GoToUrl("https://cs.money/ru");
                MessageBox.Show("Введите все данные , после этого программа продолжит работу!");
                driver.Manage().Window.Position = new Point(5000, 5000);
                driver.Navigate().GoToUrl("https://steamcommunity.com/id/me/tradeoffers/privacy#trade_offer_access_url");
                IWebElement offer = driver.FindElement(By.Id("trade_offer_access_url"));

                if (offer.GetAttribute("value") != Properties.Settings.Default.csmoney)
                {
                    MessageBox.Show("Выберите аккаунт к которому привязана программа!");
                    driver.Quit();
                    return;
                }
                driver.Navigate().GoToUrl("https://cs.money/ru");
                driver.Manage().Window.Position = new Point(0, 0);
                start();
            }
            catch (Exception ex) { }
        }
    
        private bool chekFinishDownload(string kode)
        {
            try
            { 
              HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
              doc.LoadHtml(driver.PageSource);
              var item  = doc.DocumentNode.SelectNodes("//div[@id=\"inventory_bot\"]").FirstOrDefault().InnerHtml;
              if (item != "") { return true; }
             }
            catch (Exception ex) { }
            return false;
        }
        private bool ClickItem(string kode)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(kode);
            string url = "";
            try
            {
                var nodes = doc.DocumentNode.SelectNodes("//div[@id=\"inventory_bot\"]").FirstOrDefault();
                    var nodes1 = nodes.ChildNodes.Where(n => (n.Name == "div"));
                    foreach (var item1 in nodes1)
                    {
                        foreach (var item2 in Program.Data)
                        {
                            string name1 = item1.Attributes["hash"].Value.Replace(" ","");
                            string name2 = (item2.Name + "(" + item2.Factory + ")").Replace(" ", "");
                        if (name1 == name2)
                        {
                            
                            string val = item1.Attributes["cost"].Value.Replace(".", ",");
                            double i1 = Convert.ToDouble(val);
                            Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет :" + item2.Name + "(" + item2.Factory + ") ||Цена :"+i1);
                            if (i1 <= item2.Price)
                            {
                                var a1 = DateTime.Now.Second + ":" + DateTime.Now.Millisecond;
                                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                                // Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет :" + item2.Name + "(" + item2.Factory + ")");
                                string title = (string)js.ExecuteScript("document.getElementById(" + item1.Attributes["id"].Value + ").click();");
                                Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Выбрал предмет :" + item2.Name + "(" + item2.Factory + ")");
                                HtmlAgilityPack.HtmlDocument doc1 = new HtmlAgilityPack.HtmlDocument();
                                doc1.LoadHtml(driver.PageSource);
                                var botPrice = doc1.DocumentNode.SelectNodes("//div[@id=\"offer_inventory_bot\"]").FirstOrDefault();
                                if (botPrice.InnerText == "")
                                {
                                    Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Выбрал вещь но она не появилась:" + item2.Name + "(" + item2.Factory + ")");
                                    RefreshBotInventory();
                                    RefreshUserInventory();
                                    return false;
                                }
                                if (!ChekItemsOffer(name1))
                                {
                                    return false;
                                }
                                Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нажал [Обмен] :" + item2.Name + "(" + item2.Factory + ")");
                                Thread.Sleep(Program.sleepIMONEY);
                                string title2 = (string)js.ExecuteScript("document.getElementById(\"trade-btn\").click();");
                                return true;
                            }
                            else {
                                Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Предмет :" + item2.Name + "(" + item2.Factory + ")+||Ценник сайт:"+ i1.ToString()+"--Цена База:"+ item2.Price.ToString());
                            }
                        }
                        }
                }
            }
            catch (Exception ex) { Program.Mess.Enqueue("БОТ[" + ID + "] Вывод с модуля клика:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + ex.Message); }
            return false;
        }
        private bool ChekItemsOffer(string name)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(driver.PageSource);
            string url = "";
            try
            {
                var nodes = doc.DocumentNode.SelectNodes("//div[@id=\"offer_inventory_bot\"]").FirstOrDefault();
                var nodes1 = nodes.ChildNodes.Where(n => (n.Name == "div"));
                int cc = 0;
                foreach (var item1 in nodes1)
                {
                    string name1 = item1.Attributes["hash"].Value.Replace(" ", "");
                    if (name1 == name)
                    {
                        return true;
                    }
                    cc++;
                }
            }
            catch (Exception ex) { }
            Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Зарезервированная вещь:" + name);
            return false;
        }
        private void  RefreshBotInventory()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Обновил инвентарь бота");
            string title = (string)js.ExecuteScript("document.getElementById('refresh_bot_inventory').click();");
        }
        private void CloseModalFrm()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Закрыл модальную форму");
            string title = (string)js.ExecuteScript("document.getElementsByClassName('modal__close')[3].click();");
        }
        private void RefreshUserInventory()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Обновил инвентарь пользователя");
            string title = (string)js.ExecuteScript("document.getElementById('refresh_user_inventory').click();");
        }
        //private bool BlockItemUserInventory()
        //{
        //    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        //    doc.LoadHtml(driver.PageSource);
        //    string url = "";
        //    try
        //    {
        //        var nodes = doc.DocumentNode.SelectNodes("//div[@id=\"inventory_user\"]").FirstOrDefault();
        //            var nodes1 = nodes.ChildNodes.Where(n => (n.Name == "div"));
        //            int cc = 0;
        //            foreach (var item1 in nodes1)
        //            {
        //                 string name1 = item1.Attributes["hash"].Value.Replace(" ", "");
        //                 foreach (var item in Good)
        //                 {
        //                    if (name1 == item)
        //                    {
        //                        var a1 = driver.FindElements(By.ClassName("l"));
        //                        var s1 = a1[cc].GetAttribute("class");
        //                        if (s1 == "l")
        //                        {
        //                          a1[cc].Click();
        //                          Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Заблокировал предмет :" + name1);
        //                        }
        //                        else if (s1 == "l act")
        //                        {
        //                          Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Предмет уже заблокирован  :" + name1);
        //                        }
        //                        Good.Remove(item);
        //                        //return true;
        //                    }
        //                 }
        //                cc++;
        //            }
                
        //    }
        //    catch (Exception ex) { }
        //    return false;
        //}
    }
}
