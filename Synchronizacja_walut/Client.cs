using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizacja_walut
{
    internal class Client
    {
        public void Update_Currencies() 
        {
            using (var client = new WebClient())  
            {
                string answer="";
                client.Headers.Add("Content-Type:application/json");
                client.Headers.Add("Accept:application/json");
                var result = client.DownloadString("http://localhost:5157/Currency/UpdateCurrencies");
                

                if (result.Contains("Currencies"))
                {
                    Console.WriteLine(result);
                    Console.ReadLine();

                }

                else
                {
                    Console.WriteLine("Wystapił błąd, synchronizacja nie została wykonana \n");
                    Console.WriteLine("Wcisnij T jeżeli chcesz poznać szczegóły błędu");
                    answer = Console.ReadLine();

                }
                if (answer.Equals("T"))

                {
                    Console.WriteLine(result);
                    Console.ReadLine();

                }

            }
        }
    }
}
