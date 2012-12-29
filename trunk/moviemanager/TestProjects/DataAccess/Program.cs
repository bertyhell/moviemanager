using System;
using System.Data.Entity;
using Model;
using Tmc.DataAccess.SqlCe;

namespace DataAccess
{
    class Program
    {
        static void Main()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<TmcContext>());

            foreach (Video Video in DataRetriever.Videos)
            {
                Console.WriteLine(Video.Name);
               
            }
            Console.ReadKey();
        }
    }
}
