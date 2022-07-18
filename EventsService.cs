class EventsService : IEventsService
{
    private int AlphebiticalDistance(string s, string t)
    {
        var result = 0;
        var i = 0;
        for (i = 0; i < Math.Min(s.Length, t.Length); i++)
        {
            result += Math.Abs(s[i] - t[i]);
        }
        for (; i < Math.Max(s.Length, t.Length); i++)
        {
            result += s.Length > t.Length ? s[i] : t[i];
        }
        return result;
    }

    public int GetDistance(string fromCity, string toCity)
    {
        return AlphebiticalDistance(fromCity, toCity);
    }
    
    public void AddToEmail(Customer c, Event e)
    {
        var distance = GetDistance(c.City, e.City);
        var price = GetPrice(e);
        Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
        + (distance > 0 ? $" ({distance} miles away)" : "")
        + ($" for ${price}"));
    }

    public int GetPrice(Event e)
    {
        return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
    }
}