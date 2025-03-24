using System;
using System.Collections.Generic;

public class RandomizedList<T>
{
    private List<T> _data;
    private Random _random;

    public RandomizedList()
    {
        _data = new List<T>();
        _random = new Random();
    }

    public void Add(T element)
    {
        if (_random.Next(2) == 0)
            _data.Insert(0, element);
        else
            _data.Add(element);
    }

    public T Get(int index)
    {
        if (_data.Count == 0 || index < 0 || index >= _data.Count)
            throw new IndexOutOfRangeException("Index out of range or list is empty.");

        return _data[_random.Next(0, Math.Min(index + 1, _data.Count))];
    }

    public bool IsEmpty()
    {
        return _data.Count == 0;
    }
}
class Program
{
    static void Main()
    {
        Func<int, bool> isLeapYear = year => (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
        
        Console.WriteLine($"2024 is a leap year: {isLeapYear(2024)}");
        Console.WriteLine($"1900 is a leap year: {isLeapYear(1900)}");
        Console.WriteLine($"2000 is a leap year: {isLeapYear(2000)}");
        
        RandomizedList<int> randList = new RandomizedList<int>();
        randList.Add(10);
        randList.Add(20);
        randList.Add(30);
        
        Console.WriteLine("Randomly selected element: " + randList.Get(2));
        Console.WriteLine("Is list empty? " + randList.IsEmpty()); 
    }
}


