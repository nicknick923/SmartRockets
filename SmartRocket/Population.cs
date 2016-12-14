using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartRocket
{
    class Population
    {

        private Graphics g;
        private Bitmap p;
        Rocket[] rockets;
        List<Rocket> matingPool;
        public int generation = 1;
        public static double InitalMutationFactor = 0.1;
        public static double mutationFactor = InitalMutationFactor;
        private double[] mutationMultipliers = { .5,.25,.1, .01, .001 };


        public Population(Graphics inGraphics, Bitmap inPanel, int numRockets)
        {
            g = inGraphics;
            p = inPanel;
            rockets = new Rocket[numRockets];
            for (int i = 0; i < rockets.Length; i++)
                rockets[i] = new Rocket(g, p);
        }

        public float percentOnTarget;

        int numAtTarget;
        public void evaluate()
        {
            numAtTarget = 0;
            for (int i = 0; i < rockets.Length; i++)
                if (rockets[i].isAtTarget())
                    numAtTarget++;
            percentOnTarget = ((float)numAtTarget / rockets.Length) * 100;
            if (numAtTarget != 0)
                if (percentOnTarget > 95)
                    mutationFactor = InitalMutationFactor*mutationMultipliers[4];
                else if (percentOnTarget > 90)
                    mutationFactor = InitalMutationFactor * mutationMultipliers[3];
                else if (percentOnTarget > 80)
                    mutationFactor = InitalMutationFactor * mutationMultipliers[2];
                else if (percentOnTarget > 50)
                    mutationFactor = InitalMutationFactor * mutationMultipliers[1];
                else if (percentOnTarget > 25)
                    mutationFactor = InitalMutationFactor * mutationMultipliers[0];

            double maxFit = 0;
            int indexOfMinTime = 0;
            for (int i = 0; i < rockets.Length; i++)
                if (rockets[i].arrivalTime < rockets[indexOfMinTime].arrivalTime)
                    indexOfMinTime = i;
            rockets[indexOfMinTime].fastest = true;
            for (int i = 0; i < rockets.Length; i++)
            {
                rockets[i].calcFitness();
                if (rockets[i].fitness > maxFit)
                    maxFit = rockets[i].fitness;
            }
            for (int i = 0; i < rockets.Length; i++)
                rockets[i].fitness /= maxFit;
            matingPool = new List<Rocket>();
            for (int i = 0; i < rockets.Length; i++)
            {
                int n = (int)rockets[i].fitness * 100;
                for (int j = 0; j < n; j++)
                    matingPool.Add(rockets[i]);
            }
        }

        public void naturalSelection()
        {
            generation++;
            evaluate();
            Rocket[] newRockets = new Rocket[rockets.Length];
            for (int i = 0; i < rockets.Length; i++)
            {
                DNA parentA = matingPool[Form1.r.Next(0, matingPool.Count - 1)].getDNA();
                DNA parentB = matingPool[Form1.r.Next(0, matingPool.Count - 1)].getDNA();

                DNA childDNA = parentA.crossover(parentB);
                childDNA.mutate();
                newRockets[i] = new Rocket(g, p, childDNA);
            }
            rockets = newRockets;
        }

        public void run()
        {
            bool someoneAlive = false;
            for (int i = 0; i < rockets.Length; i++)
                if (rockets[i].isAlive())
                    someoneAlive = true;
            if (!someoneAlive)
                Rocket.count = Form1.lifespan - 1;
            else
                for (int i = 0; i < rockets.Length; i++)
                    rockets[i].update();
        }

        public void newPop()
        {
            for (int i = 0; i < rockets.Length; i++)
                rockets[i] = new Rocket(g, p);
        }
    }
}
