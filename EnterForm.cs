using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlyMore
{
    public partial class EnterForm : Form
    {
        private static int ImageCounter = 0 ;

        public EnterForm(World world)
        {
            DoubleBuffered = true;
            InitializeComponent();
            var modelButton = CreateModelButton(new Point(50,50));
            Controls.Add(modelButton);

            var elemCount = new TextBox();
            elemCount.Text = "10";
            elemCount.Location = new Point(200,100);
            var text = new Label();
            text.Text = "Elements Count";
            text.Location = new Point(50,100);
            Controls.Add(elemCount);
            Controls.Add(text);

            var okBut = new Button(){Text = "Start Game", Location = new Point(50, 150)};
            okBut.Click += (s, a) => { this.Close(); };
            Controls.Add(okBut);



            Paint += Painting;
            var c = new Timer {Interval = 10};
            c.Tick += (s, a) => Invalidate();
            c.Start();
            FormClosed += (s, a) =>
            {
                var rnd = new Random();
                world.Load(Enumerable.Range(1, int.Parse(elemCount.Text.Trim())).Select(x => rnd.Next(2)<1? Gate.GeneateGate(x * 250):(ITrack)Dive.GeneateDive(x*250))
                    .ToArray());
                Drone.Image = Image.FromFile("../../images/Drone" + ImageCounter + ".png");
            };

        }

        private void Painting(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Image.FromFile("../../images/Drone"+ImageCounter+".png"),200,50 );
        }

        private static Button CreateModelButton(Point point)
        {
            var modelButton = new Button {Text = "Change Drone", Location = point};
            modelButton.Click += (s, a) =>
            {
                ImageCounter++;
                if (ImageCounter > 1)
                    ImageCounter = 0;
            };
            return modelButton;
        }
    }
}
