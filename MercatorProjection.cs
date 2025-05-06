using System.Diagnostics;
using System.Drawing;
using eSkyStudio.NavigationDatabase.Models;
using GeographicLib.Projections;

namespace eSkyStudio.Tools.Projection;

public class MercatorProjection : AbstractProjection
{
    private TransverseMercatorExact? _projection;

    public MercatorProjection() : base()
    {
        _projection = new TransverseMercatorExact(Earth, Scale);
    }

    public MercatorProjection(Coordinate reference, double scale = 0.005) : base(reference, scale)
    {
        _projection = new TransverseMercatorExact(Earth, Scale);
    }

    protected override void Update()
    {
        _projection = new TransverseMercatorExact(Earth, Scale);
    }

    public override PointF WorldToLocal(Coordinate source)
    {
        if (_projection == null)
        {
            Debug.WriteLine("MercatorProjection: projection is null");
            return PointF.Empty;
        }
        (double x, double y) = _projection.Forward(RefLongitude, source.Latitude, source.Longitude);
        y *= -1.0; //Screen is Y positive;
        PointF local = new PointF((int)x, (int)y);
        PointF halfScreen = new(ScreenSize.Width / 2, ScreenSize.Height / 2);
        local.X += halfScreen.X;
        local.Y += halfScreen.Y;
        return local;
    }

    public override Coordinate LocalToWorld(PointF screenPosition)
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
        (double lat, double lon) = _projection.Reverse(RefLongitude, screenPosition.X, screenPosition.Y);
        return new Coordinate(lat, lon);
    }
}
