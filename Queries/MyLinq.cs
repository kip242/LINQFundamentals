using System;
using System.Collections.Generic;
using System.Text;

namespace Queries
{
	public static class MyLinq
	{
		//this is a custom Filter or "Where"
		//			  LINQ Query Operators Generally return IEnumerable<T>
		//			  |				 Need To add <T> as generic type parameter to the method
		//			  |				 |		   here we make this method and extension method for the IEnumerable<T> that we are returning
		//			  |	             |		   |                               when the method implemented, invoke this code to see if the user of the method wants this thing in the sourceData
		//	          |				 |		   |							   |
		public static IEnumerable<T> Filter<T>(this IEnumerable<T> sourceData, Func<T, bool> predicate)
		{
			//instantiating the result variable is commented out to enable deferred execution to make our Filter method act more like Linq's Where method
			//in other words, we are not building a concrete list and returning that list
			//var result = new List<T>();

			foreach (var item in sourceData)
			{
				//for each item in the source data call the predicate Func to determine if it belongs in the result List<T> (if predicate returns true it does)
				if (predicate(item))
				{
					//comment out building of result to enable deffered execution
					//result.Add(item);

					//using "yield return" item if predicate is true helps build the IEnumerable<T>, we are building a data structure that we can iterate over with a foreach loop in the calling code
					//execution will start inside the Filter method only when something tries to get pulled out of the IEnumerable, execution will begin and continue until
					//I hit the first "yield" statement
					//the "yield" statement yields control back to the caller and returning an item, the caller then can manipulate the item 
					//when the next iteration is made and the next item is gotten out of the IEnumerable, execution is picked up and resumed where we jumped out of the filter method
					//nothing happens in this method until we try to pull something out of the IEnumerable that I am producing with the "yield return"
					yield return item;
				}
			}
		}

		//To Demonstrate Streaming vs Non Streaming
		//This method is basically and infinate loop with deferred execution because of the "yield reutrn", it will keep producing numbers, 
		//we will make it not behave as infinate in the calling of the method in program.cs
		public static IEnumerable<double> Random()
		{
			var random = new Random();
			while (true)
			{
				yield return random.NextDouble();
			}
		}
	}
}
