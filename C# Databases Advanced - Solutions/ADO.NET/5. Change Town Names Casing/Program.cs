using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _5._Change_Town_Names_Casing
{
    class Program
    {
        static void Main(string[] args)
        {
            var country = Console.ReadLine();
            var connectionString = new SqlConnection(@"Server=MSI;Database=MinionsDB;Integrated Security=true");

            connectionString.Open();

            using (connectionString)
            {
                var getCountryId = new SqlCommand($@"UPDATE Towns
                                                    SET Name = UPPER(Name)
                                                    WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = '{country}')", connectionString);
                var affectedRowCount = getCountryId.ExecuteNonQuery();

                if (affectedRowCount == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    Console.WriteLine($"{affectedRowCount} town names were affected.");
                    var getTownName = new SqlCommand($@"SELECT t.Name 
                                             FROM Towns as t
                                             JOIN Countries AS c ON c.Id = t.CountryCode
                                             WHERE c.Name = '{country}'", connectionString);
                    var reader = getTownName.ExecuteReader();
                    var towns = new List<string>();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            towns.Add(reader[0].ToString());
                        }
                    }

                    Console.WriteLine("[" + string.Join(", ", towns) + "]");
                }
            }
        }
    }
}
