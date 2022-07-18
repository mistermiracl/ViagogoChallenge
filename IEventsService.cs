interface IEventsService
{
    int GetDistance(string fromCity, string toCity);
    void AddToEmail(Customer c, Event e);
    int GetPrice(Event e);
}