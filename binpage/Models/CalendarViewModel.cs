namespace binpage.Models;

using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System.Linq;

public class CalendarViewModel
{
    public DateTime? Today;
    public IList<CalendarEvent> AllUpcomingDates = [];
    public IList<string> TomorrowBins = [];
    
    public CalendarViewModel(DateTime date)
    {
        Today = date;
        DateOnly calDate = new CalDateTime(date).Date;

        using StreamReader reader = new ("BinDateFile.ICS");

        var calendar = Calendar.Load(reader);
        var events = from eventItem in calendar?.Events
                    where eventItem.DtStart?.Date < calDate.AddDays(30)
                    orderby eventItem.DtStart
                    select eventItem;

        ((List<CalendarEvent>)AllUpcomingDates).AddRange(events);

        var tomorrowEvents = from eventItem in events
                    where eventItem.DtStart?.Date == calDate.AddDays(1)
                    select eventItem;

        foreach (CalendarEvent eventItem in tomorrowEvents)
        {
            if (eventItem.Summary?.Contains("HIS Restmüll, w") == true)
            {
                ((List<string>)TomorrowBins).Add("#000000");
            }
            else if (eventItem.Summary?.Contains("HIS Restmüll, 2") == true)
            {
                ((List<string>)TomorrowBins).Add("#FF0000");
            }
            else if (eventItem.Summary?.Contains("HIS P") == true)
            {
                ((List<string>)TomorrowBins).Add("#0000FF");
            }
            else if (eventItem.Summary?.Contains("HIS B") == true)
            {
                ((List<string>)TomorrowBins).Add("#996622");
            }
            else if (eventItem.Summary?.Contains("HIS G") == true)
            {
                ((List<string>)TomorrowBins).Add("#FFFF00");
            }
        }
    }
}