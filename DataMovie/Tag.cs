using System.Collections.Generic;

namespace DataMovie
{
    public struct Tag
    {
        public string name;
        public HashSet<Movie> movies;

        public Tag(string name)
        {
            this.name = name;
            movies = new HashSet<Movie>();
        }
    }
}
