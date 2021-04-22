using System;
using System.Data.SqlClient;
using System.Linq;

namespace _8._Increase_Minion_Age
{
    class Program
    {
        static void Main(string[] args)
        {
            var minionsIds = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var connectionString = new SqlConnection(@"Server=MSI;Database=MinionsDB;Integrated Security=true");

            connectionString.Open();

            using (connectionString)
            {
                for (int i = 0; i < minionsIds.Length; i++)
                {
                    var minionId = minionsIds[i];

                    var updateMinionCmd = new SqlCommand($@"UPDATE Minions
                                                            SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                                            WHERE Id = {minionId}", connectionString);
                    updateMinionCmd.ExecuteNonQuery();
                }

                var getMinions = new SqlCommand("SELECT Name, Age FROM Minions", connectionString);
                var reader = getMinions.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0] + " " + reader[1]);
                    }
                }
            }
        }
    }
}
