using System.Diagnostics;
using System.Drawing;
using eSkyStudio.NavigationDatabase.Models;
using GeographicLib;
using GeographicLib.Projections;

namespace eSkyStudio.Tools.Projection;

public class PolarProjection(Coordinate reference, double scale = 0.005) : IProjection
{
    private static readonly IEllipsoid Earth = Ellipsoid.WGS84;
    private Coordinate _center = reference;
    public Coordinate Center
    {
        get => _center;
        set
        {
            _center = value;
            Update();
        }
    }

    public double Scale { get; set; } = scale;
    public SizeF ScreenSize { get; set; }

    private PolarStereographic? _projection;

    protected void Update()
    {
        _projection = new PolarStereographic(Earth, Scale);
    }

    public Coordinate LocalToWorld(PointF screenPosition)
    {
        if (_projection == null)
        {
            Debug.WriteLine("PolarProjection: projection is null");
            return new Coordinate(0.0f, 0.0f);
        }
        bool northPole = Center.Latitude > 0;
        PointF halfScreen = new(ScreenSize.Width / 2.0f, ScreenSize.Height / 2.0f);
        screenPosition.X -= halfScreen.X;
        screenPosition.Y -= halfScreen.Y;
        screenPosition.Y = screenPosition.Y * -1;
        (double lat, double lon) = _projection.Reverse(northPole, screenPosition.X, screenPosition.Y);
        return new Coordinate(lat, lon);
    }

    public PointF WorldToLocal(Coordinate source)
    {
        if (_projection == null)
        {
            Debug.WriteLine("PolarProjection: projection is null");
            return PointF.Empty;
        }
        bool northPole = Center.Latitude > 0;
        (double x, double y) = _projection.Forward(northPole, source.Latitude, source.Longitude);
        y *= -1.0; //Screen is Y positive;
        PointF local = new PointF((float)x, (float)y);
        PointF halfScreen = new(ScreenSize.Width / 2.0f, ScreenSize.Height / 2.0f);
        local.X += halfScreen.X;
        local.Y += halfScreen.Y;
        return local;
    }
}
