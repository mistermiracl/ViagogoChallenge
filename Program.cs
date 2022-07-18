using System.Collections.Generic;

class Program
{
    private static readonly IEventsService eventsService = new EventsService();
    private static readonly IDictionary<string, int> distances = new Dictionary<string, int>();
    private const int maxRetries = 3;

    private static void Main(string[] args)
    {
        // Events
        var events = new List<Event>
        {
            new Event{ Name = "Phantom of the Opera", City = "New York"},
            new Event{ Name = "Metallica", City = "Los Angeles"},
            new Event{ Name = "Metallica", City = "New York"},
            new Event{ Name = "Metallica", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "New York"},
            new Event{ Name = "LadyGaGa", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "Chicago"},
            new Event{ Name = "LadyGaGa", City = "San Francisco"},
            new Event{ Name = "LadyGaGa", City = "Washington"}
        };
        // Customer
        var customer = new Customer { Name = "Mr. Fake", City = "New York" };
        SendEvents(customer, events);
    }

    private static void SendEvents(Customer customer, List<Event> events)
    {
        // 1.
        Console.WriteLine("Events in city: ");
        events.ForEach(e =>
        {
            if(customer.City == e.City) eventsService.AddToEmail(customer, e);
        });
        // 2.
        // 3.
        // 4.
        Console.WriteLine("Closest 5 events outiside of city: ");
        var closest5Events = events
            .Select(e =>
            {
                var key = e.City.CompareTo(customer.City) > 0 ? customer.City + e.City : e.City + customer.City;
                int? distance = null;
                if(e.City != customer.City)
                {
                    if(distances.ContainsKey(key))
                    {
                        distance = distances[key];
                    }
                    else
                    {
                        var attempts = 0;
                        while(distance == null && attempts < maxRetries)
                        {
                            try
                            {
                                distance = eventsService.GetDistance(customer.City, e.City);
                            } catch
                            {
                                attempts++;
                            }
                        }
                        if(distance != null) distances[key] = distance.Value;
                    }
                }
                return new { e, distance };
            })
            .Where(e => e.distance != null)
            .OrderBy(e => e.distance)
            .Take(5)
            .Select(e => e.e);
        foreach (var e in closest5Events)
        {
            eventsService.AddToEmail(customer, e);
        }
        // 5.
        Console.WriteLine("Cheapest 5 events: ");
        var cheapest5Events = events
            .OrderBy(eventsService.GetPrice)
            .Take(5);
        foreach (var e in cheapest5Events)
        {
            eventsService.AddToEmail(customer, e);
        }
    }

}
