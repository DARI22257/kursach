using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlConnector;

namespace kursach.Model
{
    internal class ClientDB
    {
        DbConnection connection;

        private ClientDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Client client)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Clients` Values (0, @fname, @phone);select LAST_INSERT_ID();");

                // путем добавления значений в запрос через параметры мы используем экранирование опасных символов
                cmd.Parameters.Add(new MySqlParameter("fname", client.FullName));
                cmd.Parameters.Add(new MySqlParameter("phone", client.Phone));
                cmd.Parameters.Add(new MySqlParameter("email", client.Email));
                cmd.Parameters.Add(new MySqlParameter("notes", client.Notes));
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
                        client.ID = id;
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

        internal List<Client> SelectAll()
        {
            List<Client> clients = new List<Client>();
            if (connection == null)
                return clients;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `FullName`, `Phone`, `Email`, `Notes` from `Clients` ");
                try
                {
                    // выполнение запроса, который возвращает результат-таблицу
                    MySqlDataReader dr = command.ExecuteReader();
                    // в цикле читаем построчно всю таблицу
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string fname = string.Empty;
                        int phone = dr.GetInt32(0);
                        string email = string.Empty;
                        string notes = string.Empty;
                        // проверка на то, что столбец имеет значение
                        if (!dr.IsDBNull(1))
                            fname = dr.GetString("Fname");
                        string lname = dr.GetString("Lname");
                        clients.Add(new Client
                        {
                            ID = id,
                            FullName = fname,
                            Phone = phone,
                            Email = email,
                            Notes = notes,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return clients;
        }

        internal bool Update(Client edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Clients` set `Fname`=@fname, `Phone`=@phone, `Email`=@email, `Notes`=@notes where `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("fname", edit.FullName));
                mc.Parameters.Add(new MySqlParameter("phone", edit.Phone));
                mc.Parameters.Add(new MySqlParameter("email", edit.Email));
                mc.Parameters.Add(new MySqlParameter("notes", edit.Notes));

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
                var mc = connection.CreateCommand($"delete from `Clients` where `id` = {remove.ID}");
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

        static ClientDB db;
        public static ClientDB GetDb()
        {
            if (db == null)
                db = new ClientDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
