using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlyMore
{
    class Gate : ITrack
    {
        private const int DefaultY = 480;
        private const int zoneHeight = 100;
        private const int zonewidth = 20;
        public Gate(Rectangle checkZone, Rectangle enterZone, Rectangle[] trackPart)
        {
            CheckZone = checkZone;
            EnterZone = enterZone;
            TrackPart = trackPart;
        }

        public static Gate GeneateGate(int x) => new Gate(new Rectangle(x+20, DefaultY, zonewidth, zoneHeight), new Rectangle(x, DefaultY, zonewidth, zoneHeight),
            new[] {new Rectangle(x, DefaultY+100, 40, 40), new Rectangle(x, DefaultY-40, 40, 40)});
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
