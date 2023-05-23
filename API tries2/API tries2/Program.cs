using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using NAudio.Wave;
using YoutubeExplode.Common;
using API_tries2;

// ...

Song song=new Song();
//OVA METODA VRATI ODGOVARAJUCI VIDEO, IZ URL!
song.GetYouTubeVideoFromUrl("https://www.youtube.com/watch?v=fJ9rUzIMcZQ").GetAwaiter().GetResult();

string videoId = song.ExtractVideoIdFromUrl("https://www.youtube.com/watch?v=fJ9rUzIMcZQ");

song.PlayYouTubeSong(videoId); //PROBLEM s ovom metodom, baci izuzetak
