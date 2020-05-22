using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FlyMore
{
    public class World
    {
        public Drone drone;
        public IReadOnlyCollection<ITrack> Elements => ElementsList.AsReadOnly();
        private  List<ITrack> ElementsList { get; set; }
        private Vector Gravity { get; } = new Vector(0,1);

        private Point PreviousPoint;
        public bool IsWin { get; private set; } = false;
        public int Score { private set; get; } = 0;

        public World()
        {
            drone = new Drone(new Vector(500,700),Vector.Zero, Math.PI/2,0 );
            ElementsList = new List<ITrack>();
        }

        public void Load(params ITrack[] traсks)
        {
            ElementsList = traсks.OrderBy(x=>x.LeftSide).ToList();
        }


        public void Update(double angle ,double dThrottle, Size clienSize, double dt)
        {
            PreviousPoint = new Point((int)Math.Round(drone.Position.X), (int)Math.Round(drone.Position.Y) );
            drone.Throttle += dThrottle;
            drone = MoveDrone(drone, angle, clienSize, dt);
            Check();
            MoveWorld(clienSize);
        }

       
        private void MoveWorld(Size size)
        {
            var delta = size.Width / 4;
            var dif = (int)drone.Position.X - size.Width / 2+200;
            var move = dif > 0 ? 5 : -5;
            if (Math.Abs(dif) <= delta) return;
            ElementsList = ElementsList.Select(x =>
            {
                x.CheckZone = new Rectangle(x.CheckZone.X - move, x.CheckZone.Y, x.CheckZone.Width,
                    x.CheckZone.Height);
                x.EnterZone = new Rectangle(x.EnterZone.X - move, x.EnterZone.Y, x.EnterZone.Width,
                    x.EnterZone.Height);

                x.TrackPart = x.TrackPart.Select(s => new Rectangle(s.X - move, s.Y, s.Width, s.Height)).ToArray();
                return x;

            }).ToList();
            drone = new Drone(new Vector(drone.Position.X - move, drone.Position.Y), drone.Velocity, drone.Angle,drone.Throttle);
        }

        public static double GetAngle(Point mouse, Drone drone)
        {

            var resVector = new Vector(mouse.X - drone.Position.X, mouse.Y - drone.Position.Y);
            var temp = Math.PI - Math.Acos(resVector.X / resVector.Length);

            return mouse.Y > drone.Position.Y ? Math.PI * 2 - temp : temp;
        }

        private void Check()
        {
            var curPos = new Point((int)Math.Round(drone.Position.X), (int)Math.Round(drone.Position.Y));
            var checkPos = new Rectangle(curPos.X, curPos.Y-drone.Heigth, drone.Width, drone.Heigth);
            if (!ElementsList.Any())
            {
                IsWin = true;
                return;
            }           
            if (ElementsList.First().CheckZone.Contains(curPos))
            {
                if (ElementsList.First().EnterZone.Contains(PreviousPoint))
                {
                    ElementsList.RemoveAt(0);
                    Score++;
                }
            }

            if (ElementsList.SelectMany(x=>x.TrackPart).Any(x => x.IntersectsWith(checkPos)))
            {
                drone = new Drone(new Vector(PreviousPoint.X,PreviousPoint.Y), drone.Velocity*(-0.8),drone.Angle,drone.Throttle);
            }


        }

        private readonly double maxVelocity = 250;
        private readonly double mass =0.9;
        private readonly double mainForceScale = 0.0025;

        private Vector CalcForces( double throttle)
        {
            throttle = -throttle * mainForceScale / 40;
            var thrust = new Vector(Math.Cos(drone.Angle), Math.Sin(drone.Angle));
            return thrust.Normalize()*throttle + Gravity* mainForceScale;
        }

        private Drone MoveDrone(Drone tDrone, double angle, Size spaceSize, double dt)
        {
            var velocity = tDrone.Velocity+CalcForces(this.drone.Throttle)*dt/mass;
            if (velocity.Length > maxVelocity) velocity = velocity.Normalize() * maxVelocity;
            var location = tDrone.Position + velocity * dt;
            if (location.X < 0) velocity = new Vector(Math.Max(0, velocity.X), velocity.Y);
            if (location.X > spaceSize.Width) velocity = new Vector(Math.Min(0, velocity.X), velocity.Y);
            if (location.Y < 0) velocity = new Vector(velocity.X, Math.Max(0, velocity.Y));
            if (location.Y > spaceSize.Height) velocity = new Vector(velocity.X, Math.Min(0, velocity.Y));
            
            return new Drone(location.BoundTo(spaceSize),velocity, angle, tDrone.Throttle);
        }
    }
}
