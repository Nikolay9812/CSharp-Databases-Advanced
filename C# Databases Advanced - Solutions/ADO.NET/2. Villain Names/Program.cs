using System;
using System.Data.SqlClient;

namespace _2._Villain_Names
{
    class Program
    {
        const string connectionString = "Server=MSI;Database=MinionsDB;Integrated Security=true";

        static void Main(string[] args)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var selectQuery = 
                    @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                        FROM Villains AS v
                        JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                        GROUP BY v.Id, v.Name
                        HAVING COUNT(mv.VillainId) > 3
                        ORDER BY COUNT(mv.VillainId)";

                using (var command = new SqlCommand(selectQuery,connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = reader[0];
                            var count = reader[1];

                            Console.WriteLine($"{name} - {count}");
                        }
                    }
                }
            }
        }
    }
}
