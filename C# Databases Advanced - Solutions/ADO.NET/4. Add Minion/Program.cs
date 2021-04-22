using System;
using System.Linq;
using System.Data.SqlClient;

namespace _4._Add_Minion
{
    class Program
    {
        static void Main(string[] args)
        {
            var minionInfo = Console.ReadLine().Split(" ").ToArray()[1..];
            var villainName = Console.ReadLine().Split(" ").ToArray()[1];
            var minionName = minionInfo[0];
            var minionAge = int.Parse(minionInfo[1]);
            var townName = minionInfo[2];


            var connectionString = new SqlConnection(@"Server=MSI;Database=MinionsDB;Integrated Security=true");

            connectionString.Open();

            using (connectionString)
            {
                var getTownId = new SqlCommand($"SELECT Id FROM Towns WHERE Name = '{townName}'", connectionString);
                var townId = getTownId.ExecuteScalar();

                if (townId == null)
                {
                    var insertTownCmd = new SqlCommand($"INSERT INTO Towns (Name) VALUES ('{townName}')", connectionString);
                    insertTownCmd.ExecuteNonQuery();
                    Console.WriteLine($"Town {townName} was added to the database.");

                }
                townId = (int)getTownId.ExecuteScalar();

                var getVillainId = new SqlCommand($"SELECT Id FROM Villains WHERE Name = '{villainName}'", connectionString);
                var villainId = getVillainId.ExecuteScalar();
                if (villainId == null)
                {
                    var insertVillainCmd = new SqlCommand($"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES ('{villainName}', 4)", connectionString);
                    insertVillainCmd.ExecuteNonQuery();
                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }

                villainId = (int)getVillainId.ExecuteScalar();

                var getMinionId = new SqlCommand($"SELECT Id FROM Minions WHERE Name = '{minionName}'", connectionString);
                var minionId = getMinionId.ExecuteScalar();
                if (minionId == null)
                {
                    var insertMinion = new SqlCommand($"INSERT INTO Minions (Name, Age, TownId) VALUES ('{minionName}', {minionAge}, {townId})", connectionString);
                    insertMinion.ExecuteNonQuery();
                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                }

                minionId = (int)getMinionId.ExecuteScalar();

                var pairMinionAndVillainCmd = new SqlCommand($"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES {villainId}, {minionId})", connectionString);
            }
        }
    }
}
