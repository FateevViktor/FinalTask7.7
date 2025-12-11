using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace FinalTask7._7
{
    abstract class Book
    {
        protected bool checkСompleted; //Проверка заполнен ли заказ на книгу
        protected string title; // Название книги (журнала)
        protected int price; // Цена р. за книгу (журнал)
        public bool CheckСompleted
        {
            get { return checkСompleted; }
            set { checkСompleted = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        //Виртуальный метод, который можно переопределить
        public virtual int Price
        {
            get { return price; }
            set { price = value; }
        }
    }
    // Класс проверки правильности ввода
    class CheckInput
    {
        static public DateTime CheckDate()
        {
            DateTime d;
            bool check=false;
            do
            {
                if (DateTime.TryParse(Console.ReadLine(), out d) == true)
                {
                    if (d >= DateTime.Now.Date)
                    {
                        Console.WriteLine("Журнал с такой датой еще не вышел, введите дату до текущей");
                    }
                    else
                    {
                        check = true;
                    }
                }
                else
                {
                    Console.WriteLine("Необходимо ввести дату. Введите дату в формате число.месяц.год");
                }
            } while (check == false);
            return d.Date;
        }
    }
    // книга
    class BookDelivery : Book
    {        
        private string author; // Автор книги
        
        //Конструктор без параметров
        public BookDelivery()
        {
            checkСompleted=false; //Обращаемся к переменной напрямую
            author = null;
            Title = null;
            Price = 0;
        }

        //Конструктор с параметрами
        public BookDelivery(string Author, string Title, int Price)
        {
            CheckСompleted = true;
            author = Author;
            this.Title = Title;
            this.Price = Price;
        }

        public string Author
        {
            get { return author;}
            set { author = value;}
        }
        
        //Переопределяем свойство
        public override int Price
        {
            get { return price; }
            set 
            {
                if (value > 0)
                {
                    price = value;
                }
                else
                {
                    price = 0;
                }
            }
        }
    }

    // журнал
    class JournalDelivery : Book
    {
        private DateTime releaseDate;
        
        //Конструктор без параметров
        public JournalDelivery()
        {
            CheckСompleted = false;
            releaseDate = default;
            Title = null;
            Price = 0;
        }
        //Конструктор с параметрами
        public JournalDelivery(DateTime ReleaseDate, string Title, int Price)
        {
            CheckСompleted = true;
            releaseDate = ReleaseDate;
            this.Title = Title;
            this.Price = Price;
        }
        public DateTime ReleaseDate
        {
            get { return releaseDate; }
            set { releaseDate = value; }
        }
    }
    class Order
    {
        OrderNumber<string> number = new OrderNumber<string>();
        private string addressDelivery; // Адрес доставки

        private BookDelivery OB = new BookDelivery();
        private JournalDelivery OJ = new JournalDelivery();
        public BookDelivery OrderBook
        {
            get { return OB;}
            set { OB = value;}
        }
        public JournalDelivery OrderJournal
        {
            get { return OJ;}
            set { OJ = value;}
        }
        public string AddressDelivery
        {
            get { return addressDelivery; }
            set { addressDelivery = value; }
        }
        
        public string OrderNumber
        {
            get { return number.Number; }
            set { number.Number = value; }
        }
        // Метод показа заказа
        public void DisplayOrder()
        {
            Console.WriteLine("Номер заказа {0}", number.Number);
            if(OB.CheckСompleted==true)
            {
                Console.WriteLine("Книга: {0}", OB.Title);
                Console.WriteLine("Автор: {0}", OB.Author);
                Console.WriteLine("Цена: {0}", OB.Price);
            }
            
            if (OJ.CheckСompleted == true)
            {
                Console.WriteLine("Журнал: {0}", OJ.Title);
                Console.WriteLine("Дата выпуска: {0}", OJ.ReleaseDate.ToString("d"));
                Console.WriteLine("Цена: {0}", OJ.Price);
            }
            Console.WriteLine("Стоимость заказа: {0}", OB.Price + OJ.Price);
            Console.WriteLine("Адрес доставки: {0}", addressDelivery);
        }

        //Обобщенный метод
        public void Display<S>(S i)
        {
            Console.WriteLine(i.ToString());
        }
        // ... Другие поля
    }
    //Класс для демонстрации работы с обобщениями
    class OrderNumber<T>
    {
        private T number;

        public T Number
        {
            get { return number; }
            set { number = value; }
        }

        //Конструктор без параметров
        public OrderNumber()
        {
            number = default(T);
        }
        //Конструктор с параметрами
        public OrderNumber(T n)
        {
            number = n; 
        }        
    }

        internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random(); //создаем объект класса работы с рандомом            
            int price;
            string title;
            int numberOrder=rnd.Next(1000, 3000);
            bool trueOrder=false; //Переменная, показывающая, что заказ надо оформить
            DateTime dateTime;
            BookDelivery BD = new BookDelivery();
            Order order = new Order();
            
            // Необходимо занести информацию по книге и журналу, которую пользователь хочет купить
            // 1) Занесем информацию через конструктор без параметров
            Console.WriteLine("В заказе будет книга? (да)");
            if(Console.ReadLine()=="да")
            {
                Console.WriteLine("Введите автора книги");
                BD.Author= Console.ReadLine();
                Console.WriteLine("Введите название книги");
                BD.Title= Console.ReadLine();
                price = rnd.Next(200, 500);
                Console.WriteLine("Цена вашей книги {0} рублей", price);
                BD.Price = price;
                BD.CheckСompleted = true;
                order.OrderBook = BD;
                trueOrder=true;
            }

            // 2) Занесем информацию в конструктор с параметрами
            Console.WriteLine("В заказе будет журнал? (да)");
            if (Console.ReadLine() == "да")
            {
                Console.WriteLine("Введите дату выпуска журнала");
                dateTime = CheckInput.CheckDate();
                Console.WriteLine("Введите название журнала");
                title= Console.ReadLine();
                price = rnd.Next(100, 200);
                Console.WriteLine("Цена вашего журнала {0} рублей", price);
                JournalDelivery JD = new JournalDelivery(dateTime, title, price);
                order.OrderJournal = JD;
                trueOrder = true;
            }
            
            if (trueOrder == true) // Если есть чего заказывать
            {
                Console.WriteLine("Введите адрес доставки:");
                order.AddressDelivery = Console.ReadLine();
                numberOrder = numberOrder+1;
                order.OrderNumber = numberOrder.ToString();

                Console.WriteLine("\n");
                order.DisplayOrder(); // Покажем заказа покупателю

                //Вызов обобщенного метода разными типами

                order.Display<string>("Лотерея среди покупателей будет через неделю, ваш номер:");
                order.Display<int>(20); // Любая цифра
            }
            else
            {
                Console.WriteLine("На следующей неделе будет большой завоз книг и журналов - обязательно заглядывайте.");
            }
        }
    }
}
