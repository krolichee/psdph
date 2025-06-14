﻿using Photoshop;
using System.Collections;
using System.Linq;
using Application = Photoshop.Application;

namespace psdPH.Photoshop
{
    public partial class ArtLayerWr : LayerWr
    {
        //For Copying
        public override Application Application => _layer.Application;
        public override double[] Bounds => (_layer.Bounds as dynamic[]).Cast<double>().ToArray();
        public override dynamic Parent => _layer.Parent;
        public override dynamic Name { get => _layer.Name; set => _layer.Name = value; }
        public override bool Visible { get => _layer.Visible; set => _layer.Visible = value; }
        public override double Opacity { get => _layer.Opacity; set => _layer.Opacity = value; }
        public override double[] BoundsNoEffects => (_layer.Layer.BoundsNoEffects as dynamic[]).Cast<double>().ToArray();
        public override void Duplicate() => _layer.Duplicate();
        public override void MakeActive() =>
            GetActiveDocument().ActiveLayer = _layer;

        public override void Move(object dest, PsElementPlacement placement) =>
            _layer.Move(dest, placement);

        public override void Resize(double w, double h) =>
            _layer.Resize(w, h);

        public override void Translate(double x, double y) =>
            _layer.Translate(x, y);

        public override void Duplicate(object dest, PsElementPlacement placement) =>
            _layer.Duplicate(dest, placement);
    }
}
