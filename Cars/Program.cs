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
			//--------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//which car has the best fuel efficency we also want to have any tied values be list in alphabetical order, via extension syntax
			//var query = cars.OrderByDescending(c => c.Combined)
			//			//this is a secondary sort DONT DO A SECOND ORDERBY as below, linq sees this a ordering by car name
			//			//which undoes the OrderByDescending from above and goes back to reading the entire dataset so you
			//			//won't be get just the resulting sequence from above
			//			//.OrderBy(c => c.Name)
			//			//DO IT THIS WAY use .ThenBy() and you can do this as many times as you have columns to sort
			//			.ThenBy(c => c.Name);

			//which car has the best fuel efficency we also want to have any tied values be list in alphabetical order, via extension syntax
			//var query =
			//		//given a car "c" where  . . .
			//					//Lambda expression explanation: take a single car and produce a boolean to say if the car should be added to the list of cars
			//		cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
			//							//Lambda expression explanation: take a single parameter from the sequence in this case 1 car into the function(OrderByDescending())
			//							//write an expression that produces a result the operator*(OrderByDescending) can use
			//							//so when im ordering cars I want to produce a value that OrderByDescending can use to sort things
			//			.OrderByDescending(c => c.Combined)
			//			.ThenBy(c => c.Name);
			//---------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//which car has the best fuel efficency we also want to have any tied values be list in alphabetical order, via query syntax
			var query =
				from car in cars
				// filtering operator "Where" we only want BMW's from 2016, we are filtering before we order because of the streaming deferred execution
				where car.Manufacturer == "BMW" && car.Year == 2016
				//	first we order by fuel efficency then the car name.  ascending is the default order method so you don't have to specify 
				//	unless you want to be obvious
				orderby car.Combined descending, car.Name ascending
				select car;

			foreach (var car in query.Take(10))
			{
				Console.WriteLine($"{car.Manufacturer} { car.Name} : {car.Combined}");
			}
		}

		//we make this method to build car objects from the csv file and put them in memory so we can ask questions about the cars
		private static List<Car> ProcessFile(string filePath)
		{
			//one way to use extension method where we create a custom extension method 
			var query =
				File.ReadAllLines(filePath)
				.Skip(1)
				//filter out empty lines "where l is a line, give me line where the line length is greater than 1"
				.Where(l => l.Length > 1)
				//-----------------------------------------------------------------------------------------------
				//given a line l invoke "ParseFromCsv" on that line 
				//this is one way to do it
				//.Select(l => Car.ParseFromCsv(l));
				//but we can also do it this way to make it more clear, this is a custom extension method where
				//we turn each line into a car directly
				//------------------------------------------------------------------------------------------------
				.ToCar();

			//----------------------------------------------------------------------------------------------------------------------------
			//extension method syntax to return a list of cars
			//return
			//	File.ReadAllLines(filePath)
			//		.Skip(1)
			//		.Where(line => line.Length > 1)
			//		.Select(Car.ParseFromCsv)
			//		.ToList();

			//another way to do this is with query syntax
			//var query =
			//		//read each line in the file path, except for the first line which is a header
			//		from line in File.ReadAllLines(filePath).Skip(1)
			//		//the last line is blank so we want to exclude that
			//		where line.Length > 1
			//		//take each line and build a car via the "ParseFromCsv method in the Car class"
			//		select Car.ParseFromCsv(line)
			//-----------------------------------------------------------------------------------------------------------------------------
			return query.ToList();
		}
	}

	//normally this would be done in a seperate file, but for ease of demo we will do it here
	//these are custom Linq operators we want to use when querying and creating cars 
	public static class CarExtensions
	{										//---------------------------------------------------------------------------------------------------------------------------------------------
											//												taking in a string
											//												|	   returning a Car
											//												|	   |
											//this extension method does not need the Func<string, Car>, because we are not dealing with a generic method to transform a series of strings
											//this is a dedicated method to transform a string into a car, so we don't need the Func<string, car>
											//----------------------------------------------------------------------------------------------------------------------------------------------
		public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
		{
			foreach (var line in source)
			{
				var columns = line.Split(',');
				yield return new Car
				{
					Year = int.Parse(columns[0]),
					Manufacturer = columns[1],
					Name = columns[2],
					Displacement = double.Parse(columns[3]),
					Cylinders = int.Parse(columns[4]),
					City = int.Parse(columns[5]),
					Highway = int.Parse(columns[6]),
					Combined = int.Parse(columns[7])
				};
			}
		}
	}
}
