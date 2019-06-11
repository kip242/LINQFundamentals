using System;
using System.Collections.Generic;
using System.Text;

namespace Queries
{
	public class Movie
	{
		int _year;
		public string Title { get; set; }
		public double Rating { get; set; }

		//we are going to add logging to determine when the LINQ query is executing and inspecting the Year property
		//and trying to determine if the movie should be in a query result or not
		public int Year
		{
			get
			{
				//Logging statement 
				Console.WriteLine($"Returning {_year} for {Title}");
				return _year;
			}
			set
			{
				_year = value;
			}
		}

		
	}
}
