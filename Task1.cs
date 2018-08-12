using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    class Task1
    {
        public enum Sex
        {
            Male,
            Female
        }

        public class Goods
        {
            public string Name { get; set; }

            public int Count { get; set; }

            public decimal Cost { get; set; }

            public decimal TotalCost => Cost * Count;
        }

        public class ShoppingInfo
        {
            public TimeSpan SpendTime { get; set; }

            public decimal SpendMoney
            {
                get { return Goods?.Sum(e => e.TotalCost) ?? 0; }
            }

            public List<Goods> Goods { get; set; }
        }

        public interface IPerson
        {
            TimeSpan GetDressedTime();
            ShoppingInfo GoShopping();
        }       
        public abstract class Person : IPerson
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Sex Sex { get; set; }

            public abstract TimeSpan GetDressedTime();
            public abstract ShoppingInfo GoShopping();

            public Person(string name, int age)
            {
                Name = name;
                Age = age;
            }

            public string SexToString()
            {
                return Sex == Sex.Female ? "женщина" : "мужчина";
            }           
        }
        public class TypicalWoman : Person
        {
            public TypicalWoman(string name, int age) : base(name,age)
            {
                Sex = Sex.Female;
            }

            public override TimeSpan GetDressedTime()
            {
                return new TimeSpan(3, 0, 0);
            }
            public override ShoppingInfo GoShopping()
            {
                return new ShoppingInfo
                {
                    Goods = new List<Goods> {
                            new Goods { Name = "Платье", Count = 2, Cost = 1500 },
                            new Goods { Name = "Колготки", Count = 1, Cost = 1000 },
                            new Goods { Name = "Джинсы", Count = 2, Cost = 3000 },
                            new Goods { Name = "Туфли", Count = 1, Cost = 2000 },
                            new Goods { Name = "Футболка", Count = 5, Cost = 600 },
                        },
                    SpendTime = new TimeSpan(5, 0, 0)
                };
            }
        }
        public class TypicalMale : Person
        {
            public TypicalMale(string name, int age) : base(name, age)
            {
                Sex = Sex.Male;
            }

            public override TimeSpan GetDressedTime()
            {
                return new TimeSpan(0, 15, 0);
            }

            public override ShoppingInfo GoShopping()
            {
                return new ShoppingInfo
                {
                    Goods = new List<Goods> {
                            new Goods { Name = "Джинсы", Count = 1, Cost = 2000 },
                            new Goods { Name = "Футболка", Count = 2, Cost = 1000 },
                            new Goods { Name = "Визитница", Count = 1, Cost = 1000 },
                        },
                    SpendTime = new TimeSpan(0, 40, 0)
                };
            }

            
        }       

        static void Main(string[] args)
        {
            List<Person> persons = new List<Person>();
            persons.Add(new TypicalWoman("Оля", 31));
            persons.Add(new TypicalMale("Игнат", 29));

            foreach (var person in persons)
            {
                Console.WriteLine($"{person.Name}, {person.Age} год, {person.SexToString()}");
                Console.WriteLine($"Среднее время на одевание {person.GetDressedTime()}");

                var shoppingInfo = person.GoShopping();
                Console.WriteLine($"Покупки заняли {shoppingInfo.SpendTime}. Купленные товары:");
                foreach (var goods in shoppingInfo.Goods)
                {
                    Console.WriteLine($"\t{goods.Name} - {goods.Cost}*{goods.Count}={goods.TotalCost}");
                }
                Console.WriteLine($"Итого: { shoppingInfo.SpendMoney}");
            }
            Console.ReadKey();
        }
    }
}