using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using SomeBasicEFApp.Core;
using SomeBasicEFApp.DbMigrations;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Order = SomeBasicEFApp.Core.Order;
using System.Text;
using System.Linq;

namespace SomeBasicEFApp.Tests
{
    public class CreateDb
    {
        public static void CreateSqlDatabase(string filename)
        {
            string databaseName = System.IO.Path.GetFileNameWithoutExtension(filename);
            using (var connection = new System.Data.SqlClient.SqlConnection(
                "Data Source=.\\sqlexpress;Initial Catalog=tempdb; Integrated Security=true;User Instance=True;"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        String.Format("CREATE DATABASE {0} ON PRIMARY (NAME={0}, FILENAME='{1}')", databaseName, filename);
                    command.ExecuteNonQuery();

                    command.CommandText =
                        String.Format("EXEC sp_detach_db '{0}', 'true'", databaseName);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public class ExecuteAndRedirectOutput
    {
        private System.Diagnostics.Process _p;
        public ExecuteAndRedirectOutput(string file, string arguments)
        {
            _p = new System.Diagnostics.Process();
            _p.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = file,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                //RedirectStandardOutput = true,
            };
        }

        public void StartAndWaitForExit()
        {
            _p.Start();
            _p.WaitForExit();
            //Console.WriteLine(_p.StandardOutput.ReadToEnd());
            if (_p.ExitCode != 0)
            {
                throw new Exception(String.Format("Process exit code {0}, with output:\n------\n{1}\n------\n", _p.ExitCode, _p.StandardError.ReadToEnd()));
            }
        }
    }

    public class Migrator
    {
        private readonly string _db;
        public Migrator(string db)
        {
            _db = db;
        }
        public void Migrate()
        {
            var migratePath = Directory.GetDirectories(Path.Combine("..", "..", "..", "packages"), "FluentMigrator.*").Single();
            var str = string.Join(";", new[]{"Data Source=(LocalDB)\\v11.0;",
      "AttachDbFilename="+Path.Combine(Directory.GetCurrentDirectory() ,_db),
      "Integrated Security=True"
    });
            var conn = " /connection \"" + str + "\" /db SqlServer2012 /target DbMigrations.dll";
            var migrator = new ExecuteAndRedirectOutput(Path.Combine(migratePath, "tools", "Migrate.exe"),
                conn
                );

            migrator.StartAndWaitForExit();
        }

    }
}
