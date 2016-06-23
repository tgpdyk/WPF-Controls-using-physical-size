using RadialMenuControl.Helpers;

namespace RadialMenuControl.Common
{
    public class Sector
    {
        public Sector(double radius, System.Windows.Point center, double angle1, double angle2, double radiusOffset = 0d, double angleOffset = 0d)
        {
            Center = center;
            Radius = radius - radiusOffset;
            AngleStart = angle1 + angleOffset;
            AngleEnd = angle2 - angleOffset;
            PointStart = GeometryHelper.CalculatePoint(Center.X, Center.Y, AngleStart, Radius);
            PointEnd = GeometryHelper.CalculatePoint(Center.X, Center.Y, AngleEnd, Radius);
            Size = new System.Windows.Size(Radius, Radius);
            Rotation = AngleEnd - AngleStart;
            IsLargeArc = Rotation > Helpers.GeometryHelper.HalfCircle;
        }

        public double Radius { get; set; }
        public double AngleStart { get; set; }
        public double AngleEnd { get; set; }
        public double Rotation { get; set; }
        public bool IsLargeArc { get; set; }

        public double TotalAngle { get { return AngleEnd - AngleStart; }}

        public System.Windows.Size Size { get; }

        public System.Windows.Point Center { get; }

        public System.Windows.Point PointStart { get; }

        public System.Windows.Point PointEnd { get; }
    }
}
