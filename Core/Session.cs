using System.IO;
using System;
using System.Data.Common;

namespace SomeBasicEFApp.Core
{
	public class Session
	{
		private readonly IMapPath _mapPath;

		public Session(IMapPath mapPath)
		{
			_mapPath = mapPath;
		}
		private string WebPath()
		{
			var path = Directory.GetParent(_mapPath.MapPath(@"~/")).Parent;
			return Path.Combine(path.FullName, ".db.sqlite");
		}
		
        public CoreDbContext CreateWebSessionFactory()
		{
            return new CoreDbContext(CreateWebConnection());
        }

        public DbConnection CreateWebConnection()
        {
            return CreateConnection(WebPath());
        }
        public DbConnection CreateConnection(string file)
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();
            cnn.ConnectionString = "Data Source=" + Path.Combine(Directory.GetCurrentDirectory(), file);
            cnn.Open();
            return cnn;
        }
		public CoreDbContext CreateSession(string file)
		{
			return new CoreDbContext(CreateConnection(file));
		}
	}
}
