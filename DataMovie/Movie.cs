using System.Collections.Generic;

namespace DataMovie
{
    public struct Movie
    {
        public string title;
        public string language;
        public HashSet<Staff> staffs;
        public HashSet<Tag> tags;
        public float averageRating;

        public Movie(string title, string language)
        {
            this.title = title;
            this.language = language;
            staffs = new HashSet<Staff>();
            tags = new HashSet<Tag>();
            averageRating = 0;
        }
    }
}
