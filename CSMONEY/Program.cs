using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CSMONEY
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
        
        public static List<Dat> Data = new List<Dat>();
        public static List<Dat> DataLoot = new List<Dat>();
        public static List<Dat> DataCsTrade = new List<Dat>();
        public static List<Dat> DataTSF = new List<Dat>();
        public static Queue<string> Mess = new Queue<string>();
        public static Queue<string> MessTelegram = new Queue<string>();
        public static Queue<string> MessLoot = new Queue<string>();
        public static Queue<string> MessCsTrade = new Queue<string>();
        public static Queue<string> MessTSF = new Queue<string>();
        public static bool pauseMoney = false;
        public static bool pauseLoot = false;
        public static bool pauseCsTrade = false;
        public static bool pauseTSF = false;
        public static int sleepMSecond = 500;
        public static int sleepMSecondLoot = 500;
        public static int sleepMCsTrade = 500;
        public static int sleepTSF = 500;
        public static int sleepIMONEY = 0;
        public static int sleepILoot = 0;
        public static int sleepICsTrade = 0;
        public static int sleepITSF = 0;
        public struct Dat
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Factory { get; set; }
            public double Price { get; set; }
        }

    }
    
}
