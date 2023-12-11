using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.RePlay
{
    public class RePlayer : IRePlayer
    {
        private Dictionary<string, Dictionary<string, Track>> albumsByName = new Dictionary<string, Dictionary<string, Track>>();
        private HashSet<Track> tracks = new HashSet<Track>();
        private Queue<Track> queue = new Queue<Track>();

        public int Count => tracks.Count;

        public bool Contains(Track track) => tracks.Contains(track);

        public void AddTrack(Track track, string album)
        {
            if (Contains(track)) throw new ArgumentNullException();

            if (!albumsByName.ContainsKey(album))
            {
                albumsByName.Add(album, new Dictionary<string, Track>());
            }

            track.Album = album;
            albumsByName[album].Add(track.Title, track);
            tracks.Add(track);
        }

        public Track GetTrack(string title, string albumName)
        {
            if (!albumsByName.ContainsKey(albumName) ||
                !albumsByName[albumName].ContainsKey(title))
            {
                throw new ArgumentException();
            }

            return albumsByName[albumName][title];
        }

        public IEnumerable<Track> GetAlbum(string albumName)
        {
            if (!albumsByName.ContainsKey(albumName))
            {
                throw new ArgumentException();
            }

            return albumsByName[albumName].Values
                .OrderByDescending(t => t.Plays);
        }

        public void AddToQueue(string trackName, string albumName)
        {
            if (!albumsByName.ContainsKey(albumName) ||
               !albumsByName[albumName].ContainsKey(trackName))
            {
                throw new ArgumentException();
            }

            var track = albumsByName[albumName][trackName];

            queue.Enqueue(track);
        }

        public Track Play()
        {
            if (queue.Count == 0)
            {
                throw new ArgumentException();
            }

            var track = queue.Dequeue();

            if (track.IsDeleted)
            {
                return this.Play();
            }

            track.Plays++;

            return track;
        }

        public void RemoveTrack(string trackTitle, string albumName)
        {
            if (!albumsByName.ContainsKey(albumName) ||
               !albumsByName[albumName].ContainsKey(trackTitle))
            {
                throw new ArgumentException();
            }

            var track = albumsByName[albumName][trackTitle];
            track.IsDeleted = true;
            tracks.Remove(track);
            albumsByName[albumName].Remove(trackTitle);
        }

        public IEnumerable<Track> GetTracksInDurationRangeOrderedByDurationThenByPlaysDescending(int lowerBound, int upperBound)
            => tracks
            .Where(t => t.DurationInSeconds >= lowerBound && t.DurationInSeconds <= upperBound)
            .OrderBy(t => t.DurationInSeconds)
            .ThenByDescending(t => t.Plays);

        public IEnumerable<Track> GetTracksOrderedByAlbumNameThenByPlaysDescendingThenByDurationDescending()
            => tracks.OrderBy(t => t.Album)
            .ThenByDescending(t => t.Plays)
            .ThenByDescending(t => t.DurationInSeconds);

        public Dictionary<string, List<Track>> GetDiscography(string artistName)
        {
            var artistTracks = tracks.Where(t => t.Artist == artistName);

            if (artistTracks.Count() == 0)
            {
                throw new ArgumentException();
            }

            var result = new Dictionary<string, List<Track>>();

            foreach (var track in artistTracks)
            {
                if (!result.ContainsKey(track.Album))
                {
                    result.Add(track.Album, new List<Track>());
                }

                result[track.Album].Add(track);
            }

            return result;
        }
    }
}