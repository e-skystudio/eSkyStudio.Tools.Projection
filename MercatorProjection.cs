using eSkyStudio.Tools.Projection.Models;
using GeographicLib.Projections;

namespace eSkyStudio.Tools.Projection;

public class MercatorProjection : AbstractProjection
{
    protected TransverseMercatorExact _projection;

    public MercatorProjection() : base()
    {
        _projection = new TransverseMercatorExact(earth, _scale);
    }

    public MercatorProjection(Coordinate reference, double scale = 0.005) : base(reference, scale)
    {
        _projection = new TransverseMercatorExact(earth, _scale);
    }

    protected override void Update()
    {
        _projection = new TransverseMercatorExact(earth, _scale);
    }

    public override Point WorldToLocal(Coordinate source)
    {
        (double x, double y) = _projection.Forward(_refLongitude, source.Latitude, source.Longitude);
        y *= -1.0; //Screen is Y positive;
        Point local = new Point((int)x, (int)y);
        Point halfScreen = new(_screenSize.Width / 2, _screenSize.Height / 2);
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
