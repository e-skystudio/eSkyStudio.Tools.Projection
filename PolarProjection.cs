using System.Diagnostics;
using System.Drawing;
using eSkyStudio.NavigationDatabase.Models;
using GeographicLib.Projections;

namespace eSkyStudio.Tools.Projection;

public class PolarProjection : AbstractProjection
{
    private PolarStereographic? _projection;

    public PolarProjection() : base()
    {
        _projection = new PolarStereographic(Earth, Scale);
    }

    public PolarProjection(Coordinate reference, double scale = 0.005) : base(reference, scale)
    {
        _projection = new PolarStereographic(Earth, Scale);
    }

    protected override void Update()
    {
        _projection = new PolarStereographic(Earth, Scale);
    }

    public override Coordinate LocalToWorld(PointF screenPosition)
    {
        if (_projection == null)
        {
            Debug.WriteLine("PolarProjection: projection is null");
            return new Coordinate(0.0f, 0.0f);
        }
        bool NorthPole = RefLatitude > 0;
        PointF halfScreen = new(ScreenSize.Width / 2.0f, ScreenSize.Height / 2.0f);
        screenPosition.X -= halfScreen.X;
        screenPosition.Y -= halfScreen.Y;
        screenPosition.Y = screenPosition.Y * -1;
        (double lat, double lon) = _projection.Reverse(NorthPole, screenPosition.X, screenPosition.Y);
        return new Coordinate(lat, lon);
    }

    public override PointF WorldToLocal(Coordinate source)
    {
        if (_projection == null)
        {
            Debug.WriteLine("PolarProjection: projection is null");
            return PointF.Empty;
        }
        bool NorthPole = RefLatitude > 0;
        (double x, double y) = _projection.Forward(NorthPole, source.Latitude, source.Longitude);
        y *= -1.0; //Screen is Y positive;
        PointF local = new PointF((float)x, (float)y);
        PointF halfScreen = new(ScreenSize.Width / 2.0f, ScreenSize.Height / 2.0f);
        local.X += halfScreen.X;
        local.Y += halfScreen.Y;
        return local;
    }
}
