using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Windows;

namespace kursach.Model
{
    internal class EventDB
    {
        DbConnection connection;

        private EventDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Event events)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Events` Values (0, @title, @date, @place, @budget, @status);select LAST_INSERT_ID();");

                // путем добавления значений в запрос через параметры мы используем экранирование опасных символов
                cmd.Parameters.Add(new MySqlParameter("title", events.Title));
                cmd.Parameters.Add(new MySqlParameter("date", events.Date));
                cmd.Parameters.Add(new MySqlParameter("place", events.Place));
                cmd.Parameters.Add(new MySqlParameter("budget", events.Budget));
                cmd.Parameters.Add(new MySqlParameter("status", events.Status));
                // можно указать параметр через отдельную переменную
                try
                {
                    // выполняем запрос через ExecuteScalar, получаем id вставленной записи
                    // если нам не нужен id, то в запросе убираем часть select LAST_INSERT_ID(); и выполняем команду через ExecuteNonQuery
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        // назначаем полученный id обратно в объект для дальнейшей работы
                        events.ID = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Event> SelectAll()
        {
            List<Event> events = new List<Event>();
            if (connection == null)
                return events;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `Title`, `Date`, `Place`, `Budget`, `Status` from `Events` ");
                try
                {
                    // выполнение запроса, который возвращает результат-таблицу
                    MySqlDataReader dr = command.ExecuteReader();
                    // в цикле читаем построчно всю таблицу
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string title = string.Empty;
                        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
                        string place = string.Empty;
                        int budget = 0;
                        string status = string.Empty;
                        // проверка на то, что столбец имеет значение
                        if (!dr.IsDBNull(1))
                            title = dr.GetString("Title");
                        if (!dr.IsDBNull(2))
                            date = dr.GetDateOnly("Date");
                        if (!dr.IsDBNull(3))
                            place = dr.GetString("Place");
                        if (!dr.IsDBNull(4))
                            budget = dr.GetInt32("Budget");
                        if (!dr.IsDBNull(4))
                            status = dr.GetString("Status");

                        events.Add(new Event
                        {
                            ID = id,
                            Title = title,
                            Date = date,
                            Place = place,
                            Budget = budget,
                            Status = status,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return events;
        }

        internal bool Update(Event edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Events` set `Title`=@title, `Date`=@date, `Place`=@place, `Budget`=@budget, `Status`=@status where `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("date", edit.Date));
                mc.Parameters.Add(new MySqlParameter("place", edit.Place));
                mc.Parameters.Add(new MySqlParameter("budget", edit.Budget));
                mc.Parameters.Add(new MySqlParameter("status", edit.Status));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Client remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Events` where `id` = {remove.ID}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static EventDB db;
        public static EventDB GetDb()
        {
            if (db == null)
                db = new EventDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
