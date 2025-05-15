using System.Drawing;
using eSkyStudio.NavigationDatabase.Models;
using eSkyStudio.Tools.Projection.Models;


namespace eSkyStudio.Tools.Projection;

public interface IProjection
{
    public PointF WorldToLocal(Coordinate source);
    public Coordinate LocalToWorld(PointF screenPosition);
    
    public Coordinate Center {get; set;}
    public double Scale {get; set;}
    public SizeF ScreenSize {get; set;}
}
