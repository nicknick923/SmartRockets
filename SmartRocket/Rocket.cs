using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SmartRocket
{
    class Rocket
    {
        private Vector pos;
        private Vector vel;
        private Vector acc;
        private Graphics g;


        private Bitmap p;
        private DNA dna;
        public static double maxMag = 10;
        public double fitness;
        private static Vector UP_VECTOR = new Vector(0, 1);
        private static Vector rocketSize = new Vector(10, 50);
        public static int count;
        private double largestDistance;
        private bool atTarget = false;
        private bool offScreen = false;
        double fitnessMult = 1;
        public double arrivalTime = 100000;
        public bool fastest = false;
        private int fastestBonus = 20;


        public Rocket(Graphics inGraphics, Bitmap inPanel)
        {
            g = inGraphics;
            p = inPanel;
            largestDistance = Math.Sqrt(Math.Pow(inPanel.Width, 2) + Math.Pow(inPanel.Height, 2));
            dna = new DNA();
            vel = new Vector(0, 0);
            //lol
            acc = new Vector(0, 0);
            pos = new Vector(p.Width / 2, p.Height);
            count = 0;
        }

        public Rocket(Graphics inGraphics, Bitmap inPanel, DNA inDNA)
        {
            g = inGraphics;
            p = inPanel;
            largestDistance = Math.Sqrt(Math.Pow(inPanel.Width, 2) + Math.Pow(inPanel.Height, 2));
            dna = inDNA;
            vel = new Vector(0, 0);
            acc = new Vector(0, 0);
            pos = new Vector(p.Width / 2, p.Height - 50);
            count = 0;
        }

        public void calcFitness()
        {
            double d = dist();
            fitness = Form1.map(d, 0, largestDistance, largestDistance, 0) * fitnessMult;
            if (fastest)
                fitness *= fastestBonus;
        }


        private double dist()
        {
            return Math.Sqrt(Math.Pow(pos.X - Form1.target.X, 2) + Math.Pow(pos.Y - Form1.target.Y, 2));
        }

        public void applyForce(Vector force)
        {
            acc += force;
        }

        public void update()
        {
            double d = dist();
            if (d < 40)
            {
                atTarget = true;
                if (arrivalTime > count)
                    arrivalTime = count;
            }
            if (pos.X < 0 || pos.X > p.Width
                || pos.Y < 0 || pos.Y > p.Height)
                offScreen = true;

            foreach (RectangleF rec in Form1.obsticals)
                if (pos.X > rec.X && pos.X < rec.X + rec.Width
                &&
                pos.Y > rec.Y && pos.Y < rec.Y + rec.Height)
                    offScreen = true;

            if (!atTarget && !offScreen)
            {
                applyForce(dna[count]);
                vel += acc;
                pos += vel;
                double newX = vel.X;
                double newY = vel.Y;
                if (newX > maxMag)
                    newX = maxMag;
                else if (newX < -maxMag)
                    newX = -maxMag;
                if (newY > maxMag)
                    newY = maxMag;
                else if (newY < -maxMag)
                    newY = -maxMag;
                vel = new Vector(newX, newY);
                acc = new Vector(0, 0);
            }
            else if (offScreen)
                fitnessMult = .1;
            else if (atTarget)
                fitnessMult = 10;
            show();
        }

        public void show()
        {
            g.TranslateTransform((float)pos.X, (float)pos.Y);
            g.RotateTransform((float)Vector.AngleBetween(UP_VECTOR, vel));
            if (atTarget)
                g.FillRectangle(Brushes.Red, -(float)rocketSize.X / 2, -(float)rocketSize.Y / 2, (float)rocketSize.X, (float)rocketSize.Y);
            else if (offScreen)
                g.FillRectangle(Brushes.Gray, -(float)rocketSize.X / 2, -(float)rocketSize.Y / 2, (float)rocketSize.X, (float)rocketSize.Y);
            else
                g.FillRectangle(Brushes.Aqua, -(float)rocketSize.X / 2, -(float)rocketSize.Y / 2, (float)rocketSize.X, (float)rocketSize.Y);

            g.DrawRectangle(Pens.White, -(float)rocketSize.X / 2, -(float)rocketSize.Y / 2, (float)rocketSize.X, (float)rocketSize.Y);
            g.ResetTransform();
        }

        public DNA getDNA()
        {
            return dna;
        }

        public bool isAlive()
        {
            return !atTarget && !offScreen;
        }

        public bool isAtTarget()
        {
            return atTarget;
        }
    }
}
