﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlyMore
{
    class Dive : ITrack
    {
        private const int DefaultY = 500;
        private const int zoneHeight = 20;
        private const int zonewidth = 150;


        public Dive(Rectangle checkZone, Rectangle enterZone, Rectangle[] trackPart)
        {
            CheckZone = checkZone;
            EnterZone = enterZone;
            TrackPart = trackPart;
        }

        public static Dive GeneateDive(int x) => new Dive(new Rectangle(x, DefaultY + 2*zoneHeight, zonewidth, zoneHeight), new Rectangle(x, DefaultY+zoneHeight, zonewidth, zoneHeight),
            new[] {new Rectangle(x, DefaultY + 3*zoneHeight, zoneHeight*2, zoneHeight*4) });
        public Rectangle CheckZone { get; set; }
        public Rectangle EnterZone { get; set; }
        public Rectangle[] TrackPart { get; set; }

        public int Width =>
            GetMax(TrackPart.Select(x => x.Right).Concat(new List<int>() {CheckZone.Right, EnterZone.Right}))
            - GetMin(TrackPart.Select(x => x.Left).Concat(new List<int>() {CheckZone.Left, EnterZone.Left}));

        public int Height => GetMax(TrackPart.Select(x => x.Bottom)
                                 .Concat(new List<int>() {CheckZone.Bottom, EnterZone.Bottom}))
                             - GetMin(TrackPart.Select(x => x.Top)
                                 .Concat(new List<int>() {CheckZone.Top, EnterZone.Top}));

        public int LeftSide => GetMin(TrackPart.Select(x => x.Left).Concat(new List<int>() { CheckZone.Left, EnterZone.Left}));

        private int GetMax(IEnumerable<int> rects)
        {
            return rects.Max();
        }
        private int GetMin(IEnumerable<int> rects)
        {
            return rects.Min();
        }

    }
}
