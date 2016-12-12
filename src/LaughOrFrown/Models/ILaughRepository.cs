using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.Models
{
    public interface ILaughRepository
    {
        Joke GetJoke(int id);

        IEnumerable<Joke> GetJokes();

        LaughUser GetUser(string id);

        void AddJoke(Joke joke);

        void AddRating(Rating rating);

        void UpdateRating(int id, int hotRating, int offensiveRating);

        Task<bool> Save();

        void UpdateUser(LaughUser user);
    }
}
