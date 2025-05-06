using System.Drawing;
using eSkyStudio.NavigationDatabase.Models;
using GeographicLib;

namespace eSkyStudio.Tools.Projection;

public abstract class AbstractProjection(Coordinate reference, double scale) : IProjection
{
    protected static readonly IEllipsoid Earth = Ellipsoid.WGS84;

    protected double RefLatitude = reference.Latitude;
    protected double RefLongitude = reference.Longitude;
    protected double Scale = scale;
    protected SizeF ScreenSize = new(1000.0f, 1000.0f);

    protected AbstractProjection() : this(new Coordinate(0.0, 0.0), 0.005)
    {
    }

    public abstract Coordinate LocalToWorld(PointF screenPosition);
    public abstract PointF WorldToLocal(Coordinate source);
    protected abstract void Update();

    public void SetReferencePoint(Coordinate referencePoint)
    {
        RefLatitude = referencePoint.Latitude;
        RefLongitude = referencePoint.Longitude;
        Update();
    }

    public void SetScale(double scale)
    {
        Scale = scale;
        Update();
    }

    public void SetScreenSize(SizeF screenSize)
    {
        ScreenSize = screenSize;
    }


}
