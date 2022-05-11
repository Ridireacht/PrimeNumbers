using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;

[assembly: InternalsVisibleTo("PrimeNumbers.Tests")]



namespace PrimeNumbers
{
    internal class DB
    {
        private const string path = "calculated_primes.db";


        public static void CreateDatabase()
        {
            using SqliteConnection connection = new("Data Source=" + path);
            connection.Open();

            var SQL_command = connection.CreateCommand();
            SQL_command.CommandText = "CREATE TABLE IF NOT EXISTS Primes (prime INT, UNIQUE(prime));";
            SQL_command.ExecuteNonQuery();

            connection.Close();
        }

        public static void FillDatabase(List<int> numList)
        {
            // assembling an SQL command that will input all the new calculated primes into DB
            string txt_query = "INSERT OR IGNORE INTO Primes (prime) VALUES ";
            foreach (int i in numList)
                txt_query += $"({i}),";

            txt_query = Regex.Replace(txt_query, ",$", ";");


            using (SqliteConnection connection = new("Data Source=" + path))
            {
                connection.Open();

                var SQL_command = connection.CreateCommand();
                SQL_command.CommandText = txt_query;
                SQL_command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void ClearDatabase()
        {
            using SqliteConnection connection = new("Data Source=" + path);
            connection.Open();

            var SQL_command = connection.CreateCommand();
            SQL_command.CommandText = "DELETE FROM Primes;";
            SQL_command.ExecuteNonQuery();

            connection.Close();
        }

        public static void GetFromDatabase(ref List<int> numList, int range_start, int range_end)
        {
            // getting a bunch of primes within a range of 'a' and 'b'
            using SqliteConnection connection = new("Data Source=" + path);
            connection.Open();

            var SQL_command = connection.CreateCommand();
            SQL_command.CommandText = $"SELECT * FROM Primes WHERE prime >= {range_start} AND prime <= {range_end}";
            SQL_command.ExecuteNonQuery();

            SqliteDataReader SQL_reader = SQL_command.ExecuteReader();

            while (SQL_reader.Read())
                numList.Add(Convert.ToInt32(SQL_reader["prime"]));


            connection.Close();
        }

    }
}
