using System;
using System.Windows;

namespace MonitorulOficialPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            var downloader = new Downloader();
            
            //donwload this pdf
            var link = new Link("/Monitorul-Oficial--PIV--5090--2024.html", "5090");
            downloader.DownloadPDF(link);

            //download all pdfs
            try
            {
                var date = "2024 - 12 - 04";
                var returnedList = downloader.FindPDFs(date).GetAwaiter().GetResult();

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
