using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyMore
{
    public class Drone
    {
        public int Width => Image.Width;
        public int Heigth => Image.Height;
        public Drone()
        {
            Position = new Vector(0, 0);
            Velocity = Vector.Zero;
            Throttle = 0;
            Angle = Math.PI / 2;
        }

        public Drone(Vector position, Vector velocity, double angle, double throttle)
        {
            Position = position;
            Velocity = velocity;
            Throttle = throttle;
            Angle = angle;
        }
        public static Image Image = Image.FromFile("../../images/Drone.png");
        public double Angle { get; set; }
        public Vector Position {  get; set; }
        public Vector Velocity { get; set; }
        public double Throttle
        {
            get => throttle;
            set
            {
                if (value > 100)
                {
                    throttle = 100;
                    return;
                }
                if (value < 0)
                {
                    throttle = 0;
                    return;
                }

                throttle = value;
            }
        }


        private double throttle;


        
    }
}
