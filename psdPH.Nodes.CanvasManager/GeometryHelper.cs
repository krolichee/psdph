using System;
using System.Windows; // Для Point и Rect

public static class GeometryHelper
{
    public static bool LineIntersectsRect(Point p1, Point p2, Rect rect)
    {
        // Проверяем, находятся ли какие-либо точки линии внутри прямоугольника
        if (rect.Contains(p1) || rect.Contains(p2))
            return true;

        // Проверяем пересечение с каждой стороной прямоугольника
        return LineIntersectsLine(p1, p2, new Point(rect.X, rect.Y), new Point(rect.X + rect.Width, rect.Y)) || // Верхняя сторона
               LineIntersectsLine(p1, p2, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height)) || // Правая сторона
               LineIntersectsLine(p1, p2, new Point(rect.X + rect.Width, rect.Y + rect.Height), new Point(rect.X, rect.Y + rect.Height)) || // Нижняя сторона
               LineIntersectsLine(p1, p2, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X, rect.Y)); // Левая сторона
    }

    private static bool LineIntersectsLine(Point l1p1, Point l1p2, Point l2p1, Point l2p2)
    {
        // Рассчитываем направление отрезков
        double q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
        double d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

        if (d == 0)
        {
            return false;
        }

        double r = q / d;

        q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
        double s = q / d;

        if (r < 0 || r > 1 || s < 0 || s > 1)
        {
            return false;
        }

        return true;
    }
}