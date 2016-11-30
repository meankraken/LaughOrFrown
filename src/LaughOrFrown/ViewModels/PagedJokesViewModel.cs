using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.ViewModels
{
    public class PagedJokesViewModel //view model for holding jokes and page data  
    {
        public int currentPage; //current page

        public int perPage; //jokes per page

        public int totalJokes; //total number of jokes 

        public IEnumerable<JokeViewModel> Jokes; //jokes to display

        public bool hasPreviousPage; //flag for previous page

        public bool hasNextPage; //flag for next page

        public PagedJokesViewModel(int thePage, int eachPage, int totalCount, IEnumerable<JokeViewModel> theJokes)
        {
            currentPage = thePage;
            perPage = eachPage;
            totalJokes = totalCount;
            Jokes = theJokes;
            hasPreviousPage = getHasPreviousPage();
            hasNextPage = getHasNextPage();
        }

        private bool getHasPreviousPage()
        {
            if (currentPage > 1)
            {
                return true;
            }
            else return false;
        }

        private bool getHasNextPage()
        {
            if ((currentPage * perPage) < totalJokes)
            {
                return true;
            }
            else return false;
        }
    }
}
