using System;
using System.Collections.Generic;
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

			const string DATABASE_PATH = @"C:\MMproject\Other\Database\TheMovieCollector.sdf";
			DataRetriever.Init(string.Format("Data Source = {0}", DATABASE_PATH));

			var videos = DataRetriever.Videos;

			videos.Add(new Video
			{
				Name = "test",
				Files = new List<VideoFile> { new VideoFile { Path = "c:\\" } },
				VideoType = VideoTypeEnum.Video
			});
			DataRetriever.Videos = videos;

			foreach (Video Video in DataRetriever.Videos)
			{
				Console.WriteLine(Video.Name);

			}
			Console.WriteLine("done");
			Console.ReadKey();

		}
	}
}
