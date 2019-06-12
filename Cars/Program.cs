using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
	class Program
	{
		static void Main(string[] args)
		{
			var cars = ProcessFile("fuel.csv");

			//which car has the best fuel efficency we also want to have any tied values be list in alphabetical order, via extension syntax
			var query = cars.OrderByDescending(c => c.Combined)
						//this is a secondary sort DONT DO A SECOND ORDERBY as below, linq sees this a ordering by car name
						//which undoes the OrderByDescending from above and goes back to reading the entire dataset so you
						//won't be get just the resulting sequence from above
						//.OrderBy(c => c.Name)
						//DO IT THIS WAY use .ThenBy() and you can do this as many times as you have columns to sort
						.ThenBy(c => c.Name);

			//which car has the best fuel efficency we also want to have any tied values be list in alphabetical order, via query syntax
			//var query =
			//	from car in cars
				//first we order by fuel efficency then the car name.  ascending is the default order method so you don't have to specify 
				//unless you want to be obvious
			//	orderby car.Combined descending, car.Name ascending
			//	select car;

			foreach (var car in query.Take(10))
			{
				Console.WriteLine($"{ car.Name} : {car.Combined}");
			}
		}

		//we make this method to build car objects from the csv file and put them in memory so we can ask questions about the cars
		private static List<Car> ProcessFile(string filePath)
		{
			//extension method syntax
			return
				File.ReadAllLines(filePath)
					.Skip(1)
					.Where(line => line.Length > 1)
					.Select(Car.ParseFromCsv)
					.ToList();

			//another way to do this is with query syntax
			//var query =
			//		//read each line in the file path, except for the first line which is a header
			//		from line in File.ReadAllLines(filePath).Skip(1)
			//		//the last line is blank so we want to exclude that
			//		where line.Length > 1
			//		//take each line and build a car via the "ParseFromCsv method in the Car class"
			//		select Car.ParseFromCsv(line)
			
			//return query.ToList();
		}
	}
}
