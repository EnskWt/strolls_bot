using System;
using System.Collections.Generic;

namespace strolls_bot.Database;

public partial class TrackLocation
{
    public int Id { get; set; }

    public string Imei { get; set; } = null!;

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public DateTime DateEvent { get; set; }

    public DateTime DateTrack { get; set; }

    public int TypeSource { get; set; }

    public double TotalTime { get; set; }

    public double TotalDistance { get; set; }
}
