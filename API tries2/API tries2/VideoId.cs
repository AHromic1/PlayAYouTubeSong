using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_tries2
{
	internal class VideoId
	{
		//public VideoId() { }
		public string GetVideoIdFromUrl(string videoUrl)
		{
			Uri uri = new Uri(videoUrl);
			string query = uri.Query;
			string videoId = string.Empty;

			if (!string.IsNullOrEmpty(query))
			{
				string[] queryParams = query.TrimStart('?').Split('&');
				foreach (string param in queryParams)
				{
					string[] keyValue = param.Split('=');
					if (keyValue.Length == 2 && keyValue[0].ToLower() == "v")
					{
						videoId = keyValue[1];
						break;
					}
				}
			}

			return videoId;
		}
	}
}
