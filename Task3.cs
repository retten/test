using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    class Task3
    {
        public class Foo2
        {
            private static readonly Random _rnd = new Random();
            private readonly ConcurrentDictionary<long, TimeSpan> _profilingData = new ConcurrentDictionary<long, TimeSpan>();
            private object Locker = new object();
            private long NumberOfTasks = 0;
            private AutoResetEvent JobDoneEvent = new AutoResetEvent(false);

            public void SaveTime(long i, TimeSpan time)
            {
                _profilingData.TryAdd(i, time);
            }

            public void AsyncQuery(long [] queryIds)
            {               
                NumberOfTasks = queryIds.Count();
                foreach(var queryId in queryIds)
                {
                    ThreadPool.QueueUserWorkItem(SendRequest, queryId);                   
                }
                JobDoneEvent.WaitOne();
            }           

            private void SendRequest(object queryId)
            {                
                var sw = new Stopwatch();
                sw.Start();
              
                var time = _rnd.Next(1000, 3000);
                Thread.Sleep(time);

                sw.Stop();
                _profilingData.TryAdd((long)queryId, sw.Elapsed);

                // исключительно для того, чтобы получать актуальную информацию о коллекции                
                lock (Locker)
                {
                    var summTime = _profilingData.Values.Select(e => e.TotalMilliseconds).Sum();
                    Console.WriteLine($"Запрос {(long)queryId} отработан за {sw.ElapsedMilliseconds}. Выполненных запросов {_profilingData.Count} за {summTime}!");
                }
               
                if (Interlocked.Decrement(ref NumberOfTasks) == 0)
                {
                    JobDoneEvent.Set();
                };

            }           

            public static void Run()
            {
                var foo2 = new Foo2();
                long[] queryIds = Enumerable.Range(1, 100).Select(x=> (long)x).ToArray();
                foo2.AsyncQuery(queryIds);               
                
                Console.WriteLine("Все!");
                Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {                 
            Foo2.Run();
        }
    }
}
