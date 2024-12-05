using System;
using System.Windows;

namespace MonitorulOficialPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            var downloader = new Downloader();
            try
            {
                var returnedList = downloader.FindPDFs().GetAwaiter().GetResult();

                foreach (Link item in returnedList)
                {
                    if (item.URL.Equals("#"))
                        continue;

                    downloader.DownloadPDF(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
