using System;
using System.Windows;


namespace RadialMenuControl.Helpers
{
    public static class GeometryHelper
    {
        public static readonly int FullCircle = 360;
        public static readonly int HalfCircle = 180;
        
        public static bool IsDegreeValueValid(double value)
        {
            return (value >= 0 && value <= FullCircle);
        }

        public static double RadianToDegree(double angle)
        {
            return angle * HalfCircle / Math.PI;
        }

        public static double DegreeToRadian(double angle)
        {
            return (Math.PI * angle) / HalfCircle;
        }


        public static Point CalculatePoint(double centerX, double centerY, double angle, double radius)
        {
            double x = centerX + Math.Cos((Math.PI / 180.0) * angle) * radius;
            double y = centerY + Math.Sin((Math.PI / 180.0) * angle) * radius;
            return new Point(x, y);
        }

        public static Point CalculatePoint(Point centerPoint, double angle, double radius)
        {
            double x = centerPoint.X + Math.Cos((Math.PI / 180.0) * angle) * radius;
            double y = centerPoint.Y + Math.Sin((Math.PI / 180.0) * angle) * radius;
            return new Point(x, y);
        }

        public static double CalculateAngle(Point point, Point centerPoint)
        {
            double dy = (point.Y - centerPoint.Y);
            double dx = (point.X - centerPoint.X);
            double theta = Math.Atan2(dy, dx);
            double angle = 360 + (theta * (360 / Math.PI) % 360);
            if (Math.Round(angle) == 180.00)
            {
                angle += 360;
            }
            
            return angle;
        }
    }
}
