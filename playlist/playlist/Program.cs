using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Playlist
{
    // Class to represent a song
    class Song
    {   public string Title { get; set; }
        public string Artist { get; set; }
        public int Year { get; set; }
        // Constructor to initialize the Song object
        public Song(string title, string artist, int year)
        {
            this.Title = title;
            this.Artist = artist;
            this.Year = year;
        }
        // Overrides the ToString() method for better string representation
        public override string ToString()
        {
            return $"{Title} by {Artist} ({Year})";
        }
    }

    // Interface for objects that can be serialized to/from JSON
    interface IJsonSerializable
    {
        string ToJson();
        void FromJson(string json);
    }

    // Represents a simple music player that manages a playlist of songs
    class MusicPlayer : IJsonSerializable
    {
        // List to store songs in the playlist
        private List<Song> songs = new List<Song>();

        // Adds a new song to the playlist
        public void AddSong(string title, string artist, int year)
        {
            var newSong = new Song(title, artist, year);
            songs.Add(newSong);
            Console.WriteLine($"Added song: {newSong}");
        }

        // Overloaded method to add a new song with default year
        public void AddSong(string title, string artist)
        {
            AddSong(title, artist, DateTime.Now.Year);
        }

        // Removes a song from the playlist based on the title
        public void RemoveSong(string title)
        {
            Song songToRemove = null;

            foreach (var song in songs)
            {
                if (string.Equals(song.Title, title))
                {
                    songToRemove = song;
                    break;
                }
            }

            if (songToRemove != null)
            {
                songs.Remove(songToRemove);
                Console.WriteLine($"Removed song: {songToRemove}");
            }
            else
            {
                Console.WriteLine($"Song '{title}' does not exist.");
            }
        }

        // Displays the list of songs in the playlist
        public void DisplaySongs()
        {
            Console.WriteLine(songs.Count == 0 ? "No songs in the playlist." : "Playlist:\n" + string.Join("\n", songs));
        }

        // Searches for songs in the playlist based on a keyword
        public void SearchSong(string keyword)
        {
            List<Song> matchingSongs = new List<Song>();

            foreach (var song in songs)
            {
                if (song.Title.Contains(keyword) ||
                    song.Artist.Contains(keyword))
                {
                    matchingSongs.Add(song);
                }
            }

            if (matchingSongs.Count == 0)
            {
                Console.WriteLine($"No songs matching the keyword '{keyword}'.");
            }
            else
            {
                Console.WriteLine($"Found songs matching the keyword '{keyword}':");
                foreach (var matchingSong in matchingSongs)
                {
                    Console.WriteLine(matchingSong);
                }
            }
        }

        // Saves the playlist to a JSON file
        public void SavePlaylistToJson()
        {
            File.WriteAllText("playlist.json", ToJson());
            Console.WriteLine("Playlist saved to 'playlist.json'.");
        }

        // Loads the playlist from a JSON file
        public void LoadPlaylistFromJson()
        {
            if (File.Exists("playlist.json"))
            {
                FromJson(File.ReadAllText("playlist.json"));
                Console.WriteLine("Playlist loaded from 'playlist.json'.");
            }
            else
            {
                Console.WriteLine("No playlist file found.");
            }
        }

        // Converts the playlist to a JSON-formatted string
        public string ToJson()
        {
            return JsonConvert.SerializeObject(songs, Newtonsoft.Json.Formatting.Indented);
        }

        // Deserializes the playlist from a JSON-formatted string
        public void FromJson(string json)
        {
            this.songs = JsonConvert.DeserializeObject<List<Song>>(json);
        }
    }

    // Entry point of the application
    class Program
    {
        static void Main()
        {
            var musicPlayer = new MusicPlayer();

            while (true)
            {
                // Display menu options to the user
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Add a song");
                Console.WriteLine("2. Remove a song");
                Console.WriteLine("3. Display songs");
                Console.WriteLine("4. Search songs");
                Console.WriteLine("5. Save playlist to JSON");
                Console.WriteLine("6. Load playlist from JSON");
                Console.WriteLine("7. Exit");

                // Read and validate user input
                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice) || choice > 7 || choice < 1)
                {
                    Console.WriteLine("Invalid choice, enter a number from 1 to 7");
                    continue;
                }

                // Future actions based on user choice
                switch (choice)
                {
                    case 1:
                        Console.Write("Enter the title of the song: ");
                        var title = Console.ReadLine();
                        Console.Write("Enter the artist: ");
                        var artist = Console.ReadLine();
                        int year;

                        while (true)
                        {
                            Console.Write("Enter the year: ");
                            string input = Console.ReadLine();

                            if (int.TryParse(input, out year))
                            {
                                // Successfully read the year, break out of the loop
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid year format, please enter a valid year");
                            }
                        }
                        musicPlayer.AddSong(title, artist, year);
                        break;

                    case 2:
                        Console.Write("Enter the title of the song to remove: ");
                        musicPlayer.RemoveSong(Console.ReadLine());
                        break;

                    case 3:
                        musicPlayer.DisplaySongs();
                        break;

                    case 4:
                        Console.Write("Enter the keyword to search for: ");
                        musicPlayer.SearchSong(Console.ReadLine());
                        break;

                    case 5:
                        musicPlayer.SavePlaylistToJson();
                        break;

                    case 6:
                        musicPlayer.LoadPlaylistFromJson();
                        break;

                    case 7:
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
