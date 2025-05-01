using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Windows;

namespace kursach.Model
{
    internal class TaskDB
    {
        DbConnection connection;

        private TaskDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Task tasks)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Tasks` Values (0, @title, @description, @term, @assigne, @status);select LAST_INSERT_ID();");

                // путем добавления значений в запрос через параметры мы используем экранирование опасных символов
                cmd.Parameters.Add(new MySqlParameter("title", tasks.Title));
                cmd.Parameters.Add(new MySqlParameter("description", tasks.Description));
                cmd.Parameters.Add(new MySqlParameter("term", tasks.Term));
                cmd.Parameters.Add(new MySqlParameter("assigne", tasks.Assigne));
                cmd.Parameters.Add(new MySqlParameter("status", tasks.Status));
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
                        tasks.ID = id;
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

        internal List<Task> SelectAll()
        {
            List<Task> tasks = new List<Task>();
            if (connection == null)
                return tasks;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `Title`, `Description`, `Term`, `Assigne`, `Status` from `Tasks` ");
                try
                {
                    // выполнение запроса, который возвращает результат-таблицу
                    MySqlDataReader dr = command.ExecuteReader();
                    // в цикле читаем построчно всю таблицу
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string title = string.Empty;
                        string description = string.Empty;
                        DateOnly term = DateOnly.FromDateTime(DateTime.Now);
                        string assigne = string.Empty;
                        string status = string.Empty;
                        // проверка на то, что столбец имеет значение
                        if (!dr.IsDBNull(1))
                            title = dr.GetString("Title"); 
                        if (!dr.IsDBNull(2))
                            description = dr.GetString("Description");
                        if (!dr.IsDBNull(3))
                            term = dr.GetDateOnly("Term");
                        if (!dr.IsDBNull(4))
                            assigne = dr.GetString("Assigne");
                        if (!dr.IsDBNull(4))
                            status = dr.GetString("Status");

                        tasks.Add(new Task
                        {
                            ID = id,
                            Title = title,
                            Description = description,
                            Term = term,
                            Assigne = assigne,
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
            return tasks;
        }

        internal bool Update(Task edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Tasks` set `Title`=@title, `Description`=@description, `Term`=@term, `Assigne`=@assigne, `Status`=@status where `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("description", edit.Description));
                mc.Parameters.Add(new MySqlParameter("term", edit.Term));
                mc.Parameters.Add(new MySqlParameter("assigne", edit.Assigne));
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


        internal bool Remove(Task remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Tasks` where `id` = {remove.ID}");
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

        static TaskDB db;
        public static TaskDB GetDb()
        {
            if (db == null)
                db = new TaskDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
