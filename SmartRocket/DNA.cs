using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartRocket
{
    class DNA
    {
        private Vector[] genes = new Vector[Form1.lifespan];

        public Vector this[int index]
        {
            get
            {
                return genes[index];
            }
        }
        public DNA()
        {
            for (int i = 0; i < Form1.lifespan; i++)
                genes[i] = newRandomVector();
        }

        public DNA crossover(DNA inDNA)
        {
            DNA newDNA = new DNA();
            int midpoint = Form1.r.Next(0, genes.Length);
            for (int i = 0; i < genes.Length; i++)
                if (i < midpoint)
                    newDNA.genes[i] = genes[i];
                else
                    newDNA.genes[i] = inDNA.genes[i];
            return newDNA;
        }

        public void mutate()
        {
            for (int i = 0; i < genes.Length; i++)
                if (Form1.r.NextDouble() < Population.mutationFactor)
                    genes[i] = newRandomVector();
        }

        private Vector newRandomVector()
        {
            return new Vector(Form1.map(Form1.r.NextDouble(), 0, 1, -Rocket.maxMag, Rocket.maxMag), Form1.map(Form1.r.NextDouble(), 0, 1, -Rocket.maxMag, Rocket.maxMag));
        }
    }
}
