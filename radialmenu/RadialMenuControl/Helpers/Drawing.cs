using System.Windows.Media;

namespace RadialMenuControl.Helpers
{
    internal static class Drawing
    {
        public static void DrawLineFromCenter(DrawingContext dc, System.Windows.Media.Brush brush, double thickness, System.Windows.Point p1, System.Windows.Point p2)
        {
            PathFigure line1PathFigure = new PathFigure();
            line1PathFigure.Segments = new PathSegmentCollection();
            line1PathFigure.StartPoint = p1;

            LineSegment line1 = new LineSegment(p2, true);
            line1PathFigure.Segments.Add(line1);

            PathGeometry line1Geometry = new PathGeometry();
            line1Geometry.Figures.Add(line1PathFigure);
            dc.DrawGeometry(null, new System.Windows.Media.Pen(brush, thickness), line1Geometry);
        }


        public static void DrawArc(DrawingContext dc, System.Windows.Point centerPoint, ArcItemData item)
        {
            var center = centerPoint;
            var point1 = item.Angel1;
            var point2 = item.Angle2;

            if (item.OuterRadius < 0 || item.InnerRadius < 0) return;

            System.Windows.Point arcP1 = GeometryHelper.CalculatePoint(center.X, center.Y, point1, item.OuterRadius);
            System.Windows.Point arcP2 = GeometryHelper.CalculatePoint(center.X, center.Y, point2, item.OuterRadius);
            System.Windows.Point arcP3 = GeometryHelper.CalculatePoint(center.X, center.Y, point1, item.InnerRadius);
            System.Windows.Point arcP4 = GeometryHelper.CalculatePoint(center.X, center.Y, point2, item.InnerRadius);

            PathFigure archPathFigure = new PathFigure();
            archPathFigure.Segments = new PathSegmentCollection();
            archPathFigure.StartPoint = arcP1;

            double rotation = point2 - point1;
            
            var outerArc = new ArcSegment(arcP2, new System.Windows.Size(item.OuterRadius, item.OuterRadius), rotation - point1, rotation > 180, SweepDirection.Clockwise, true);
            var line = new LineSegment(arcP4, true);
            var innerArc = new ArcSegment(arcP3, new System.Windows.Size(item.InnerRadius, item.InnerRadius), rotation - point1, rotation > 180, SweepDirection.Counterclockwise, true);

            archPathFigure.Segments.Add(outerArc);
            archPathFigure.Segments.Add(line);
            archPathFigure.Segments.Add(innerArc);

            archPathFigure.IsFilled = true;
            archPathFigure.IsClosed = true;

            PathGeometry arcGeometry = new PathGeometry();
            arcGeometry.Figures.Add(archPathFigure);
            dc.DrawGeometry(item.Fill, new System.Windows.Media.Pen(item.Stroke, item.StrokeThickness), arcGeometry);
        }
    }
    
    internal struct ArcItemData
    {
        public ArcItemData(double angle1, double angle2, System.Windows.Media.Brush fill, System.Windows.Media.Brush stroke, double strokeThickness, double outerRadius, double innerRadius)
        {
            Angel1 = angle1;
            Angle2 = angle2;
            Fill = fill;
            Stroke = stroke;
            StrokeThickness = strokeThickness;
            OuterRadius = outerRadius;
            InnerRadius = innerRadius;
        }
        public double Angel1;
        public double Angle2;
        public Brush Fill;
        public Brush Stroke;
        public double StrokeThickness;
        public double OuterRadius;
        public double InnerRadius;
    }
}
