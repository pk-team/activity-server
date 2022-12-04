namespace App.Model;

public class Invoice : Entity {
    public int Year { get; set; }
    public int Number { get; set; }
    public DateTimeOffset Date { get; set; }

    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int RatePerHour { get; set; }
    public decimal SubTotal { get; set;  }
    public float VatPercent { get; set;  }
    public decimal Total { get; set;  }

    public Organization Customer { get; set; } = new Organization();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}