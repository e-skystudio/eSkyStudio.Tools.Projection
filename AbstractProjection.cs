using eSkyStudio.Tools.Projection.Models;
using GeographicLib;

namespace eSkyStudio.Tools.Projection;

public abstract class AbstractProjection : IProjection
{
    protected static IEllipsoid earth = Ellipsoid.WGS84;

    protected double _refLatitude;
    protected double _refLongitude;
    protected double _scale;
    protected Size _screenSize;

    public AbstractProjection() : this(new Coordinate(0.0, 0.0), 0.005)
    {
    }

    public AbstractProjection(Coordinate reference, double scale)
    {
        _refLatitude = reference.Latitude;
        _refLongitude = reference.Longitude;
        _scale = scale;
        _screenSize = new(1000.0, 1000.0);
    }

    public abstract Coordinate LocalToWorld(Point screenPosition);
    public abstract Point WorldToLocal(Coordinate source);
    protected abstract void Update();

    public void SetReferencePoint(Coordinate referencePoint)
    {
        _refLatitude = referencePoint.Latitude;
        _refLongitude = referencePoint.Longitude;
        Update();
    }

    public void SetScale(double scale)
    {
        _scale = scale;
        Update();
    }

    public void SetScreenSize(Size screenSize)
    {
        _screenSize = screenSize;
    }


}
