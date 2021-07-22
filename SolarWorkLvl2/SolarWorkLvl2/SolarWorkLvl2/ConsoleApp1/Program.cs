using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace ConsoleApp2
{

    public class Friends
    {
        public static string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\asus\Desktop\Rabotay padla\ConsoleApp1\bin\Debug\ConnectPLS.mdb";
        public OleDbConnection connect;
        public OleDbDataAdapter adapter;
        public OleDbCommand command;
        public DataTable datatable;
        public Friends()
        {
            connect = new OleDbConnection(strConnect);
            datatable = new DataTable();
        }
        public DataTable GetData()
        {
            connect.Open();
            datatable.Clear();
            adapter = new OleDbDataAdapter("SELECT * FROM Table1", connect);
            adapter.Fill(datatable);
            connect.Close();
            return datatable;
        }
        public DataTable GetDataLast()
        {
            connect.Open();
            datatable.Clear();
            adapter = new OleDbDataAdapter($"SELECT * FROM Table1 WHERE month(Date)>= {DateTime.Today.Month} ORDER BY month(Date)*100+day(Date) ASC", connect);
            adapter.Fill(datatable);
            connect.Close();
            return datatable;
        }
        public void delete(int id)
        {
            connect.Open();
            command = new OleDbCommand($"DELETE FROM Table1 WHERE id = {id}", connect);
            command.ExecuteNonQuery();
            connect.Close();
        }
        public void add(string name, DateTime date)
        {
            connect.Open();
            command = new OleDbCommand($"INSERT INTO Table1 ([Name],[Date]) VALUES ('{name}', '{date.ToString("dd.MM.yyyy")}') ", connect);
            command.ExecuteNonQuery();
            connect.Close();
        }   

        public void change(int id, string name, DateTime date)
        {
            connect.Open();
            command = new OleDbCommand($"UPDATE Table1 SET [Name] = '{name}', [Date] = '{date.ToString("dd.MM.yyyy")}' WHERE id = {id}", connect);
            command.ExecuteNonQuery();
            connect.Close();

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Friends friends = new Friends();
            int choise=1;
            DataTable data;
            DataView view;
            data = friends.GetData();
            view = data.DefaultView;
            Console.WriteLine("Ближайшие дни рождения: ");
            Console.WriteLine("id Имя                        Дата рождения");
            for (int i = 0; i < view.Count; i++)
                {
                    if (view[i] == null)
                        break;
                    if( Math.Abs(DateTime.Now.DayOfYear - ((DateTime)view[i][2]).DayOfYear) < 30)
                        Console.WriteLine(view[i][0].ToString() + "  " + view[i][1].ToString() + "        " + ((DateTime)view[i][2]).ToShortDateString());
                }
            Console.WriteLine();
            do
            {
                Console.WriteLine("___________________________________________________________");
                Console.WriteLine("1 Вывод списка дней рождения");
                Console.WriteLine("2 Добавление в список");
                Console.WriteLine("3 Удаление из списка");
                Console.WriteLine("4 Изменение записи");
                Console.WriteLine("0 Выход");
                choise = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("___________________________________________________________");
                if (choise == 1)
                {
                    data = friends.GetData();
                    Console.WriteLine("id Имя                        Дата рождения");
                    foreach (DataRow item in data.Rows)
                    {
                        Console.WriteLine(item[0].ToString() + "  " + item[1].ToString() + "        " + ((DateTime)item[2]).ToShortDateString());
                    }
                    Console.WriteLine("___________________________________________________________");
                }
                if (choise == 2)
                {
                    DateTime date;
                    int check = 0, year, month, day;
                    Console.WriteLine("Введите ФИО ");
                    string name = Console.ReadLine();
                    do
                    {
                        check = 0;
                        Console.WriteLine("Введите дату рождения ");
                        string row = Console.ReadLine();
                        day = Convert.ToInt32(row.Substring(0, 2));
                        month = Convert.ToInt32(row.Substring(3, 2));
                        year = Convert.ToInt32(row.Substring(6));
                        if ((day < 1) || (day > 31)) check++;
                        if ((month % 2 == 0) && (day == 31)) check++;
                        if (month == 2)
                        {
                            if (year % 4 == 0)
                                if (day > 29) check++;
                                else if (day > 28) check++;
                        }
                        if (check > 0) Console.WriteLine("Неверный ввод даты, повторите попытку");
                    }
                    while (check > 0);
                    date = new DateTime(year, month, day);
                    friends.add(name, date);
                    Console.WriteLine("___________________________________________________________");
                }
                if (choise == 3)
                {
                    data = friends.GetData();
                    Console.WriteLine("id Имя                        Дата рождения");
                    foreach (DataRow item in data.Rows)
                    {
                        Console.WriteLine(item[0].ToString() + "  " + item[1].ToString() + "        " + ((DateTime)item[2]).ToShortDateString());
                    }
                    Console.WriteLine("Введите id для удаления");
                    int id;
                    id = Convert.ToInt32(Console.ReadLine());
                    friends.delete(id);
                    Console.WriteLine("___________________________________________________________");
                }
                if (choise == 4)
                {
                    data = friends.GetData();
                    Console.WriteLine("id Имя                        Дата рождения");
                    foreach (DataRow item in data.Rows)
                    {
                        Console.WriteLine(item[0].ToString() + "  " + item[1].ToString() + "        " + ((DateTime)item[2]).ToShortDateString());
                    }
                    Console.WriteLine("Введите id изменяемого человека");
                    int id;
                    DataRow row = null;
                    do
                    {
                        id = Convert.ToInt32(Console.ReadLine());
                    } while (id < 1);
                    foreach (DataRow item in data.Rows)
                    {
                        if ((int)item[0] == id)
                        {
                            row = item;
                            break;
                        }
                    }
                    if (row != null)
                    {
                        DateTime date;
                        int day, month, year, check =0;
                        Console.WriteLine(row[0].ToString() + "  " + row[1].ToString() + "        " + ((DateTime)row[2]).ToShortDateString());
                        Console.WriteLine("Введите ФИО");
                        string name = Console.ReadLine();
                        Console.WriteLine("Введите дату рождения");
                        do
                        {
                            check = 0;
                            string dates = Console.ReadLine();
                            day = Convert.ToInt32(dates.Substring(0, 2));
                            month = Convert.ToInt32(dates.Substring(3, 2));
                            year = Convert.ToInt32(dates.Substring(6));
                            if ((day < 1) || (day > 31)) check++;
                            if ((month % 2 == 0) && (day == 31)) check++;
                            if (month == 2)
                            {
                                if (year % 4 == 0)
                                    if (day > 29) check++;
                                    else if (day > 28) check++;
                            }
                            if (check > 0) Console.WriteLine("Неверный ввод даты, повторите попытку");
                        } while (check > 0);
                        date = new DateTime(year, month, day);
                        friends.change(id, name, date);
                        Console.WriteLine("___________________________________________________________");
                    }
                    else
                    {
                        Console.WriteLine("Не найдено");
                        Console.WriteLine("___________________________________________________________");
                    }

                }
            }
            while (choise != 0);
        }
    }
}
