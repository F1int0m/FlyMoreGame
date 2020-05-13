using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyMore
{
    public interface ITrack
    {
        Rectangle CheckZone { get; set; }
        Rectangle EnterZone { get; set; }
        Rectangle[] TrackPart { get; set; }
        int Width { get; }
        int Height { get; }
        int LeftSide { get; }

    }
}
