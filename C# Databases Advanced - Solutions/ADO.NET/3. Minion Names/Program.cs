using System;
using System.Data.SqlClient;

namespace _3._Minion_Names
{
    class Program
    {
        static void Main(string[] args)
        {
            var id = Console.ReadLine();
            var connectionString = new SqlConnection(@"Server=MSI;Database=MinionsDB;Integrated Security=true");

            connectionString.Open();

            using (connectionString)
            {
                var cmd = new SqlCommand($@"SELECT Name FROM Villains WHERE Id = {id}", connectionString);

                var villianName = cmd.ExecuteScalar() ?? $"No villain with ID {id} exists in the database.";

                Console.WriteLine("Villian:" + villianName);
                cmd = new SqlCommand($@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = {id}
                                ORDER BY m.Name", connectionString);

                var reader = cmd.ExecuteReader();

                using (reader)
                {
                    if (!reader.Read())
                    {
                        Console.WriteLine("(no minions)");
                    }
                    else
                    {
                        Console.WriteLine(reader[1] + " " + reader[2]);
                    }

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[1] + " " + reader[2]);
                    }
                }

            }
        }
    }
}