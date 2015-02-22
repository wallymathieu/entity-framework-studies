using System.IO;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.EntityClient;

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
            var path= Directory.GetParent (_mapPath.MapPath(@"~/")).Parent;
            return Path.Combine(path.FullName ,".db.sqlite" );
        }
        public CoreDbContext CreateWebSessionFactory()
        {
			throw new NotImplementedException();
		}
        public CoreDbContext CreateSession(string file)
        {
            var dm = new CoreDbContext("(LocalDB)\\MSSQLLocalDB;attachdbfilename=|DataDirectory|\\" + file);
			return dm;
        }
    }
}
