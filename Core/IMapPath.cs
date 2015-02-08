using System.Web;

namespace SomeBasicEFApp.Core
{
    public interface IMapPath
    {
        string MapPath(string path);
    }

    public class ConsoleMapPath : IMapPath
    {
        public string MapPath(string path)
        {
            return path;
        }
    }

    public class WebMapPath : IMapPath
    {
        public string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}
