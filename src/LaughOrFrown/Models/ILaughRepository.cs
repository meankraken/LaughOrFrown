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

    }
}
