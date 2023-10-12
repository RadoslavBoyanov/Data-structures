using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Exam.MovieDatabase
{
    public class Movie
    {
        public string Id { get; set; }

        public int DurationInMinutes { get; set; }

        public string Title { get; set; }

        public double Rating { get; set; }

        public double Budget { get; set; }

        public ICollection<Actor> Actors { get; set; }

        public Movie(string id, int durationInMinutes, string title, double rating, double budget)
        {
            Id = id;
            DurationInMinutes = durationInMinutes;
            Title = title;
            Rating = rating;
            Budget = budget;
            this.Actors = new List<Actor>();    
        }
    }
}
