using System;
using System.Collections.Generic;

public class Car
{
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string VIN { get; set; }
    public decimal Price { get; set; }

    // Destructor (not often needed but included for completeness)
    ~Car()
    {
        // Cleanup resources if necessary
    }

    public override string ToString()
    {
        return $"{Year} {Make} {Model}, VIN: {VIN}, Price: {Price:C}";
    }
}

public class Dealership
{
    private static List<Car> inventory = new List<Car>();
    public static int TotalCars => inventory.Count;

    public void AddCar(string make, string model, int year, string vin, decimal price)
    {
        AddCar(new Car { Make = make, Model = model, Year = year, VIN = vin, Price = price });
    }

    public void AddCar(string make, string model, int year, string vin, decimal price, string[] optionalFeatures)
    {
        var car = new Car { Make = make, Model = model, Year = year, VIN = vin, Price = price };
        Console.WriteLine("Optional Features: " + string.Join(", ", optionalFeatures));
        AddCar(car);
    }

    private void AddCar(Car car)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(car.VIN))
                throw new ArgumentNullException(nameof(car.VIN), "VIN cannot be null or empty.");

            if (inventory.Exists(c => c.VIN == car.VIN))
                throw new ArgumentException("A car with this VIN already exists.");

            inventory.Add(car);
            Console.WriteLine($"Car added: {car}");
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw; // Rethrow the exception for logging purposes if needed
        }
    }

    public static void DisplayInventory()
    {
        Console.WriteLine("Dealership Inventory:");
        if (inventory.Count == 0)
        {
            Console.WriteLine("No cars in inventory.");
            return;
        }
        foreach (var car in inventory)
        {
            Console.WriteLine(car);
        }
    }

    public Car FindCarByVIN(string vin)
    {
        return FindCarByVIN(vin, 0);
    }

    private Car FindCarByVIN(string vin, int index)
    {
        if (index >= inventory.Count) return null;
        if (inventory[index].VIN == vin) return inventory[index];
        return FindCarByVIN(vin, index + 1);
    }
}

class Program
{
    static void Main()
    {
        Dealership dealership = new Dealership();

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Add a Car");
            Console.WriteLine("2. View Inventory");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();
            if (choice == "3") break;

            switch (choice)
            {
                case "1":
                    // Adding a Car
                    Console.Write("Enter Make: ");
                    string make = Console.ReadLine();

                    Console.Write("Enter Model: ");
                    string model = Console.ReadLine();

                    Console.Write("Enter Year: ");
                    if (!int.TryParse(Console.ReadLine(), out int year))
                    {
                        Console.WriteLine("Invalid year. Please enter a valid integer.");
                        continue;
                    }

                    Console.Write("Enter VIN: ");
                    string vin = Console.ReadLine();

                    Console.Write("Enter Price in Rands: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal priceInRands))
                    {
                        Console.WriteLine("Invalid price. Please enter a valid decimal number.");
                        continue;
                    }

                    dealership.AddCar(make, model, year, vin, priceInRands);
                    break;

                case "2":
                    // Viewing Inventory
                    Dealership.DisplayInventory();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }
    }
}
