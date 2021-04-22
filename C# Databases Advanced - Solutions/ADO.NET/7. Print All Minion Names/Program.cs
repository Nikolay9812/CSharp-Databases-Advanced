using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace _7._Print_All_Minion_Names
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = new SqlConnection(@"Server=MSI;Database=MinionsDB;Integrated Security=true");

            connectionString.Open();

            using (connectionString)
            {
                var getMinionCmd = new SqlCommand("SELECT Name FROM Minions", connectionString);

                var reader = getMinionCmd.ExecuteReader();
                var minions = new List<string>();
                using (reader)
                {
                    while (reader.Read())
                    {
                        minions.Add(reader[0].ToString());
                    }
                }

                Console.WriteLine(string.Join(",", minions));
                while (minions.Any())
                {

                    Console.WriteLine(minions[0]);
                    minions.RemoveAt(0);
                    if (minions.Any())
                    {
                        Console.WriteLine(minions[^1]);
                        minions.RemoveAt(minions.Count - 1);
                    }


                }
            }
        }

    }
}
