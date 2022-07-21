using System;

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
        static private int SizeFealsRequest = 5000;

        static void Main(string[] args)
        {

            var sql = new MSSQL(StringConnect);
            while (true)
            {
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
                    break;
                }
                finally
                {
                    sql.Disconnect();
                }

                Console.WriteLine($"Перерыв на {TimePause / 60000} минут...");
                System.Threading.Thread.Sleep(TimePause);
            }

        }
    }
}
