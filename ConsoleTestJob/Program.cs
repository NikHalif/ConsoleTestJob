using System;
using System.Diagnostics;

namespace ConsoleTestJob
{
    internal class Program
    {
        public static readonly string StringConnect = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Environment.CurrentDirectory}\\Deals.mdf;Integrated Security=True;MultipleActiveResultSets=True";


        /// <summary>
        /// Время ожидания перед итерацией полного обнолвения
        /// </summary>
        static private int TimePause = 600000; // 10 минут
        /// <summary>
        /// Количетсво строк данных в одном запросе
        /// </summary>
        static private int SizeFealsRequest = 20000;

        static void Main(string[] args)
        {

            var sql = new MSSQL(StringConnect);
            //while (true)
            //{
                Console.WriteLine("Для завершения работы нажмите CRTL + C");
            try
            {
                sql.Connection();
                var allSize = Request.PostDealsCountAsync(SizeFealsRequest, 0).Result;

                double total = allSize.total;
                double size = SizeFealsRequest;
                int iter = Convert.ToInt32(Math.Ceiling(total / size));

                var loandInt = 0; var addint = 0;
                for (int i = 0; i <= iter; i++)
                {
                    var result = Request.PostDealAsync(SizeFealsRequest, i).Result;
                    foreach (var item in result)
                    {
                        var t = sql.IsAddDeal(item);
                        if (t < 0) loandInt++;
                        else addint++;
                    }

                    Console.WriteLine($"Страница {i}/{iter}, загруженно {addint}, в базе {loandInt}. Всего: {addint + loandInt}/{total}");
                }

                Console.WriteLine($"{sql.SelectInt()}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
                Console.ReadKey();
                return;
            }
            finally
            {
                sql.Disconnect();
            }

            Console.WriteLine($"Перерыв на {TimePause / 60000} минут...");
            //System.Threading.Thread.Sleep(TimePause);
            //}

            sql.Connection();

            Deal deal1 = new Deal();

            deal1.dealDate = new DateTime(2022, 7, 21);
            deal1.buyerInn = "553505613176";
            deal1.buyerName = "ИП МАЛЬКОВ АЛЕКСАНДР АЛЕКСАНДРОВИЧ";
            deal1.sellerInn = "5501191203";
            deal1.sellerName = "Общество с ограниченной ответственностью \"ИТЮГАС\"";
            deal1.dealNumber = "0002553505613176005501191203";
            deal1.woodVolumeBuyer = 0;
            deal1.woodVolumeSeller = 0;

            Deal deal2 = new Deal();

            deal2.dealDate = new DateTime(2022, 7, 21);
            deal2.buyerInn = "1234543sd425";
            deal2.buyerName = "sdfdkjaslfdshljdshl";
            deal2.sellerInn = "5501191203";
            deal2.sellerName = "Общество с ограниченной ответственностью \"ИТЮГАС\"";
            deal2.dealNumber = "0002553505613176005501191203";
            deal2.woodVolumeBuyer = 0;
            deal2.woodVolumeSeller = 0;

            Stopwatch v1 = new Stopwatch();
            Stopwatch v2 = new Stopwatch();

            v1.Start();
            sql.IsTestV1(deal1);
            sql.IsTestV2(deal1);
            v1.Stop();

            v2.Start();
            sql.IsTestV1(deal1);
            sql.IsTestV2(deal1);
            v2.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan timeV1 = v1.Elapsed;
            TimeSpan timeV2 = v2.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTimeV1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                timeV1.Hours, timeV1.Minutes, timeV1.Seconds,
                timeV1.Milliseconds / 10);

            string elapsedTimeV2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                timeV2.Hours, timeV2.Minutes, timeV2.Seconds,
                timeV2.Milliseconds / 10);

            Console.WriteLine($"Проверка существующих данных:\nВерсия 1: {timeV1}\nВерсия 2: {timeV2}");

            v1.Restart();
            sql.IsTestV1(deal2);
            sql.IsTestV2(deal2);
            v1.Stop();

            v2.Restart();
            sql.IsTestV1(deal2);
            sql.IsTestV2(deal2);
            v2.Stop();
            // Get the elapsed time as a TimeSpan value.
            timeV1 = v1.Elapsed;
            timeV2 = v2.Elapsed;

            // Format and display the TimeSpan value.
            elapsedTimeV1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                timeV1.Hours, timeV1.Minutes, timeV1.Seconds,
                timeV1.Milliseconds / 10);

            elapsedTimeV2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                timeV2.Hours, timeV2.Minutes, timeV2.Seconds,
                timeV2.Milliseconds / 10);
            Console.WriteLine($"Проверка несуществующих данных:\nВерсия 1: {timeV1}\nВерсия 2: {timeV2}");


            Console.ReadKey();
        }
    }
}
