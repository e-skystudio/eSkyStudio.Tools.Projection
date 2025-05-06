using System.Drawing;
using eSkyStudio.NavigationDatabase.Models;
using eSkyStudio.Tools.Projection.Models;


namespace eSkyStudio.Tools.Projection;

public interface IProjection
{
    public PointF WorldToLocal(Coordinate source);
    public Coordinate LocalToWorld(PointF screenPosition);

    public void SetReferencePoint(Coordinate referencePoint);
    public void SetScale(double scale);
    public void SetScreenSize(SizeF screenSize);
}
