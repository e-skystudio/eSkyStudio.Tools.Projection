using GeographicLib;
using UnitsNet;
using UnitsNet.Units;

namespace eSkyStudio.Tools.Projection.Models;

public class Coordinate
{ 
    public static IEllipsoid earth = Ellipsoid.WGS84;
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Coordinate()
    {
        Latitude = double.MinValue;
        Longitude = double.MinValue;
    }

    public Coordinate(double lat, double lon)
    {
        Latitude = lat; 
        Longitude = lon;
    }

    public Length Distance(Coordinate target)
    {
        Geodesic.WGS84.Inverse(Latitude, Longitude, target.Latitude, target.Longitude, out double distM);
        return Length.FromMeters(distM);
    }

    public double BearingStart(Coordinate target)
    {
        Geodesic.WGS84.Inverse(Latitude, Longitude, target.Latitude, target.Longitude, out double distM, out double az1, out double _);
        return az1;
    }

    public double BearingEnd(Coordinate target)
    {
        Geodesic.WGS84.Inverse(Latitude, Longitude, target.Latitude, target.Longitude, out double distM, out double _, out double az2);
        return az2;
    }

    public override string ToString()
    {
        return $"Latitude : {Latitude}°N; Longitude : {Longitude}°E";
    }
}


public class Coordinate3D : Coordinate
{
    public Length Elevation { get; set; }

    public Coordinate3D() : base() {
        Elevation = Length.FromMeters(double.MinValue);
    }

    public Coordinate3D(double lat, double lon, double elevation = 0.0, LengthUnit unit = LengthUnit.Meter) : base(lat, lon)
    {
            Elevation = Length.From(elevation, unit);
    }

    public double SlopePercent(Coordinate3D target)
    {
        var dist = Distance(target).Meters;
        var ele = Elevation.Meters - target.Elevation.Meters;
        return ele / dist;
    }

    public double SlopeDegrees(Coordinate3D target)
    {
        var slope = SlopePercent(target);
        return Math.Atan(slope);
    }

    public override string ToString()
    {
        return $"{base.ToString()}; Elevation : {Elevation.Meters} m";
    }
}
