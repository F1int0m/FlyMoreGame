using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace FlyMore
{
    public partial class GameForm : Form
    {
        public const int dy = 10;
        private const int dt = 10;
        private readonly World world;
        public GameForm(World inputWorld)
        {
            this.world = inputWorld;
            var throttleBar = new ProgressBar {Maximum = 100, Minimum = 0, Value = (int) world.drone.Throttle, Step = 10};
            throttleBar.MarqueeAnimationSpeed = 1;
            Controls.Add(throttleBar);
            BackColor = Color.White;
            Size = new Size(1024,720);
            DoubleBuffered = true;
            var timer = new Timer();
            double dthr = 0.0;
            timer.Interval = dt;

            timer.Tick += (sender, args) =>
            {
                world.Update(GetAngle(world.drone.Position), dthr, ClientSize, dt);
                dthr = 0;
                Invalidate();
                throttleBar.Value = (int) world.drone.Throttle;
            };

            ClientSizeChanged += (s, a) =>
            {
                world.Load(CalcY(world.Elements, ClientSize));
            };

            MouseWheel += (s, a) => dthr += a.Delta > 0 ? 10 : -10;

            Paint += Draw;

            timer.Start();
        }

        

        private void Draw(object abc, PaintEventArgs a)
        {
            
            var dronePoint = new PointF((float)world.drone.Position.X,(float)world.drone.Position.Y - world.drone.Heigth);
            var dronePicture = new Bitmap(Drone.Image);
            dronePicture.MakeTransparent();
            a.Graphics.DrawImage(RotateImage(dronePicture,(world.drone.Angle-Math.PI/2)*180/Math.PI),dronePoint);

            a.Graphics.DrawString("Gate left: "+world.Elements.Count.ToString(),new Font("arial", 12), Brushes.Black, 0, 40);
            
            if (world.IsWin)
            {
                a.Graphics.DrawString("Yep, you win!!!", new Font("arial", 20), 
                    Brushes.Black, ClientSize.Width/2-50, ClientSize.Height/2);
            }

            foreach (var element in world.Elements)
            {
                a.Graphics.DrawRectangle(Pens.Green,element.EnterZone);
                a.Graphics.DrawRectangle(Pens.Yellow,element.CheckZone);
                foreach (var part in element.TrackPart)
                {
                    a.Graphics.FillRectangle(Brushes.Black,part);
                }
            }
        }

        public double GetAngle(Vector  vect)
        {
            var mouse = PointToClient(Cursor.Position);
            var resVector = new Vector(mouse.X - vect.X, mouse.Y - vect.Y);
            var temp = Math.PI - Math.Acos(resVector.X / resVector.Length);
            
            return mouse.Y > vect.Y ? Math.PI * 2 - temp : temp;
        }

        private ITrack[] CalcY(IEnumerable<ITrack> items, Size size)
        {
            return items.Select(x =>
            {
                var check = x.CheckZone;
                var enter = x.EnterZone;
                var eHeight = x.Height;

                x.CheckZone = new Rectangle(check.X, size.Height - dy - eHeight, check.Width, check.Height);
                x.EnterZone = new Rectangle(enter.X, size.Height - dy - eHeight, enter.Width, enter.Height);
                x.TrackPart = 
                    x.TrackPart.Select(z => new Rectangle(z.X, size.Height - eHeight - dy , z.Width, z.Height))
                    .ToArray();

                return x;
            }).ToArray();
        }

        public static Image RotateImage(Bitmap img, double rotationAngle)
        {
            var size = (int)Math.Sqrt(img.Height*img.Height+img.Width*img.Width);
            
            Bitmap bmp = new Bitmap(size,size);
          
            Graphics gfx = Graphics.FromImage(bmp);
           
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            gfx.RotateTransform((float)rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            gfx.DrawImage(img, new Point(0, 0));

            gfx.Dispose();

            return bmp;
        }

    }
}
