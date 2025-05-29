using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Logic.PhotoshopDocumentExtension;
using System.Windows.Controls;
using System.Windows;
using psdPH.Photoshop;

namespace psdPH.Logic
{
	public static partial class PhotoshopLayerExtension
	{
		public static Vector GetAlightmentVector(this LayerWr dynamicLayer, LayerWr targetLayer, Alignment alignment = null)
		{
			return GetAlightmentVector(targetLayer.GetBoundRect(), dynamicLayer.GetBoundRect(), alignment);
		}
		public static void AlignLayer(this LayerWr dynamicLayer, LayerWr targetLayer, Alignment alignment)
		{
			dynamicLayer.TranslateV(dynamicLayer.GetAlightmentVector(targetLayer, alignment));
        }
		public static Vector GetAlightmentVector(Rect targetRect, Rect dynamicRect, Alignment alignment = null)
		{
			if (alignment == null)
				alignment = new Alignment(HAilgnment.Left, VAilgnment.Top);
			double x = 0;
			double y = 0;
			switch (alignment.H)
			{
				case HAilgnment.Left:
					x = targetRect.Left - dynamicRect.Left;
					break;
				case HAilgnment.Right:
					x = targetRect.Right - dynamicRect.Right;
					break;
				case HAilgnment.Center:
                    double t_w = targetRect.Width;
                    double d_w = dynamicRect.Width;
                    x = (targetRect.Left + t_w / 2) - (dynamicRect.Left + d_w / 2);
                    break;
                case HAilgnment.None:
					x = y = 0;
					break;
			}
			switch (alignment.V)
			{
				case VAilgnment.Top:
					y = targetRect.Top - dynamicRect.Top;
					break;
				case VAilgnment.Bottom:
					y = targetRect.Bottom - dynamicRect.Bottom;
					break;
				case VAilgnment.Center:
					double t_h = targetRect.Height;
					double d_h = dynamicRect.Height;
					y = (targetRect.Top + t_h / 2) - (dynamicRect.Top + d_h / 2);
					break;
                case VAilgnment.None:
                    x = y = 0;
                    break;
            }
			return new Vector(x, y);
		}
	}
}
