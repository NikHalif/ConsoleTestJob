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
        static private int SizeFealsRequest = 20000;

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

                    for (int i = 0; i <= iter; i++)
                    {
                        sql.Bulk(Request.PostDealAsync(SizeFealsRequest, i).Result);

                        Console.WriteLine($"Страница {i}/{iter} загруженна. Всего строк: {total}");
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
