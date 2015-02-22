using System;
using System.IO;
using System.Linq;

namespace SomeBasicEFApp.Tests
{
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
			var migratePath = Directory.GetDirectories(Path.Combine("..", "..", "..", "packages"), "FluentMigrator.*").Last();
			var migrator = new ExecuteAndRedirectOutput(Path.Combine(migratePath, "tools", "Migrate.exe"), "/connection \"Data Source=" + _db + ";Version=3;\" /db sqlite /target DbMigrations.dll");

			migrator.StartAndWaitForExit();
		}
	}
}
