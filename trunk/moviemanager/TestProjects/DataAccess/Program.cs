using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.testmodels;

namespace DataAccess
{
    class Program
    {
        static void Main()
        {
            using (var DB = new VideoContext())
            {

                List<SimpleVideo> Vids = new List<SimpleVideo>
                    {
                        
                        new SimpleVideo
                            {
                                Name = "test vid 1",
                                Subs = new List<Sub> {new Sub {Language = "NL"}, new Sub {Language = "EN"}},
                                MainSub = new Sub {Language = "IT1"}
                                
                            },
                        new SimpleVideo
                            {
                                Name = "test vid 2",
                                Subs = new List<Sub> {new Sub {Language = "FR"}, new Sub {Language = "EN"}},
                                MainSub = new Sub {Language = "IT2"}
                            }
                        ,
                        new SimpleVideo
                            {
                                Name = "test vid 3",
                                Subs = new List<Sub> {new Sub {Language = "FR"}, new Sub {Language = "EN"}},
                                MainSub = new Sub {Language = "IT3"}
                            }
                        ,
                        new SimpleVideo
                            {
                                Name = "test vid 4",
                                Subs = new List<Sub> {new Sub {Language = "FR"}, new Sub {Language = "EN"}},
                                MainSub = new Sub {Language = "IT4"}
                            }
                        ,
                        new SimpleVideo
                            {
                                Name = "test vid 5",
                                Subs = new List<Sub> {new Sub {Language = "FR"}, new Sub {Language = "EN"}},
                                MainSub = new Sub {Language = "IT5"}
                            }
                        ,
                        new SimpleVideo
                            {
                                Name = "test vid 6",
                                Subs = new List<Sub> {new Sub {Language = "FR"}, new Sub {Language = "EN"}},
                                MainSub = new Sub {Language = "IT6"}
                            }
                    };

                foreach (SimpleVideo SimpleVideo in Vids)
                {
                    DB.SimpleVideos.Add(SimpleVideo);
                }
                DB.SaveChanges();
                PrintVids(DB);

                Console.WriteLine("num of subs before: " + DB.Subs.Count());

                DB.SimpleVideos.Remove(Vids[0]);
                DB.SaveChanges();
                PrintVids(DB);

                Console.WriteLine("num of subs after: " + DB.Subs.Count());

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }

			
        }

        private static void PrintVids(VideoContext db)
        {

            Console.WriteLine("All vids in the database:");
            var Query = from Sv in db.SimpleVideos
                        orderby Sv.Name
                        select Sv;
            foreach (var Vid in Query)
            {
                Console.WriteLine(Vid.Name);
                Console.WriteLine("\t###" + Vid.MainSub.Language);
                Vid.Subs.ForEach(s => Console.WriteLine("\t" + s.Language));
            }
        }
    }
}
