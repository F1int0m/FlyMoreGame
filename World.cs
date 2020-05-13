using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework.Internal;

namespace FlyMore
{
    public class World
    {
        public Drone drone;
        public List<ITrack> Elements { get; set; }
        private Vector Gravity { get; set; } = new Vector(0,1);

        private Point PreviousPoint;

        public bool IsWin { get; private set; } = false;



        public int Score { private set; get; } = 0;

        public World()
        {
            drone = new Drone();
        }

        public void Load(params ITrack[] traсks)
        {
            Elements = traсks.OrderBy(x=>x.LeftSide).ToList();
        }


        public void Update(double dAngle ,double dThrottle, Size ClienSize, double dt)
        {
            PreviousPoint = new Point((int)Math.Round(drone.Position.X), (int)Math.Round(drone.Position.Y) );
            drone.Throttle += dThrottle;
            drone = MoveDrone(drone, dAngle, ClienSize, dt);
            Check();
        }

        private void Check()
        {
            var curPos = new Point((int)Math.Round(drone.Position.X), (int)Math.Round(drone.Position.Y));
            var checkPos = new Rectangle(curPos.X, curPos.Y-drone.Heigth, drone.Width, drone.Heigth);
            if (!Elements.Any())
            {
                IsWin = true;
                return;
            }           
            if (Elements.First().CheckZone.Contains(curPos))
            {
                if (Elements.First().EnterZone.Contains(PreviousPoint))
                {
                    Elements.RemoveAt(0);
                    Score++;
                }
            }

            if (Elements.SelectMany(x=>x.TrackPart).Any(x => x.IntersectsWith(checkPos)))
            {
                drone = new Drone(new Vector(PreviousPoint.X,PreviousPoint.Y), drone.Velocity*(-0.8),drone.Angle,drone.Throttle);
            }

        }

        private readonly double maxVelocity = 350;
        private readonly double mass =0.9;
        private readonly double mainForceScale = 0.0025;

        private Vector CalcForces( double throttle)
        {
            throttle = -throttle * mainForceScale / 40;
            var thrust = new Vector(Math.Cos(drone.Angle), Math.Sin(drone.Angle));
            return thrust.Normalize()*throttle + Gravity* mainForceScale;
        }

        public Drone MoveDrone(Drone drone, double turn, Size spaceSize, double dt)
        {
            var dir = turn;
            var velocity = drone.Velocity+CalcForces(this.drone.Throttle)*dt/mass;
            if (velocity.Length > maxVelocity) velocity = velocity.Normalize() * maxVelocity;
            var location = drone.Position + velocity * dt;
            if (location.X < 0) velocity = new Vector(Math.Max(0, velocity.X), velocity.Y);
            if (location.X > spaceSize.Width) velocity = new Vector(Math.Min(0, velocity.X), velocity.Y);
            if (location.Y < 0) velocity = new Vector(velocity.X, Math.Max(0, velocity.Y));
            if (location.Y > spaceSize.Height) velocity = new Vector(velocity.X, Math.Min(0, velocity.Y));
            return new Drone(location.BoundTo(spaceSize),velocity,dir,drone.Throttle);
        }

        private Point CalcForwardPoint(Drone drone)
        {
            return  new Point((int)Math.Round(drone.Position.X) + this.drone.Width, (int)Math.Round(drone.Position.Y) - drone.Heigth / 2);
        }

    }
}
