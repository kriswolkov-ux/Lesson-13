using System;
using System.Data;
using System.Data.Common;
using Npgsql;

namespace dot.net.Lesson2
{
    public class Result
    {
        public static void RunResult()
        {
            string connString = "Host=wotacoma.ru;Port=5090;Username=students;Password=asd123qwe4ewq321dsa;Database=9/3-RPO-24/1";

            DbProviderFactories.RegisterFactory("Npgsql", NpgsqlFactory.Instance);

            DbProviderFactory factory = DbProviderFactories.GetFactory("Npgsql");

            using DbConnection connection = factory.CreateConnection()
                ?? throw new InvalidOperationException("Не удалось создать соединение");
            connection.ConnectionString = connString;
            string nickname = "Krishleb";
            string lastName = "Stepanov";
            string firstName = "Roman";

            connection.Open();

            using DbCommand cmd1 = connection.CreateCommand();
            cmd1.CommandText = "INSERT INTO students_lesson13 (nickname, first_name, last_name,) VALUES (@nickname, @firstName, @lastName)";

            var param1 = cmd1.CreateParameter();
            param1.ParameterName = "@nickname";
            param1.Value = nickname;
            cmd1.Parameters.Add(param1);

            var param2 = cmd1.CreateParameter();
            param2.ParameterName = "@firstName";
            param2.Value = firstName;
            cmd1.Parameters.Add(param2);

            var param3 = cmd1.CreateParameter();
            param3.ParameterName = "@lastName";
            param3.Value = lastName;
            cmd1.Parameters.Add(param3);

            try
            {
                var rows1 = cmd1.ExecuteNonQuery();
                Console.WriteLine($"Количество затронутых строк (INSERT): {rows1}");
            }
            catch (Exception ex)
            {

            }

            using DbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM students_lesson13 ORDER BY id;";

            using DbDataAdapter adapter = factory.CreateDataAdapter()
                ?? throw new InvalidOperationException("Не удалось создать адаптер");

            adapter.SelectCommand = command;

            DataTable table = new DataTable();
            adapter.Fill(table);

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            Console.WriteLine($"\n Содержимое таблицы students_lesson13 (всего строк: {table.Rows.Count}) ");

            foreach (DataColumn column in table.Columns)
            {
                Console.Write($"{column.ColumnName,-20} | ");
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', table.Columns.Count * 22));

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    string value = row[column] == DBNull.Value ? "NULL" : row[column].ToString();
                    Console.Write($"{value,-20} | ");
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Result.RunResult();
        }
    }
}