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
			var cars = ProcessCars("fuel.csv");
			var manufacturers = ProcessManufacturers("manufacturers.csv");
			//--------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//                                                                                   Filtering Data
			//--------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//which car has the best fuel efficency we also want to have any tied values be list in alphabetical order, via extension syntax
			//var query = cars.OrderByDescending(c => c.Combined)
			//			//this is a secondary sort DONT DO A SECOND ORDERBY as below, linq sees this a ordering by car name
			//			//which undoes the OrderByDescending from above and goes back to reading the entire dataset so you
			//			//won't be get just the resulting sequence from above
			//			//.OrderBy(c => c.Name)
			//			//DO IT THIS WAY use .ThenBy() and you can do this as many times as you have columns to sort
			//			.ThenBy(c => c.Name);
			//
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
			//---------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//which car has the best fuel efficency we also want to have any tied values be list in alphabetical order, via query syntax
			//var query =
			//	from car in cars
			//	// filtering operator "Where" we only want BMW's from 2016, we are filtering before we order because of the streaming deferred execution
			//	where car.Manufacturer == "BMW" && car.Year == 2016
			//	//	first we order by fuel efficency then the car name.  ascending is the default order method so you don't have to specify 
			//	//	unless you want to be obvious
			//	orderby car.Combined descending, car.Name ascending
			//	select car;
			//----------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//----------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//                                                                                   Joining 
			//----------------------------------------------------------------------------------------------------------------------------------------------------------------------
			// trying to figure out where the car manufacuturer headquarters is via query syntax
			//var query =
			//	from car in cars
			//		//variable to be used in expressions throughout rest of this query
			//		//|				 second data source for the join
			//	join manufacturer in manufacturers 
			//		//what piece of data are we using to associate the 2 files, here the car manufacturer and the manufacturer name are the same in both the files
			//		on car.Manufacturer equals manufacturer.Name
			//	orderby car.Combined descending, car.Name ascending
			//	select new
			//	{
			//		manufacturer.Headquarters,
			//		car.Name,
			//		car.Combined
			//	};
			//
			//Lets say we need to join on 2 different pieces of information not only the car manufacturer, but also the car year against the manufacuter year to get the headquarters
			//for a given year, in this case we need to make an new anonymous object to put the information in
			//var query =
			//	from car in cars
			//		//variable to be used in expressions throughout rest of this query
			//		//|				 second data source for the join
			//	join manufacturer in manufacturers 
			//		//what piece of data are we using to associate the 2 files, here the car manufacturer and the manufacturer name are the same in both the files
			//		//this is also where we build the new object 
			//		//this doesn't work completely right because the properties for the joining objects, in this case car.Manufacturer and manufacturer.Name, need to be the same
			//		//on new { car.Manufacturer, car.Year } 
			//				equals 
			//				new { Manufacturer = manufacturer.Name, manufacturer.Year}
			//		//So We Will Do This
			//		//on new { car.Manufacturer, car.Year equals new { manufacturer.Name, manufacturer.Year}
			//	orderby car.Combined descending, car.Name ascending
			//	select new
			//	{
			//		manufacturer.Headquarters,
			//		car.Name,
			//		car.Combined
			//	};
			//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//trying to figure out where the car manufacuturer headquarters is via extension method syntax
			//var query =
			//	//joining 2 data sources
			//	cars.Join(manufacturers,
			//				//what are we putting together, in this case a Car and a Manufacturer on the Manufacturer property
			//				//of the car and the Name property of the Manufacturer(which are the same, the Primary and Foreign Key if you will)
			//				c => c.Manufacturer,
			//				m => m.Name, 
			//				//This is how we put it together, "Project" the object, produce a single IEnumerable of something
			//				//in this case we are doing an anonymous object, but we could make a class that has the 
			//				//Headquarters, Name, Combined properties called "ManLocations" or something like that and instatiate that
			//				(c, m) => new //ManLocations
			//				{
			//					m.Headquarters,
			//					c.Name,
			//					c.Combined
			//				})
			//	//continue Linq operations
			//	.OrderByDescending(c => c.Combined)
			//	.ThenBy(c => c.Name);

			//Lets say we need to join on 2 different pieces of information not only the car manufacturer, but also the car year against the manufacuter year to get the headquarters
			//for a given year, in this case we need to make an new anonymous object to put the information in
			//var query =
			//	//joining 2 data sources
			//	cars.Join(manufacturers,
			//				//what are we putting together, in this case a Car and a Manufacturer on the Manufacturer property
			//				//of the car and the Name property of the Manufacturer(which are the same, the Primary and Foreign Key if you will)
			//				c => new { c.Manufacturer, c.Year },
			//				m => new { Manufacturer = m.Name, m.Year },
			//				//This is how we put it together, "Project" the object, produce a single IEnumerable of something
			//				//in this case we are doing an anonymous object, but we could make a class that has the 
			//				//Headquarters, Name, Combined properties called "ManLocations" or something like that and instatiate that
			//				(c, m) => new //ManLocations
			//				{
			//					m.Headquarters,
			//					c.Name,
			//					c.Combined
			//				})
			//	//continue Linq operations
			//	.OrderByDescending(c => c.Combined)
			//	.ThenBy(c => c.Name);

			//foreach (var car in query.Take(10))
			//{
			//	//write line for finding fuel efficency
			//	//Console.WriteLine($"{car.Manufacturer} { car.Name} : {car.Combined}");
			//	//write line for finding car manufacturer headquaters
			//	Console.WriteLine($"{car.Headquarters} {car.Name} : {car.Combined}");
			//}

			//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//                                                                                Grouping data
			//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//trying to get 2 most fuel efficient cars by manufacturer via query syntax
			//we are going to group cars by manufacturer, go into each grouping and order the cars by combined gas mileage and grab the top 2

			//var query =
			//	from car in cars						//in order to be able to orderby we need to put the car into a named variable, this is because the car variable from above has been
			//											//grouped into a different sequence
			//	group car by car.Manufacturer.ToUpper() into manufacturer //we do ToUpper() because a couple of manufacturers appear twice once lower case, once upper case, this will combine
			//	orderby manufacturer.Key
			//	select manufacturer;

			//trying to get 2 most fuel efficient cars by manufacturer via extension method syntax
			//var query =
			//	cars.GroupBy(c => c.Manufacturer.ToUpper())
			//		.OrderBy(g => g.Key);

			//foreach (var group in query)
			//{
			//	//to print out results a group operator takes all the cars places them in buckets that have a Key
			//	//the Key is the value that I grouped on(car.Manufacturer), inside that grouping are the cars that fall under that manufacturer
			//	//this will show how many cars per manufacturer, this is something extra
			//	//Console.WriteLine($"{group.Key} has {group.Count()}");

			//	Console.WriteLine(group.Key);
			//	foreach (var car in group.OrderByDescending(c => c.Combined).Take(2))
			//	{
			//		Console.WriteLine($"\t{car.Name} : {car.Combined}");
			//	}
			//}

			//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//                                                                                Grouping data with GroupJoin operator
			//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
			//offers features of joining and grouping with one operator, you can join 2 data sets then group them.  It can do this because when GroupJoin joins the objects together
			//it builds a heirarchy, so instead of a flat result like with a join where each car object has an associated manufacturer object.  
			//With a groupjoin a single manufacturer object can have all its associated car objects
			//trying to get not only the 2 most fuel efficient cars by manufacturer, but also which country the manufacturer is headquarted in via query syntax

			//var query =
			//	//we are getting manufacturer first because this is the heading we want to group the cars under
			//	from manufacturer in manufacturers
			//		//this joins objects together
			//	join car in cars on manufacturer.Name equals car.Manufacturer
			//		//this makes it a GroupJoin for each manufacturer find all the cars match that manufacturer based on the ON part from above
			//		//   take all those cars and put them in a variable we can use later in the expression for sorting and projection. 
			//		//	 the manufacturer variable is also still available for us to use if needed, car is not available however
			//		//	 |
			//		into carGroup
			//	//orderby manufacturer.Name
			//	//projection make a new object
			//	select new
			//	{
			//		//properties   this is what everything was grouped into
			//		Manufacturer = manufacturer,
			//		Cars = carGroup
			//	};

			//trying to get not only the 2 most fuel efficient cars by manufacturer, but also which country the manufacturer is headquarted in via extension method syntax

			//var query =
			//	//here we are saying inner join manufacturers and cars on the outerKeySelector of manufacturer.Name and the innerKeySelector of car.Manufacturer(which are the same)
			//	//and give a resultSelector which is a Func that takes 2 parameters(Func<manufacturer, grouping of cars>) this goes to an expression that creates a new anonymous type
			//	manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, 
			//		(m, g) =>
			//			new
			//			{
			//				Manufacturer = m,
			//				Cars = g
			//			})
			//	.OrderBy(m => m.Manufacturer.Name);

			//------------------------------------------------------------------------------------------------------------------------------------
			//                                     Top 3 Fuel Efficient Cars By Country
			//------------------------------------------------------------------------------------------------------------------------------------

			var query =
				from manufacturer in manufacturers
				join car in cars on manufacturer.Name equals car.Manufacturer
					into carGroup
				select new
				{
					Manufacturer = manufacturer,
					Cars = carGroup
				} into result
				group result by result.Manufacturer.Headquarters;

			foreach (var group in query)
			{
				//to print out results a group operator takes all the cars places them in buckets that have a Key
				//the Key is the value that I grouped on(car.Manufacturer), inside that grouping are the cars that fall under that manufacturer
				//this will show how many cars per manufacturer, this is something extra
				//Console.WriteLine($"{group.Key} has {group.Count()}");

				//Console.WriteLine($"{group.Manufacturer.Name}:{group.Manufacturer.Headquarters}");
				//foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
				//{
				//	Console.WriteLine($"\t{car.Name} : {car.Combined}");
				//}

				Console.WriteLine($"{group.Key}");
				foreach (var car in group .SelectMany(g => g.Cars)
										  .OrderByDescending(c => c.Combined).Take(3))
				{
					Console.WriteLine($"\t{car.Name} : {car.Combined}");
				}
			}
		}

		//we make this method to build car objects from the csv file and put them in memory so we can ask questions about the cars
		private static List<Car> ProcessCars(string filePath)
		{
			//one way to use extension method where we create a custom extension method 
			var query =
				File.ReadAllLines(filePath)
				.Skip(1)
				//filter out empty lines "where l is a line, give me line where the line length is greater than 1"
				.Where(l => l.Length > 1)
				//-----------------------------------------------------------------------------------------------------------------------
				//given a line l invoke "ParseFromCsv" on that line 
				//this is one way to do it
				//.Select(l => Car.ParseFromCsv(l));
				//but we can also do it this way to make it more clear, this is a custom extension method where
				//we turn each line into a car directly
				//------------------------------------------------------------------------------------------------------------------------
				.ToCar();

			//----------------------------------------------------------------------------------------------------------------------------
			//extension method syntax to return a list of cars
			//return
			//	File.ReadAllLines(filePath)
			//		.Skip(1)
			//		.Where(line => line.Length > 1)
			//		.Select(Car.ParseFromCsv)
			//		.ToList();
			//
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

		public static List<Manufacturer> ProcessManufacturers(string filePath)
		{
			var query =
				File.ReadAllLines(filePath)
				.Where(l => l.Length > 1)
				.Select(l =>
				{
					var columns = l.Split(',');
					return new Manufacturer
					{
						Name = columns[0],
						Headquarters = columns[1],
						Year = int.Parse(columns[2])
					};
				});
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
