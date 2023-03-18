using eSkyStudio.Tools.Projection.Models;

namespace eSkyStudio.Tools.Projection;

public interface IProjection
{
    public Point WorldToLocal(Coordinate source);
    public Coordinate LocalToWorld(Point screenPosition);

    public void SetReferencePoint(Coordinate referencePoint);
    public void SetScale(double scale);
    public void SetScreenSize(Size screenSize);
}
