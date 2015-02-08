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
            //var file = WebPath();
            var dm = new CoreDbContext();
            return dm;
            //throw new NotImplementedException();
			//return Fluently.Configure()
			//  .Database(
			//    SQLiteConfiguration.Standard
			//      .UsingFile(file))
			//  .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Customer>())
			//  .BuildSessionFactory();
		}
        public CoreDbContext CreateTestSessionFactory(string file)
        {
            var dm = new CoreDbContext();
			return dm;
			//throw new NotImplementedException();
            //return Fluently.Configure()
            //  .Database(
            //    SQLiteConfiguration.Standard.UsingFile(file))//NOTE:why not use in memory? some queries wont work for nhibernate
            //  .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Customer>())
            //  .ExposeConfiguration(cfg =>
            //      new SchemaExport(cfg).Execute(true, true, false)
            //  )
            //  .BuildSessionFactory();
        }
    }
}
