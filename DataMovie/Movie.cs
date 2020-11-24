using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataMovie
{
    public class Movie
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

        public List<Movie> GetSimilarMovies()
        {
            Dictionary<Movie, double> dictOfSimilarMovies = new Dictionary<Movie, double>();
            foreach (var movie in from tag in this.tags
                                  from movie in tag.movies
                                  select movie)
            {
                if (dictOfSimilarMovies.ContainsKey(movie))
                {
                    dictOfSimilarMovies[movie]++;
                }
                else
                {
                    dictOfSimilarMovies.Add(movie, 1);
                }
            }

            foreach (var staff in this.staffs)
            {
                if (staff.isActor.Contains(this))
                    foreach (var movie in staff.isActor)
                    {
                        if (dictOfSimilarMovies.ContainsKey(movie))
                        {
                            dictOfSimilarMovies[movie]++;
                        }
                        else
                        {
                            dictOfSimilarMovies.Add(movie, 1);
                        }
                    }
                else foreach(var movie in staff.isDirector)
                    {
                        if (dictOfSimilarMovies.ContainsKey(movie))
                        {
                            dictOfSimilarMovies[movie]++;
                        }
                        else
                        {
                            dictOfSimilarMovies.Add(movie, 1);
                        }
                    }
            }
            double maxValue = 0;
            foreach (var item in from item in dictOfSimilarMovies
                                 where item.Value > maxValue && !item.Key.Equals(this)
                                 select item)
            {
                maxValue = item.Value;
            }

            dictOfSimilarMovies.Remove(this);

            
            dictOfSimilarMovies = dictOfSimilarMovies.OrderByDescending(
                pair => pair.Value / maxValue * 0.5 + pair.Key.averageRating * 0.05).ToDictionary(
                pair => pair.Key, pair => pair.Value / maxValue * 0.5 + pair.Key.averageRating * 0.05);
            foreach (var item in dictOfSimilarMovies)
            {
                System.Console.WriteLine(item.Value + " " + item.Key.averageRating.ToString());
                //dictOfSimilarMovies[item.Key] = item.Value / maxValue * 0.5 + item.Key.averageRating * 0.5;
            }
            int counter = 0;
            List<Movie> toReturn = new List<Movie>();
            foreach(var item in dictOfSimilarMovies)
            {
                toReturn.Add(item.Key);
                counter++;
                if (counter == 10)
                    break;
            }
            return toReturn;
        }

    }
}
