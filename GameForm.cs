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

    //Вопросы
    //Как поворачивать картинку без ообрезания?
    //Как сделать "прокуртку" мира, относительно дрона?(как в Марио)
    public partial class GameForm : Form
    {
        public readonly int dy = 10;
        public GameForm()
        {
            BackColor = Color.White;

            Size = new Size(800,800);
            DoubleBuffered = true;
            var world = new World();
            var distance = 150;
            world.Load(Enumerable.Range(1, 6).Select(x => Gate.GeneateGate(x * distance)).ToArray());


            var timer = new Timer();

            var dt = 10; //Подобрать
            double dthr = 0.0;
            double dAngle = 0;
            timer.Interval = dt;
            timer.Tick += (sender, args) =>
            {
                world.Update(GetAngle(world.drone.Position), dthr, ClientSize, dt);
                dAngle = 0;
                dthr = 0;
                Invalidate();
            };
            ClientSizeChanged += (s, a) =>
            {
                world.Load(CalcY(world.Elements, ClientSize));
            };
            
            KeyDown += (s, a) =>
            {
                if (a.KeyCode == Keys.A)
                {
                    dAngle = -1;
                }

                if (a.KeyCode == Keys.D)
                {
                    dAngle = 1;
                }
            };

            MouseWheel += (s, a) =>
            {
                if (a.Delta > 0)
                {
                    dthr += 10;
                }

                if (a.Delta < 0)
                {
                    dthr -= 10;
                }
            };

            timer.Start();
            Paint += (s, a) =>
            {
                var dronePoint = new PointF((float)world.drone.Position.X,(float)world.drone.Position.Y - world.drone.Heigth);
                //a.Graphics.FillEllipse(Brushes.Aqua, (float) world._drone.Position.X,
                //    (float) world._drone.Position.Y - 50, world._drone.Width, world._drone.Heigth);

                a.Graphics.DrawImage(RotateImage(Drone.Image,(world.drone.Angle-Math.PI/2)*180/Math.PI),dronePoint);
                a.Graphics.DrawString(world.drone.Throttle.ToString(), new Font("arial", 12), Brushes.Black, 0, 0);
                a.Graphics.DrawString(world.drone.Angle.ToString(), new Font("arial", 12), Brushes.Black, 0, 20);
                a.Graphics.DrawString(world.Score.ToString(), new Font("arial", 12), Brushes.Black, 0, 40);
                //a.Graphics.DrawString(Cursor.Position.ToString(), new Font("arial", 12), Brushes.Black, 0, 60);
                var c = PointToClient(Cursor.Position);
                a.Graphics.DrawLine(Pens.Brown, (int)world.drone.Position.X, (int)world.drone.Position.Y, c.X, c.Y);




                foreach (var element in world.Elements)
                {
                    a.Graphics.DrawRectangle(Pens.Green,element.EnterZone);
                    a.Graphics.DrawRectangle(Pens.Yellow,element.CheckZone);
                    foreach (var part in element.TrackPart)
                    {
                        a.Graphics.FillRectangle(Brushes.Black,part);
                    }
                }
            };

        }

        public double GetAngle(Vector  vect)
        {
            var mouse = PointToClient(Cursor.Position);
            var resVector = new Vector(mouse.X - vect.X, mouse.Y - vect.Y);
            var temp = Math.PI - Math.Acos(resVector.X / resVector.Length);
            
            return mouse.Y > vect.Y ? Math.PI * 2 - temp : temp;
        }

        private ITrack[] CalcY(List<ITrack> items, Size size)
        {
            return items.Select(x =>
            {
                var chek = x.CheckZone;
                var enter = x.EnterZone;
                var eHeight = x.Height;
                //x.CheckZone = new Rectangle(chek.X, size.Height  - dy-chek.Height, chek.Width, chek.Height);
                //x.EnterZone = new Rectangle(enter.X, size.Height  - dy-enter.Height, enter.Width, enter.Height);
                //x.TrackPart = //x.TrackPart;
                //    x.TrackPart.Select(z => new Rectangle(z.X, size.Height - z.Height - dy, z.Width, z.Height))
                //    .ToArray();

                x.CheckZone = new Rectangle(chek.X, size.Height - dy - eHeight, chek.Width, chek.Height);
                x.EnterZone = new Rectangle(enter.X, size.Height - dy - eHeight, enter.Width, enter.Height);
                x.TrackPart = //x.TrackPart;
                    x.TrackPart.Select(z => new Rectangle(z.X, size.Height - x.Height - dy , z.Width, z.Height))
                    .ToArray();

                return x;
            }).ToArray();
        }

        public static Image RotateImage(Image img, double rotationAngle)
        {
            var size = (int)Math.Sqrt(img.Height*img.Height+img.Width*img.Width);
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(size,size);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform((float)rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }




    }
}
