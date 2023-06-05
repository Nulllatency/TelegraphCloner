using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegraph.Net;
using Telegraph.Net.Models;

namespace TelegraphCloner
{
    internal class TelegraphManager
    {

        public async Task<int> ClonePage(string source, int count, string name, string author)
        {
           
            var client = new TelegraphClient();
          
            Page page = await client.GetPageAsync(source, true);

            List<string> createdPageURLs = new List<string>();

            Page createdPage = page;

            for (int i = 0; i < count; i++)
            {
                try
                {
                    var account = await client.CreateAccountAsync(author);

                    ITokenClient tokenClient = client.GetTokenClient(account.AccessToken);

                    createdPage = await tokenClient.CreatePageAsync(createdPage.Title, createdPage.Content.ToArray(), createdPage.AuthorName, createdPage.AuthorUrl, true);
                    if (createdPage != null)
                    {
                        Console.WriteLine($"Page created! Number {i + 1}");

                        createdPageURLs.Add(createdPage.Url);
                    }
                    else
                        Console.WriteLine($"Page is NOT created! Number {i + 1}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (ex.Message.Contains("FLOOD_WAIT_") && Convert.ToInt32(ex.Message.Substring(11)) > 30) break;
                    
                    i--;
                    Thread.Sleep((Convert.ToInt32(ex.Message.Substring(11)) + 1) * 1000);
                }

                
            }

            if(createdPageURLs.Count != 0)
            {
                await File.AppendAllLinesAsync(name, createdPageURLs);
                Console.WriteLine($"Writed {createdPageURLs.Count} lines!");
            }
            else
            {
                Console.WriteLine("Nothing writed!");
            }

            count -= createdPageURLs.Count;
            return count;
        }

    }
}
