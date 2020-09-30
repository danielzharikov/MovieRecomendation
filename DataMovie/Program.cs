using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataMovie
{
    class Program
    {
        const string pathMovieCodes = "MovieCodes_IMDB.tsv";
        const string pathRatings = "Ratings_IMDB.tsv";
        const string pathActorsDirectorsCodes = "ActorsDirectorsCodes_IMDB.tsv";
        const string pathActorsDirectorsNames = "ActorsDirectorsNames_IMDB.txt";
        const string pathLinks = "links_IMDB_MovieLens.csv";
        const string pathTagCodes = "TagCodes_MovieLens.csv";
        const string pathTagScores = "TagScores_MovieLens.csv";

        // IMDBID - string
        static Dictionary<string, Movie> moviesWithImdbID = new Dictionary<string, Movie>();
        static Dictionary<string, Staff> staffsWithID = new Dictionary<string, Staff>();
        static Dictionary<int, Tag> tagsWithID = new Dictionary<int, Tag>();
        static Dictionary<int, int> imdbIDWithTmdbID = new Dictionary<int, int>();
        static Dictionary<string, Movie> moviesWithTitles = new Dictionary<string, Movie>();
        static Dictionary<string, Staff> staffsWithNames = new Dictionary<string, Staff>();
        static Dictionary<string, Tag> tagsWithNames = new Dictionary<string, Tag>();


        static void Main(string[] args)
        {
            GetDictionaryOfMoviesAndImdbID();
            GetDictionaryOfStaffNames();
            GetInfoActorsAndDirectors();
            Console.ReadKey();
        }

        public static void GetDictionaryOfMoviesAndImdbID()
        {
            DateTime timeStart = DateTime.Now;
            var tempstrings = File
                .ReadLines(pathMovieCodes)
                .AsParallel()
                .Skip(1)
                .Select(line => line.Split("\t"));

            foreach (string[] line in tempstrings)
            {
                if ((line[4] == "ru" || line[4] == "en") && !moviesWithImdbID.ContainsKey(line[0]))
                {
                    moviesWithImdbID.Add(line[0], new Movie(line[2], line[4]));
                }
            }
            Console.WriteLine((DateTime.Now - timeStart).ToString());
        }

        public static void GetDictionaryOfStaffNames()
        {
            DateTime timeStart = DateTime.Now;
            staffsWithID = File
                .ReadLines(pathActorsDirectorsNames)
                .AsParallel()
                .Skip(1)
                .Select(line => line.Split("\t"))
                .ToDictionary(line => line[0], line => new Staff(line[1]));
            Console.WriteLine((DateTime.Now - timeStart).ToString());
        }

        public static void GetInfoActorsAndDirectors()
        {
            DateTime timeStart = DateTime.Now;
            var tempLinksOfActorsAndDirectors = File
                .ReadLines(pathActorsDirectorsCodes)
                .AsParallel()
                .Skip(1)
                .Select(line => line.Split("\t"));

            foreach (string[] line in tempLinksOfActorsAndDirectors)
            {
                switch (line[3])
                {
                    case ("director"):
                        ConnectMovieWithStaff(line[0], line[2], false);
                        break;
                    case ("actor"):
                        ConnectMovieWithStaff(line[0], line[2], true);
                        break;
                }
                Staff staff;
                staffsWithID.TryGetValue(line[2], out staff);
                Console.WriteLine(staff.fullName);
            }



            Console.WriteLine((DateTime.Now - timeStart).ToString());
        }

        public static void ConnectMovieWithStaff(string keyOfMovie, string keyOfStaff, bool isActor)
        {
            if (!(moviesWithImdbID.ContainsKey(keyOfMovie) && staffsWithID.ContainsKey(keyOfStaff)))
                return;
            Staff tempStaff;
            staffsWithID.TryGetValue(keyOfStaff, out tempStaff);
            Movie tempMovie;
            moviesWithImdbID.TryGetValue(keyOfMovie, out tempMovie);

            tempMovie.staffs.Add(tempStaff);
            if (isActor)
                tempStaff.isActor.Add(tempMovie);
            else
                tempStaff.isDirector.Add(tempMovie);
        }

        public struct Movie
        {
            public string title;
            public string language;
            public HashSet<Staff> staffs;
            public HashSet<Tag> tags;
            public int averageRating;

            public Movie(string title, string language)
            {
                this.title = title;
                this.language = language;
                staffs = new HashSet<Staff>();
                tags = new HashSet<Tag>();
                averageRating = 0;
            }
        }

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
}
