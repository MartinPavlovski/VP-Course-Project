using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labyrinth
{
    class Graph
    {
        public int N;
        public List<int>[] sosedi;

        public Graph(int N)
        {
            sosedi = new List<int>[N];
            for (int i = 0; i < N; i++)
            {
                sosedi[i] = new List<int>();
            }
            this.N = N;
        }

        public void dodadiRebro(int i, int j)
        { //od i -> j i j -> i
            sosedi[i].Add(j);
            sosedi[j].Add(i);
        }
        public void dodadiRebro_Directed(int i, int j)
        {	//od i kon j
            sosedi[i].Add(j);
        }
    }
}
