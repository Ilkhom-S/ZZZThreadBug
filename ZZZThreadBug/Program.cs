
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;
using ZZZThreadBug;

public class Program
{
    private static void Main(string[] args)
    {
        using var db = new AppDbContext();

        // Insert dummy data
        db.Add(new SampleTemporal { Name = "Test" });
        db.Add(new SampleTemporal { Name = "Test" });
        db.SaveChanges();

        // Query database with standart ef core temporal extensions, with default culture 
        var sEfQuery = db.SampleTemporals.TemporalAsOf(DateTime.UtcNow);
        Console.WriteLine(sEfQuery.Count());

        // Query database with ZZZ extensions, with default thread culture
        // Explain: Commented this code, because extension can't query hook more than once.
        //
        //var zQueryDefaultculture = db.SampleTemporals.TemporalTableAsOf(DateTime.UtcNow);
        //Console.WriteLine(zQueryDefaultculture.Count());

        // Change Thread culture to ru-RU
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

        // Same query with standart ef core temporal extensions, with ru-RU culture
        var sQueryDiffCulture = db.SampleTemporals.TemporalAsOf(DateTime.UtcNow);
        Console.WriteLine(sQueryDiffCulture.Count());

        // Now querying data with zzz extensions, throws datetime error
        var zQueryDiffCulture = db.SampleTemporals.TemporalTableAsOf(DateTime.UtcNow);
        Console.WriteLine("Should show count of rows, but throws error: {0}", zQueryDiffCulture.Count());
    }
}