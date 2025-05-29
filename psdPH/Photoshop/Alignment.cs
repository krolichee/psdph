using System.Dynamic;
using System.Windows;
using System;
using System.Windows.Media;

namespace psdPH.Logic
{
    public enum HAilgnment
    {
        Left,
        Right,
        Center,
        None
    }
    public enum VAilgnment
    {
        Top,
        Bottom,
        Center,
        None
    }
    public static partial class PhotoshopDocumentExtension
    {
        public class Alignment: IEquatable<Alignment>
        {
            public override int GetHashCode() => (int)H * 4 + (int)V;
            public HAilgnment H;
            public VAilgnment V;
            public Alignment(HAilgnment horizontal, VAilgnment vertical)
            {
                H = horizontal;
                V = vertical;
            }
            public static Alignment Create(string vStr, string hStr)
            {
                hStr = hStr.ToLower();
                vStr = vStr.ToLower();
                HAilgnment h;
                VAilgnment v;
                switch (hStr)
                {
                    case "left":
                        h = HAilgnment.Left;
                        break;
                    case "center":
                        h = HAilgnment.Center;
                        break;
                    case "right":
                        h = HAilgnment.Right;
                        break;
                    case "none":
                        h = HAilgnment.None;
                        break;
                    default:
                        throw new ArgumentException();
                }
                switch (vStr)
                {
                    case "up":
                    case "top":
                        v = VAilgnment.Top;
                        break;
                    case "center":
                        v = VAilgnment.Center;
                        break;
                    case "down":
                    case "bottom":
                        v = VAilgnment.Bottom;
                        break;
                    case "none":
                        v = VAilgnment.None;
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
