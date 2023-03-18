using eSkyStudio.Tools.Projection.Models;
using GeographicLib.Projections;

namespace eSkyStudio.Tools.Projection;

public class PolarProjection : AbstractProjection
{
    protected PolarStereographic _projection;

    public PolarProjection() : base()
    {
        _projection = new PolarStereographic(earth, _scale);
    }

    public PolarProjection(Coordinate reference, double scale = 0.005) : base(reference, scale)
    {
        _projection = new PolarStereographic(earth, _scale);
    }

    protected override void Update()
    {
        _projection = new PolarStereographic(earth, _scale);
    }

    public override Coordinate LocalToWorld(Point screenPosition)
    {
        bool NorthPole = _refLatitude > 0;
        Point halfScreen = new(_screenSize.Width / 2, _screenSize.Height / 2);
        screenPosition.X -= halfScreen.X;
        screenPosition.Y -= halfScreen.Y;
        screenPosition.Y = screenPosition.Y * -1;
        (double lat, double lon) = _projection.Reverse(NorthPole, screenPosition.X, screenPosition.Y);
        return new Coordinate(lat, lon);
    }

    public override Point WorldToLocal(Coordinate source)
    {
        bool NorthPole = _refLatitude > 0;
        (double x, double y) = _projection.Forward(NorthPole, source.Latitude, source.Longitude);
        y *= -1.0; //Screen is Y positive;
        Point local = new Point((int)x, (int)y);
        Point halfScreen = new(_screenSize.Width / 2, _screenSize.Height / 2);
        local.X += halfScreen.X;
        local.Y += halfScreen.Y;
        return local;
    }
}
