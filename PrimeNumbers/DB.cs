using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

[assembly: InternalsVisibleTo("PrimeNumbers.Tests")]



namespace PrimeNumbers
{
    internal class DB
    {
        private const string path = "calculated_primes.db";


        // create database for storing prime numbers
        public static void CreateDatabase()
        {
            using SqliteConnection connection = new("Data Source=" + path);
            connection.Open();

            var SQL_command = connection.CreateCommand();
            SQL_command.CommandText = "CREATE TABLE IF NOT EXISTS Primes (prime INT, UNIQUE(prime));";
            SQL_command.ExecuteNonQuery();

            connection.Close();
        }


        // inserts prime numbers into database
        public static void FillDatabase(List<int> numList)
        {
            using SqliteConnection connection = new("Data Source=" + path);
            connection.Open();

            var Transaction = connection.BeginTransaction();
            var SQL_command = connection.CreateCommand();
            SQL_command.Transaction = Transaction;

            string txt_query;

            foreach (int i in numList)
            {
                txt_query = $"INSERT OR IGNORE INTO Primes (prime) VALUES ({i});";

                SQL_command.CommandText = txt_query;
                SQL_command.ExecuteNonQuery();
            }

            Transaction.Commit();

            connection.Close();
        }


        // wipes all records from database
        public static void ClearDatabase()
        {
            using SqliteConnection connection = new("Data Source=" + path);
            connection.Open();

            var SQL_command = connection.CreateCommand();
            SQL_command.CommandText = "DELETE FROM Primes;";
            SQL_command.ExecuteNonQuery();

            connection.Close();
        }


        // fills external list with prime numbers of given range
        public static void GetFromDatabase(ref List<int> numList, int range_start, int range_end)
        {
            using SqliteConnection connection = new("Data Source=" + path);
            connection.Open();

            var SQL_command = connection.CreateCommand();
            SQL_command.CommandText = $"SELECT * FROM Primes WHERE prime >= {range_start} AND prime <= {range_end}";
            SQL_command.ExecuteNonQuery();

            SqliteDataReader SQL_reader = SQL_command.ExecuteReader();

            while (SQL_reader.Read())
                numList.Add(System.Convert.ToInt32(SQL_reader["prime"]));


            connection.Close();
        }

    }
}
