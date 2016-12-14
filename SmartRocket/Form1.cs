using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartRocket
{

    public partial class Form1 : Form
    {
        public static int lifespan = 250;
        public static int numRockets = 10000;
        private Graphics g;
        private Population pop;
        static public Random r = new Random();
        public static Rect target;
        public static int targetRadius = 40;
        public static List<RectangleF> obsticals = new List<RectangleF>();
        Stats statsForm = new Stats();
        Bitmap bm;
        Graphics other;

        public Form1()
        {
            InitializeComponent();
            g = drawingPanel.CreateGraphics();
            bm = new Bitmap(drawingPanel.Width, drawingPanel.Height);
            other = Graphics.FromImage(bm);
            other.Clear(Color.White);
            pop = new Population(other, bm, numRockets);
            target = new Rect(Width / 2, 50, 2 * targetRadius, 2 * targetRadius);
            obsticals.Add(new RectangleF(150, 220, 150, 50));
            //obsticals.Add(new RectangleF(450, 220, 150, 50));
            obsticals.Add(new RectangleF(650, 220, 150,50));
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            statsForm.Show();
            String s = pop.percentOnTarget.ToString() + "% on target. Generation: " + pop.generation + " Number of Rockets: " + numRockets;
            statsForm.AddItemToList(s);
            label1.Text = s;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            other.Clear(drawingPanel.BackColor);
            other.DrawEllipse(Pens.Green, (float)(target.X - targetRadius), (float)(target.Y - targetRadius), targetRadius, targetRadius);
            foreach (RectangleF rec in obsticals)
                other.FillRectangle(Brushes.Yellow, rec);
            pop.run();
            Rocket.count++;
            if (Rocket.count == lifespan)
            {
                pop.naturalSelection();
                Rocket.count = 0;
                String s = pop.percentOnTarget.ToString() + "% on target. Generation: " + pop.generation + " Number of Rockets: " + numRockets;
                statsForm.AddItemToList(s);
                label1.Text = s;
                System.Threading.Thread.Sleep(500);
            }
            drawingPanel_Paint(null, null);
        }

        private void drawingPanel_Paint(object sender, PaintEventArgs e)
        {
            g.DrawImage(bm, 0, 0);
        }

        public static double map(double x, double in_min, double in_max, double out_min, double out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

    }
}
