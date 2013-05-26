using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labyrinth
{
    class EdgeList
    {
        public List<EdgeHelpClass> lista;

        public EdgeList()
        {
            lista = new List<EdgeHelpClass>();
        }

        public void put(int a, string b)
        {
            lista.Add(new EdgeHelpClass(a, b));
        }
        public void put(string b, int a)
        {
            lista.Add(new EdgeHelpClass(b, a));
        }

        public int get(string b)
        {
            for (int i = 0; i < lista.Count; i++)
                if (lista[i].b == b)
                    return lista[i].a;
            return -1;
        }
        public string get(int a)
        {
            for (int i = 0; i < lista.Count; i++)
                if (lista[i].a == a)
                    return lista[i].b;
            return null;
        }
    }
}
