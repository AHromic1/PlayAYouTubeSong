using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace API_tries2
{
	internal class Converter
	{
		public string DownloadAudio(string videoUrl)
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "youtube-dl",
					Arguments = $"-x --audio-format mp3 --audio-quality 0 --get-url {videoUrl}",
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};

			process.Start();
			string output = process.StandardOutput.ReadToEnd();
			process.WaitForExit();

			return output.Trim();
		}
	}
}
