using System.Collections.Generic;

namespace DataMovie
{
    public struct Staff
    {
        public string fullName;
        public HashSet<Movie> isActor;
        public HashSet<Movie> isDirector;

        public Staff(string fullName)
        {
            this.fullName = fullName;
            isActor = new HashSet<Movie>();
            isDirector = new HashSet<Movie>();
        }
    }
}
