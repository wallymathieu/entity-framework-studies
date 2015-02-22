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
			var str = string.Join(";", new[]{"Data Source=(LocalDB)\\v11.0;",
	  "AttachDbFilename="+Path.Combine(Directory.GetCurrentDirectory() ,file),
	  "Integrated Security=True"
	});
			var dm = new CoreDbContext(str);
			return dm;
        }
    }
}
