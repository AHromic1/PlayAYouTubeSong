using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using NAudio.Wave;
using YoutubeExplode;





namespace API_tries2
{
	internal class Song
	{

		//public Song() { }

		public async Task SearchYouTubeVideos()
		{
			var youtubeService = new YouTubeService(new BaseClientService.Initializer()
			{
				ApiKey = "AIzaSyDGuW4OZgNlerudPj8I6uSCwyD2uUhY74I",
				ApplicationName = this.GetType().ToString()
			});
			var searchListRequest = youtubeService.Search.List("snippet");
			searchListRequest.Q = "Queen"; // Any search term, can be a parameter send to a function, this is just a hardcoded version
			searchListRequest.MaxResults = 50;
			// Call the search.list method to retrieve results matching the specified query term.
			var searchListResponse = await searchListRequest.ExecuteAsync();
			List<string> videos = new List<string>();
			List<string> channels = new List<string>();
			List<string> playlists = new List<string>();
			// Add each result to the appropriate list, and then display the lists of
			// matching videos, channels, and playlists.
			foreach (var searchResult in searchListResponse.Items)
			{
				switch (searchResult.Id.Kind)
				{
					case "youtube#video":
						videos.Add(string.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
						break;
					case "youtube#channel":
						channels.Add(string.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
						break;
					case "youtube#playlist":
						playlists.Add(string.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
						break;
				}
			}
			Console.WriteLine(string.Format("Videos:\n{0}\n", string.Join("\n", videos)));
			Console.WriteLine(string.Format("Channels:\n{0}\n", string.Join("\n", channels)));
			Console.WriteLine(string.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));
		}

		//returns a youtuvideo object 

		public async Task<string> GetYouTubeVideo()
		{
			var youtubeService = new YouTubeService(new BaseClientService.Initializer()
			{
				ApiKey = "AIzaSyDGuW4OZgNlerudPj8I6uSCwyD2uUhY74I",
				ApplicationName = this.GetType().ToString()
			});

			var searchListRequest = youtubeService.Search.List("snippet");
			searchListRequest.Q = "Queen"; // Replace with your search term.
			searchListRequest.MaxResults = 1;

			var searchListResponse = await searchListRequest.ExecuteAsync();

			if (searchListResponse.Items.Count > 0)
			{
				var firstResult = searchListResponse.Items[0];

				switch (firstResult.Id.Kind)
				{
					case "youtube#video":
						Console.WriteLine("Video ID: " + firstResult.Id.VideoId);
						Console.WriteLine("Video Title: " + firstResult.Snippet.Title);
						return firstResult.Id.VideoId;
				}
			}
			Console.WriteLine("No video found.");
			return null; // Return null if no video was found
		}
		//returns a video using url
		public async Task<string> GetYouTubeVideoFromUrl(string videoUrl)
		{
			var youtubeService = new YouTubeService(new BaseClientService.Initializer()
			{
				ApiKey = "AIzaSyDGuW4OZgNlerudPj8I6uSCwyD2uUhY74I",
				ApplicationName = this.GetType().ToString()
			});

			var videoId = ExtractVideoIdFromUrl(videoUrl);

			if (videoId != null)
			{
				var searchListRequest = youtubeService.Search.List("snippet");
				searchListRequest.Q = videoId; // Use the video ID as the search query
				searchListRequest.MaxResults = 1;

				var searchListResponse = await searchListRequest.ExecuteAsync();

				if (searchListResponse.Items.Count > 0)
				{
					var firstResult = searchListResponse.Items[0];

					switch (firstResult.Id.Kind)
					{
						case "youtube#video":
							Console.WriteLine("Video ID: " + firstResult.Id.VideoId);
							Console.WriteLine("Video Title: " + firstResult.Snippet.Title);
							return firstResult.Id.VideoId;
					}
				}
			}
				Console.WriteLine("No video found.");
				return null; // Return null if no video was found

			
		}
		//an attempt at a method to play a YouTube song
		public void PlayYouTubeSong(string videoId)
		{
			var youtubeService = new YouTubeService(new BaseClientService.Initializer()
			{
				ApiKey = "AIzaSyDGuW4OZgNlerudPj8I6uSCwyD2uUhY74I",
				ApplicationName = this.GetType().ToString()
			});

			var searchListRequest = youtubeService.Search.List("snippet");
			searchListRequest.Q = videoId;
			searchListRequest.MaxResults = 1;

			var searchListResponse = searchListRequest.Execute();
			var video = searchListResponse.Items.FirstOrDefault();
			if (video != null)
			{
				var videoUrl = $"https://www.youtube.com/watch?v={video.Id.VideoId}";
		//PROBLEM: ova stranica vise ne postoji, kako pretvoriti u mp3??
				var mediaUrl = $"https://www.youtubeinmp3.com/fetch/?video={videoUrl}";

				using (var audioStream = new MediaFoundationReader(mediaUrl))
				{
					using (var waveOut = new WaveOutEvent())
					{
						waveOut.Init(audioStream);
						waveOut.Play();
						while (waveOut.PlaybackState == PlaybackState.Playing)
						{
							System.Threading.Thread.Sleep(100);
						}
					}
				}
			}
		}



		public string ExtractVideoIdFromUrl(string videoUrl)
		{
			// Extract the video ID from the YouTube video URL
			// Example video URLs: https://www.youtube.com/watch?v=VIDEO_ID
			//                     https://youtu.be/VIDEO_ID
			var uri = new Uri(videoUrl);
			var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
			var videoId = queryParams["v"] ?? uri.Segments[uri.Segments.Length - 1];

			return videoId;
		}
	}
}
