using eSkyStudio.Tools.Projection.Models;
using GeographicLib.Projections;

namespace eSkyStudio.Tools.Projection;

public class LambertProjection : AbstractProjection
{
    protected LambertConformalConic _projection;

    public LambertProjection() : base()
    {
        _projection = new LambertConformalConic(earth, _refLatitude, _scale);
    }

    public LambertProjection(Coordinate reference, double scale = 0.005) : base(reference, scale)
    {
        _projection = new LambertConformalConic(earth, _refLatitude, _scale);
    }

    protected override void Update()
    {
        _projection = new LambertConformalConic(earth, _refLatitude, _scale);
    }


    public override Point WorldToLocal(Coordinate source)
    {
        (double x, double y) = _projection.Forward(_refLongitude, source.Latitude, source.Longitude);
        y *= -1.0; //Screen is Y positive;
        Point local = new Point((int)x, (int)y);
        Point halfScreen = new(_screenSize.Width / 2.0, _screenSize.Height / 2.0);
        local.X += halfScreen.X;
        local.Y += halfScreen.Y;
        return local;
    }

    public override Coordinate LocalToWorld(Point screenPosition)
    {
        Point halfScreen = new(_screenSize.Width / 2, _screenSize.Height / 2);
        screenPosition.X -= halfScreen.X;
        screenPosition.Y -= halfScreen.Y;
        screenPosition.Y = screenPosition.Y * -1;
        (double lat, double lon) = _projection.Reverse(_refLongitude, screenPosition.X, screenPosition.Y);
        return new Coordinate(lat, lon);

    }
}
