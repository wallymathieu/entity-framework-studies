using System;
using System.IO;
using System.Linq;

namespace SomeBasicEFApp.Tests
{
    public class Migrator
    {
        private readonly string _db;
        public Migrator(string db)
        {
            _db = db;
        }
        public void Migrate()
        {
            var migratePath = Directory.GetDirectories(Path.Combine("..", "..", "..", "packages"), "FluentMigrator.*").Last();
            var parameters = "/connection \"Data Source=" + _db + ";Version=3;\" /db sqlite /target DbMigrations.dll";
            ExecuteAndRedirectOutput migrator;
            if (IsWindows(Environment.OSVersion.Platform))
            {
                migrator = new ExecuteAndRedirectOutput(Path.Combine(migratePath, "tools", "Migrate.exe"),
                    parameters);
            }
            else
            {
                migrator = new ExecuteAndRedirectOutput("mono",
                "--runtime=v4.0.30319 " + Path.Combine(migratePath, "tools", "Migrate.exe") + " " + parameters);

            }

            migrator.StartAndWaitForExit();
        }
        private bool IsWindows(PlatformID plattform)
        {
            switch (plattform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return true;
                default:
                    return false;
            }
        }
    }
}
