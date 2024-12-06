using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MonitorulOficialPDF
{
    public class Downloader
    {
        public async void DownloadPDF(Link link)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9");
                client.DefaultRequestHeaders.Add("cache-control", "no-cache");
                client.DefaultRequestHeaders.Add("pragma", "no-cache");
                client.DefaultRequestHeaders.Add("priority", "u=0, i");
                client.DefaultRequestHeaders.Referrer = new Uri("https://monitoruloficial.ro/e-monitor/");
                client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\"");
                client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                client.DefaultRequestHeaders.Add("sec-fetch-dest", "document");
                client.DefaultRequestHeaders.Add("sec-fetch-mode", "navigate");
                client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
                client.DefaultRequestHeaders.Add("sec-fetch-user", "?1");
                client.DefaultRequestHeaders.Add("upgrade-insecure-requests", "1");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");

                // Specify the URL
                //string url = "https://monitoruloficial.ro/Monitorul-Oficial--PI--1187--2024.html";

                // Send the request
                HttpResponseMessage response = await client.GetAsync("https://monitoruloficial.ro" + link.URL);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the content as a byte array
                byte[] content = await response.Content.ReadAsByteArrayAsync();

                // Specify the output file
                //outputName="output.pdf";
                string outputFilePath = link.Name + ".pdf";

                // Write the content to the file
                File.WriteAllBytes(outputFilePath, content);

                Console.WriteLine("File downloaded successfully." + link.Name + ".pdf");
            }
        }

        public async Task<List<Link>> FindPDFs(string date)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the base address for the client (optional)
                client.BaseAddress = new Uri("https://monitoruloficial.ro");

                // Set the headers
                client.DefaultRequestHeaders.Add("accept", "*/*");
                client.DefaultRequestHeaders.Add("accept-language", "ro-RO,ro;q=0.9,en-US;q=0.8,en;q=0.7,it;q=0.6");
                client.DefaultRequestHeaders.Add("origin", "https://monitoruloficial.ro");
                client.DefaultRequestHeaders.Add("priority", "u=1, i");
                client.DefaultRequestHeaders.Add("referer", "https://monitoruloficial.ro/e-monitor/");
                client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\"");
                client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
                client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
                client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");
                client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");

                // Prepare the POST data
                var content = new StringContent(@"today=" + date, Encoding.UTF8, "application/x-www-form-urlencoded");

                // Make the POST request
                HttpResponseMessage response = await client.PostAsync("ramo_customs/emonitor/get_mo.php", content);

                // Read and display the response
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseContent);

                var returnedList = ExtractLinks(responseContent);

                return returnedList;
            }
        }

        private List<Link> ExtractLinks(string html)
        {
            var returnedList = new List<Link>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Extract the title
            //var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//title");
            //Console.WriteLine("Title: " + titleNode.InnerText);

            // Extract the href attribute of the anchor
            var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");
            foreach (var item in anchorNodes)
            {
                var hrefValue = item.GetAttributeValue("href", string.Empty);

                var link = new Link();
                link.Name = item.InnerText;
                link.URL = hrefValue;

                Console.WriteLine("Anchor HREF: " + link.Name + link.URL);
                returnedList.Add(link);
            }

            return returnedList;  
        }
    }
}
