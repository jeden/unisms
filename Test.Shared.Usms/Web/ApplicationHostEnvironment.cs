using System;
using System.Collections.Generic;
using System.IO;

namespace Test.Shared.Usms.Gateway.Web
{
	public sealed class ApplicationHostEnvironment : IDisposable
	{
		private readonly Stack<string> _filesToRemove = new Stack<string>();
		private readonly string _basePath;
		private readonly string _binFolder;

		public void Dispose()
		{
			Cleanup();
		}

		public ApplicationHostEnvironment(string basePath)
		{
			_basePath = basePath;
			if (_basePath.EndsWith(@"\") == false)
				_basePath += @"\";

			_binFolder = _basePath + @"bin\";
			if (Directory.Exists(_binFolder) == false)
				Directory.CreateDirectory(_binFolder);

			AddAssemblies();
		}

		private void AddAssemblies()
		{
			string[] files;

			files = Directory.GetFiles(_basePath, "*.dll");
			foreach (string file in files)
				AddToBinFolder(file);
		}

		private void AddToBinFolder(string filename)
		{
			FileInfo file;

			file = new FileInfo(filename);
			file.CopyTo(_binFolder + file.Name, true);
			_filesToRemove.Push(file.Name);
		}

		private void Cleanup()
		{
			while (_filesToRemove.Count > 0)
			{
				var filename = _filesToRemove.Pop();
				File.Delete(_binFolder + filename);
			}

			if (Directory.Exists(_binFolder))
				Directory.Delete(_binFolder);
		}
	}
}
