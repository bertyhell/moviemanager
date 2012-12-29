using System;
using System.Data.Entity;
using Tmc.DataAccess.SqlCe;
using Tmc.SystemFrameworks.Model;

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
            Console.WriteLine("done");
			Console.ReadKey();
			
        }
    }
}
