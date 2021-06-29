using System;

namespace Backend_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetBufferSize(Console.BufferWidth, short.MaxValue - 1);

            Console.Write("Введите URL: ");
            string url = Console.ReadLine();

            UrlStatistics urlStatistics = new UrlStatistics();
            urlStatistics.statisticsMethod = new UrlWordsStatisticsMethod();

            bool urlVerified = urlStatistics.VerifyURL(url);

            if (urlVerified)
            {
                bool success = urlStatistics.TryGetStatistics(new Uri(url), out string statistics);
                if (success)
                {
                    Console.WriteLine("Статистика:\n" + statistics);
                }
                else
                {
                    Console.WriteLine("Не удается получить данные с сайта");
                }
            }
            else
            {
                Console.WriteLine("Некорректный URL");
            }

            Console.ReadKey();
        }
    }
}
