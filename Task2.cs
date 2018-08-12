using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    class Task2
    {
        public class Foo1
        {          
            private class ReportInfo
            {
                public TimeSpan Elapsed { get; set; }
                public int ClientsCount { get; set; }
                public int ActiveClientsCount { get; set; }
                public int ClientsWithAppropriateEmailCount { get; set; }
            }

            public async static void Run()
            {
                var progress = new Progress<ReportInfo>(report =>
                {
                    Console.WriteLine($"Получено клиентов {report.ClientsCount}");
                    Console.WriteLine($"Активных клиентов {report.ActiveClientsCount}");
                    Console.WriteLine($"Подходящих клиентов {report.ClientsWithAppropriateEmailCount}");
                    Console.WriteLine($"Поиск идет уже {report.Elapsed.ToString("t")}\n");
                });
                var clients = await GetClients(progress);
                Console.WriteLine($"Найдено {clients.Count} клиентов. Активных {clients.Where(e => e.IsActive).Count()}, неактивных {clients.Where(e => !e.IsActive).Count()}");

                var appropriateClients = new List<Client>();
                foreach (var client in clients)
                {
                    if (!IsAppropriateEmail(client))
                    {
                        continue;
                    }

                    if (!client.IsActive)
                    {
                        continue;
                    }

                    appropriateClients.Add(client);
                }

                Console.WriteLine($"Найдено {appropriateClients.Count}");
                Console.ReadLine();
            }

            private static bool IsAppropriateEmail(Client client)
            {
                return client.Email.EndsWith(".ru");
            }          

            private static Task<List<Client>> GetClients(IProgress<ReportInfo> progress)
            {
                return Task.Run(()=> {
                    int activeClientCount = 0;
                    int appropriateClientsCount = 0;
                    var sw = new Stopwatch();
                    TimeSpan elapsed = new TimeSpan(0, 0, 0);
                    sw.Start();
                    //TODO: получать данных из БД
                    var result = new List<Client>();
                    for (int i = 1; i <= 1000000000; i++)
                    {
                        sw.Stop();
                        if (sw.Elapsed - elapsed > new TimeSpan(0,0,3))
                        {
                            elapsed = sw.Elapsed;
                            progress.Report(new ReportInfo()
                            {
                                ActiveClientsCount = activeClientCount,
                                ClientsCount = i,
                                ClientsWithAppropriateEmailCount = appropriateClientsCount,
                                Elapsed = elapsed
                            });
                        }
                        sw.Start();
                        var client = new Client
                        {
                            Name = "клиент " + i,
                            IsActive = i % 2 == 0 || i % 5 == 0
                        };

                        string domain = "yandex.ru";
                        if (i % 3 == 0)
                        {
                            domain = "gmail.com";
                        }
                        else if (i % 5 == 0)
                        {
                            domain = "mail.ru";
                        }
                        else if (i % 9 == 0)
                        {
                            domain = "yandex.ua";
                        }
                        else if (i % 11 == 0)
                        {
                            domain = "yandex.kz";
                        }

                        client.Email = $"client{i}@{domain}";
                        result.Add(client);
                        if (client.IsActive)
                        {
                            activeClientCount++;
                            if (IsAppropriateEmail(client))
                            {
                                appropriateClientsCount++;
                            }
                        }
                    }
                    sw.Stop();
                    return result;
                });        
            }

            public class Client
            {
                public string Name { get; set; }

                public string Email { get; set; }

                public bool IsActive { get; set; }
            }
        }



        static void Main(string[] args)
        {            
            Foo1.Run();
            Console.ReadKey();
        }
    }
}
