using P01_StudentSystem.Data;
using System;

namespace P01_StudentSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var contex = new StudentSystemContext();
            contex.Database.EnsureDeleted();
            contex.Database.EnsureCreated();
        }
    }
}
