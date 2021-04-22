using System;
using System.Data.SqlClient;

namespace _9._Increase_Age_Stored_Procedure
{
    class Program
    {
        static void Main(string[] args)
        {
            var id = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            var connectionString = new SqlConnection(@"Server=MSI;Database=MinionsDB;Integrated Security=true");

            connectionString.Open();

            using (connectionString)
            {
                var useProcCmd = new SqlCommand($"EXEC usp_GetOlder {id}", connectionString);
                var reader = useProcCmd.ExecuteReader();
                reader.Read();
                Console.WriteLine(reader[0] + " - " + reader[1] + " years old");
            }
        }
    }
}
