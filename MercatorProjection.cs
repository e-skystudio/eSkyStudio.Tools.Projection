using System.Diagnostics;
using System.Drawing;
using eSkyStudio.NavigationDatabase.Models;
using GeographicLib;
using GeographicLib.Projections;

namespace eSkyStudio.Tools.Projection;

public class MercatorProjection(Coordinate reference, double scale = 0.005) : IProjection
{
    private static readonly IEllipsoid Earth = Ellipsoid.WGS84;
    private TransverseMercatorExact? _projection;
    public SizeF ScreenSize { get; set; }
    public double Scale { get; set; } = scale;
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

    private void Update()
    {
        _projection = new TransverseMercatorExact(Earth, Scale);
    }

    public PointF WorldToLocal(Coordinate source)
    {
        if (_projection == null)
        {
            Debug.WriteLine("MercatorProjection: projection is null");
            return PointF.Empty;
        }
        (double x, double y) = _projection.Forward(Center.Longitude, source.Latitude, source.Longitude);
        y *= -1.0; //Screen is Y positive;
        PointF local = new PointF((int)x, (int)y);
        PointF halfScreen = new(ScreenSize.Width / 2, ScreenSize.Height / 2);
        local.X += halfScreen.X;
        local.Y += halfScreen.Y;
        return local;
    }

    public Coordinate LocalToWorld(PointF screenPosition)
    {
        if (_projection == null)
        {
            Debug.WriteLine("MercatorProjection: projection is null");
            return new Coordinate();
        }
        PointF halfScreen = new(ScreenSize.Width / 2.0f, ScreenSize.Height / 2.0f);
        screenPosition.X -= halfScreen.X;
        screenPosition.Y -= halfScreen.Y;
        screenPosition.Y = screenPosition.Y * -1;
        (double lat, double lon) = _projection.Reverse(Center.Longitude, screenPosition.X, screenPosition.Y);
        return new Coordinate(lat, lon);
    }


}
