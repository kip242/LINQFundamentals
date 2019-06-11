using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
	class Program
	{
		static void Main(string[] args)
		{
			//a deferred execution operator like "Where" is considered lazy, it won't execute until the foreach loop
			//whereas "ToList()" is not, it will execute right away

			//here we are calling our Random method, but it won't create an infinate loop because we are only "yielding" the first 10(.Take(10)) results
			//this is because Take() is a streaming operator it only goes through the data until it gets the number of items it needs then stops
			//if we did not have the Take() streaming operator below, the below query (while deferred) produces an infinate loop
			var numbers = MyLinq.Random().Where(n => n > 0.5).Take(10);

			//because Mylinq.Random() is a deferred execution method it doesn't get executed until the following foreach loop
			foreach (var number in numbers)
			{
				Console.WriteLine(number);
			}


			//you can use streaming and non streaming in the same statement.  this would be something like
			// var query = movies.Where(m => m.Year > 2000)
			//					 .OrderByDescending(m => m.Rating)
			//above the streaming part of the the query is the Linq operator "Where" this is just getting movies > 2000 and creating that subset of data
			//the non streaming part is "OrderByDescending" this has to look at the entire subset to order them
			//it makes sense to do it this way so you are not ordering the entire data set then pulling out the data you need, you are only ordering the movies
			//that were released after 2000, it is more efficient this way when in comes to in memory data(List, Array, IEnumerable)

			var movies = new List<Movie>
			{
				new Movie{ Title = "The Dark Knight", Rating = 8.9f, Year = 2008},
				new Movie{ Title = "Teh King's Speech", Rating = 8.0f, Year = 2010},
				new Movie{ Title = "Casablanca", Rating = 8.5f, Year = 1942},
				new Movie{ Title = "Star Wars V", Rating = 8.7f, Year = 1980},
			};


			//here we are building the query
			//if you want to not use deferred excution and build a concrete list, you need to add .ToList(), .ToArray(), .ToDictionary() at end of the query at this point we 
			//have a list of movies we can the do things on that list
			var query = movies.Filter(m => m.Year > 2000);

			//with deferred execution queries do not get executed until the foreach loop
			//this is the same as PHP execute command I think
			foreach (var movie in query)
			{
				Console.WriteLine(movie.Title);
			}
		}
	}
}
