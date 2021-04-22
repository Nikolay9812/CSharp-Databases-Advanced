using System;
using System.Data.SqlClient;

namespace _6._Remove_Villain
{
    class Program
    {
        static void Main(string[] args)
        {
            var villainId = Console.ReadLine();
            var connectionString = new SqlConnection(@"Server=MSI;Database=MinionsDB;Integrated Security=true");

            connectionString.Open();

            using (connectionString)
            {
                var getVillainCommand = new SqlCommand($"SELECT Name FROM Villains WHERE Id = {villainId}", connectionString);
                var villainName = getVillainCommand.ExecuteScalar();
                if (villainName != null)
                {
                    var deleteFromMinionsVillainsTable = new SqlCommand($"DELETE FROM MinionsVillains WHERE VillainId = {villainId}", connectionString);
                    var deleteVillainCommand = new SqlCommand($@" DELETE FROM Villains WHERE Id = {villainId}", connectionString);
                    var minionsCount = deleteFromMinionsVillainsTable.ExecuteNonQuery();
                    Console.WriteLine($"{villainName} was deleted.");
                    deleteVillainCommand.ExecuteNonQuery();
                    Console.WriteLine($"{minionsCount} minions were released.");
                }
                else
                {
                    Console.WriteLine("No such villain was found.");
                }



            }

        }
    }
}
