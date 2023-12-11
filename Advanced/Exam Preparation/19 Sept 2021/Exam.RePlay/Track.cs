namespace Exam.RePlay
{
    public class Track
    {
        public Track(string id, string title, string artist, int plays, int durationInSeconds)
        {
            this.Id = id;
            this.Title = title;
            this.Artist = artist;
            this.Plays = plays;
            this.DurationInSeconds = durationInSeconds;
            this.Album = null;
            this.IsDeleted = false;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public int Plays { get; set; }

        public int DurationInSeconds { get; set; }

        public string Album { get; set; }

        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            return Id.Equals((obj as Track).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
