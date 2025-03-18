using GoldSavings.App.Model;
using GoldSavings.App.Client;
using System.Xml.Linq;
namespace GoldSavings.App;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Saver!");

        GoldClient goldClient = new GoldClient();

        GoldPrice currentPrice = goldClient.GetCurrentGoldPrice().GetAwaiter().GetResult();
        Console.WriteLine($"The price for today is {currentPrice.Price}");

        // task A
        DateTime oneYearAgo = DateTime.Now.AddYears(-1);
        DateTime today = DateTime.Now;

        List<GoldPrice> lastYearPrices = goldClient.GetGoldPrices(oneYearAgo, today).GetAwaiter().GetResult();

        var top3HighestPricesQuery = (from price in lastYearPrices
                                    orderby price.Price descending
                                    select price).Take(3);

        var top3LowestPricesQuery = (from price in lastYearPrices
                                    orderby price.Price ascending
                                    select price).Take(3);

        Console.WriteLine("Top 3 highest prices:");
        foreach (var price in top3HighestPricesQuery)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }

        Console.WriteLine("Top 3 lowest prices:");
        foreach (var price in top3LowestPricesQuery)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }

        // task B
        DateTime january2020Start = new DateTime(2020, 1, 1);
        DateTime january2020End = new DateTime(2020, 1, 31);

        List<GoldPrice> january2020Prices = goldClient.GetGoldPrices(january2020Start, january2020End).GetAwaiter().GetResult();

        var bestDayInJanuary2020 = (from price in january2020Prices
                                    orderby price.Price descending
                                    select price).Take(1).FirstOrDefault();

        if (bestDayInJanuary2020 != null)
        {
            List<GoldPrice> allPricesSince2020 = goldClient.GetGoldPrices(january2020Start, DateTime.Now).GetAwaiter().GetResult();

            var profitableDays = (from laterPrice in allPricesSince2020
                                where laterPrice.Price > bestDayInJanuary2020.Price * 1.05
                                select new
                                {
                                    OriginalDate = bestDayInJanuary2020.Date,
                                    LaterDate = laterPrice.Date,
                                    OriginalPrice = bestDayInJanuary2020.Price,
                                    LaterPrice = laterPrice.Price,
                                    ProfitPercentage = ((laterPrice.Price - bestDayInJanuary2020.Price) / bestDayInJanuary2020.Price) * 100
                                }).Take(10);

            Console.WriteLine("Days with more than 5% profit from the best day in January 2020:");
            foreach (var day in profitableDays)
            {
                Console.WriteLine($"Bought on: {day.OriginalDate}, Price: {day.OriginalPrice}");
                Console.WriteLine($"If sold on: {day.LaterDate}, Price: {day.LaterPrice}");
                Console.WriteLine($"Profit: {day.ProfitPercentage:F2}%");
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("No data available for January 2020.");
        }

        // task C
        DateTime startDate = new DateTime(2019, 1, 1);
        DateTime endDate = new DateTime(2022, 12, 31);

        List<GoldPrice> allPrices = goldClient.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();

        var secondTenDates = (from price in allPrices
                           orderby price.Price descending
                           select price)
                          .Skip(10)
                          .Take(3);

        Console.WriteLine("Dates opening the second ten of the prices ranking:");
        foreach (var entry in secondTenDates)
        {
            Console.WriteLine($"Date: {entry.Date}, Price: {entry.Price}");
        }

        // task D
        DateTime startDate2020 = new DateTime(2020, 1, 1);
        DateTime endDate2020 = new DateTime(2020, 12, 31);

        DateTime startDate2023 = new DateTime(2023, 1, 1);
        DateTime endDate2023 = new DateTime(2023, 12, 31);

        DateTime startDate2024 = new DateTime(2024, 1, 1);
        DateTime endDate2024 = new DateTime(2024, 12, 31);

        List<GoldPrice> prices2020 = goldClient.GetGoldPrices(startDate2020, endDate2020).GetAwaiter().GetResult();
        List<GoldPrice> prices2023 = goldClient.GetGoldPrices(startDate2023, endDate2023).GetAwaiter().GetResult();
        List<GoldPrice> prices2024 = goldClient.GetGoldPrices(startDate2024, endDate2024).GetAwaiter().GetResult();

        var average2020 = (from price in prices2020 
                           select price.Price).Average();

        var average2023 = (from price in prices2023
                           select price.Price).Average();

        var average2024 = (from price in prices2024
                           select price.Price).Average();

        Console.WriteLine("\nAverage prices:");
        Console.WriteLine($"Average price in 2020: {average2020:F2}");
        Console.WriteLine($"Average price in 2023: {average2023:F2}");
        Console.WriteLine($"Average price in 2024: {average2024:F2}");

        // task E
        DateTime startDate2020v2 = new DateTime(2020, 1, 1);
        DateTime endDate2024v2 = new DateTime(2024, 12, 31);

        List<GoldPrice> prices2020to2024 = goldClient.GetGoldPrices(startDate2020v2, endDate2024v2).GetAwaiter().GetResult();

        var minPrice = (from price in prices2020to2024
                        orderby price.Price ascending
                        select price).Take(1).FirstOrDefault();

        var maxPrice = (from price in prices2020to2024
                        where price.Date >= minPrice.Date
                        orderby price.Price descending
                        select price).Take(1).FirstOrDefault();

        if (minPrice != null && maxPrice != null)
        {
            Console.WriteLine("\nBest investment:");
            Console.WriteLine($"Bought on: {minPrice.Date}, Price: {minPrice.Price}");
            Console.WriteLine($"Sold on: {maxPrice.Date}, Price: {maxPrice.Price}");
            Console.WriteLine($"Return on investment: {((maxPrice.Price - minPrice.Price) / minPrice.Price) * 100:F2}%");
        }
        else
        {
            Console.WriteLine("No data.");
        }

        // task 3
        DateTime startDateLastMonth = new DateTime(2022, 1, 1);
        DateTime endDateLastMonth = new DateTime(2022, 1, 11);

        List<GoldPrice> oneMonthPrices = goldClient.GetGoldPrices(startDateLastMonth, endDateLastMonth).GetAwaiter().GetResult();

        XDocument doc = new XDocument(
            new XElement("Prices",
                oneMonthPrices.Select(price =>
                    new XElement("Price",
                        new XElement("Date", price.Date),
                        new XElement("Price", price.Price)
                    )
                )
            )
        );

        doc.Declaration = new XDeclaration("1.0", "utf-8", "true");
        doc.Save("Prices.xml");

        // task 4
        var prices = from elements in XElement.Load("Prices.xml").Elements("Prices") from price in elements.Elements("Price") select price;

        foreach (var price in prices)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }
        // Default implementation
        // List<GoldPrice> thisMonthPrices = goldClient.GetGoldPrices(oneYearAgo, today).GetAwaiter().GetResult();
        // foreach(var goldPrice in thisMonthPrices)
        // {
        //     Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        // }

    }
}
