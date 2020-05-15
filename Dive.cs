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
    class Dive : ITrack
    {
        
        public Dive(Rectangle checkZone, Rectangle enterZone, Rectangle[] trackPart)
        {
            CheckZone = checkZone;
            EnterZone = enterZone;
            TrackPart = trackPart;
        }

        public static Dive GeneateDive(int x) => new Dive(new Rectangle(x, 100, 150, 20), new Rectangle(x, 80, 150, 20),
            new[] {new Rectangle(x, 120, 40, 80)});
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
