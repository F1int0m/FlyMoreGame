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
        
        public Gate(Rectangle checkZone, Rectangle enterZone, Rectangle[] trackPart)
        {
            CheckZone = checkZone;
            EnterZone = enterZone;
            TrackPart = trackPart;
        }

        public static Gate GeneateGate(int x) => new Gate(new Rectangle(x+20, 480, 20, 100), new Rectangle(x, 480, 20, 100),
            new[] {new Rectangle(x, 580, 40, 40), new Rectangle(x, 440, 40, 40)});
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
