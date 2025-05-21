using System.Dynamic;
using System.Windows;
using System;

namespace psdPH.Logic
{
    public static partial class PhotoshopDocumentExtension
    {
        public class Alignment: IEquatable<Alignment>
        {
            public override int GetHashCode() => (int)H * 4 + (int)V;
            public HorizontalAlignment H;
            public VerticalAlignment V;
            public Alignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
            {
                H = horizontal;
                V = vertical;
            }
            public static Alignment Create(string vStr, string hStr)
            {
                hStr = hStr.ToLower();
                vStr = vStr.ToLower();
                HorizontalAlignment h;
                VerticalAlignment v;
                switch (hStr)
                {
                    case "left":
                        h = HorizontalAlignment.Left;
                        break;
                    case "center":
                        h = HorizontalAlignment.Center;
                        break;
                    case "right":
                        h = HorizontalAlignment.Right;
                        break;
                    default:
                        throw new ArgumentException();
                }
                switch (vStr)
                {
                    case "up":
                    case "top":
                        v = VerticalAlignment.Top;
                        break;
                    case "center":
                        v = VerticalAlignment.Center;
                        break;
                    case "down":
                    case "bottom":
                        v = VerticalAlignment.Bottom;
                        break;
                    default:
                        throw new ArgumentException();
                }
                return new Alignment(h, v);

            }

            public bool Equals(Alignment other)
            {
                return this.H == other.H && this.V == other.V;
            }
            public override string ToString()
            {
                return $"H = {this.H}\nV = {this.V}";
            }
            public string ToLocalizedString()
            {
                return $"Горизонтально: {EnumLocalization.GetLocalizedDescription(this.H)}\n" +
                    $"Вертикально: {EnumLocalization.GetLocalizedDescription(this.V)}";
            }
        }
    }
}
