using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labyrinth
{
    class Labyrinth
    {
        public static int abs(int a)
        {
            if (a > 0)
                return a;
            else return (-a);
        }

        //indeksite lavirint[i][j] se hashiraat vo "i j" -> broj
        public static string kodirajKoordinati(int i, int j)
        {
            return string.Format("{0} {1}", i, j);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //	 GENERIRANJE NA SLUCAEN LAVIRINT
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /* Generiranje na proizvolen lavirint so randomiziran DFS */
        public static char[][] generirajLavirint(int N)
        {
            char[][] lavirint = new char[2 * N + 1][];
            for (int i = 0; i < (2 * N + 1); i++)
                lavirint[i] = new char[2 * N + 1];

            for (int i = 0; i < 2 * N + 1; i++)
            {
                for (int j = 0; j < 2 * N + 1; j++)
                {
                    lavirint[i][j] = ' ';
                }
            }
            // Na pocetokot lavirintot e celiot so dzidovi, nema nitu eden pat
            for (int i = 0; i < 2 * N + 1; i += 2)
            {
                for (int j = 0; j < 2 * N + 1; j++)
                {
                    lavirint[i][j] = '#';
                    lavirint[j][i] = '#';
                }
            }

            Graph g = new Graph(N * N);
            Random r = new Random();
            EdgeList indeksi = new EdgeList();
            EdgeList pol = new EdgeList();
            int C = 0;

            //Offset za Gore, Desno, Dole, Levo
            int[] dx = new int[] { 0, 2, 0, -2 };
            int[] dy = new int[] { -2, 0, 2, 0 };

            //Stavanje na koordinatite na lavirintot vo EdgeLista
            for (int i = 1; i < 2 * N; i += 2)
            {
                for (int j = 1; j < 2 * N; j += 2)
                {
                    indeksi.put(kodirajKoordinati(i, j), C);
                    pol.put(C, kodirajKoordinati(i, j));
                    C++;
                }
            }

            for (int i = 1; i < 2 * N; i += 2)
            {
                for (int j = 1; j < 2 * N; j += 2)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        int ito = i + dy[k], jto = j + dx[k];
                        if (!(ito == -1 || jto == -1 || ito >= 2 * N || jto >= 2 * N))
                        {
                            g.dodadiRebro(indeksi.get(kodirajKoordinati(i, j)), indeksi.get(kodirajKoordinati(ito, jto)));
                        }
                    }
                }
            }

            // Vo prvata iteracija pravime eden DFS (slednata sostojba se izbira slucajno) od teminja koi gi oznacuvame S i E (start & end)
            // So ova obezbeduvame deka lavirintot ke ima resenie
            // DFS e randomiziran, t.e odbira eden neposeten sosed na SLUCAEN NACIN
            // Potoa pustame uste 2 DFS od slucajni pozicii vo lavirintot za da otstranime del od preprekite, da ima povekje patista

            for (int brojac = 0; brojac < 2; brojac++)
            {
                int inn = -1, jn = -1, startSostojba, celnaSostojba;
                if (brojac == 0)
                {
                    int strana = abs(r.Next()) % 2;
                    int iStart, jStart, iEnd, jEnd;
                    if (strana == 1)
                    {
                        iStart = 1 + abs(r.Next()) % (2 * N - 1);
                        jStart = 1;
                        iEnd = 1 + abs(r.Next()) % (2 * N - 1);
                        jEnd = 2 * N - 1;
                        if (lavirint[iStart][jStart] == '#')
                        {
                            iStart++;
                        }
                        if (lavirint[iEnd][jEnd] == '#')
                        {
                            iEnd++;
                        }
                    }
                    else
                    {
                        iStart = 1;
                        jStart = 1 + abs(r.Next()) % (2 * N - 1);
                        iEnd = 2 * N - 1;
                        jEnd = 1 + abs(r.Next()) % (2 * N - 1);
                        if (lavirint[iStart][jStart] == '#')
                        {
                            jStart++;
                        }
                        if (lavirint[iEnd][jEnd] == '#')
                        {
                            jEnd++;
                        }
                    }
                    lavirint[iStart][jStart] = 'S';
                    lavirint[iEnd][jEnd] = 'E';
                    startSostojba = indeksi.get(kodirajKoordinati(iStart, jStart));
                    celnaSostojba = indeksi.get(kodirajKoordinati(iEnd, jEnd));
                }
                else
                {
                    startSostojba = abs(r.Next()) % N;
                    celnaSostojba = (2 * N - 2) * (2 * N - 2) + abs(r.Next()) % (N * N - (2 * N - 2) * (2 * N - 2));
                }
                Stack<int> s = new Stack<int>();
                bool[] visited = new bool[N * N];
                s.Push(startSostojba);
                visited[startSostojba] = true;
                while (s.Count != 0)
                {
                    int v = s.Peek();
                    if (v == celnaSostojba)
                    {
                        break;
                    }
                    char[] delimiters = new char[] { ' ' };
                    string[] br = pol.get(v).Split(delimiters);
                    int vi = int.Parse(br[0]), vj = int.Parse(br[1]), obidi = 0, rand = -1;
                    bool imaNeposetenSosed = false;
                    while (!imaNeposetenSosed && obidi < 10)
                    {
                        rand = abs(r.Next()) % 4;
                        inn = vi + dy[rand];
                        jn = vj + dx[rand];
                        obidi++;
                        if (!(jn <= 0 || inn <= 0 || inn >= 2 * N || jn >= 2 * N))
                        {
                            if (!visited[indeksi.get(kodirajKoordinati(inn, jn))])
                            {
                                imaNeposetenSosed = true;
                            }
                        }
                    }
                    if (imaNeposetenSosed)
                    {
                        int next = indeksi.get(kodirajKoordinati(inn, jn));
                        s.Push(next);
                        visited[next] = true;
                        // Se probivame niz preprekite i gi otstranuvame
                        if (rand == 0)
                        {
                            lavirint[inn + 1][jn] = ' ';
                        }
                        else if (rand == 1)
                        {
                            lavirint[inn][jn - 1] = ' ';
                        }
                        else if (rand == 2)
                        {
                            lavirint[inn - 1][jn] = ' ';
                        }
                        else if (rand == 3)
                        {
                            lavirint[inn][jn + 1] = ' ';
                        }
                    }
                    else
                    {
                        s.Pop();
                    }
                }
            }
            return lavirint;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /* Naogjanje pateka od Start do Exit vo prethodno generiraniot slucaen lavirint so DFS */
        // Ovaa funckija ja rekonstruira patekata vratena vo stekot od DFS i ja oznacuva vo lavirintot
        public static char[][] oznaciPatekaVoLavirint_DFS(Stack<int> s, char[][] lavirint)
        {   
            char[][] lavirintOznacen = new char[lavirint.Length][];
            for (int i = 0; i < lavirint.Length; i++)
                lavirintOznacen[i] = new char[lavirint.Length];

            for (int i = 0; i < lavirint.Length; i++)
            {
                for (int j = 0; j < lavirint.Length; j++)
                {
                    lavirintOznacen[i][j] = lavirint[i][j];
                }
            }

            char[] delimiters = new char[] { ' ' };
            while (s.Count != 0)
            {
                int pole = s.Pop();

                string[] indeksi = polinja.get(pole).Split(delimiters);
                int iSeg = int.Parse(indeksi[0]);
                int jSeg = int.Parse(indeksi[1]);
                if (lavirintOznacen[iSeg][jSeg] != 'S' && lavirintOznacen[iSeg][jSeg] != 'E')
                {
                    if (s.Count != 0)
                    {
                        string[] koordpret = polinja.get(s.Peek()).Split(delimiters);
                        int iPret = int.Parse(koordpret[0]);
                        int jPret = int.Parse(koordpret[1]);
                        if (iPret < iSeg)
                        {
                            lavirintOznacen[iSeg][jSeg] = 'v';
                        }
                        else if (iPret > iSeg)
                        {
                            lavirintOznacen[iSeg][jSeg] = '^';
                        }
                        else if (jPret > jSeg)
                        {
                            lavirintOznacen[iSeg][jSeg] = '<';
                        }
                        else if (jPret < jSeg)
                        {
                            lavirintOznacen[iSeg][jSeg] = '>';
                        }
                    }
                }
            }
            return lavirintOznacen;
        }


        public static char[][] oznaciPatekaVoLavirint_BFS(int[] prethodnik, char[][] lavirint)
        {
            if (prethodnik == null)
            {
                return lavirint;
            }
            char[][] lavirintOznacen = new char[lavirint.Length][];
            for (int ii = 0; ii < lavirint.Length; ii++)
                lavirintOznacen[ii] = new char[lavirint.Length];

            for (int ii = 0; ii < lavirint.Length; ii++)
            {
                for (int jj = 0; jj < lavirint.Length; jj++)
                {
                    lavirintOznacen[ii][jj] = lavirint[ii][jj];
                }
            }
            int i = prethodnik[exit];

            char[] delimiters = new char[] { ' ' };
            while (prethodnik[i] != i)
            {
                string[] koordpret = polinja.get(prethodnik[i]).Split(delimiters);
                int iPret = int.Parse(koordpret[0]);
                int jPret = int.Parse(koordpret[1]);
                string[] koord = polinja.get(i).Split(delimiters);
                int iSeg = int.Parse(koord[0]);
                int jSeg = int.Parse(koord[1]);
                if (iPret < iSeg)
                {
                    lavirintOznacen[iSeg][jSeg] = 'v';
                }
                else if (iPret > iSeg)
                {
                    lavirintOznacen[iSeg][jSeg] = '^';
                }
                else if (jPret > jSeg)
                {
                    lavirintOznacen[iSeg][jSeg] = '<';
                }
                else if (jPret < jSeg)
                {
                    lavirintOznacen[iSeg][jSeg] = '>';
                }
                i = prethodnik[i];
            }
            return lavirintOznacen;
        }
        /************************************************************/
        /* Globalni promenlivi - koordinati, polinja, start, exit */
        /**/
        /**/
        static EdgeList koordinati;
        /**/
        static EdgeList polinja;
        /**/
        static int start, exit;
        /*************************************************************/


        public static Graph generirajGraph(char[][] lavirint)
        {
            // Od lavirintot sozdavame graf na sledniov nacin:
            // 1. Sekoe pole od lavirintot go pretstavuvame so jazel vo grafot
            // 2. Dokolku nema prepreka pomegju 2 polinja, togas dodavame rebro pomegju nivnite soodvetni jazli vo grafot
            // 3. Dokolku nema prepreka pomegju 2 polinja, togas NE dodavame rebro pomegju soodvetnite jazli vo grafot
            // 4. Grafot e ORIENTIRAN, so ogled na toa deka ako postoi pat pomegju 2 sosedni polinja, togas dodavame rebra i vo dvete nasoki,
            //	 bidejki ne postoi ogranicuvanje za nasokata vo koja smeeme da se dvizime
            // Algoritam za naogjanje resenie:
            // 1. DFS: pocetna sostojva: START, celnaSostojba: EXIT (soodvetnite jazli vo grafot)
            koordinati = new EdgeList();
            polinja = new EdgeList();
            int brojPolinja = lavirint.Length;
            int[] dx = new int[] { 0, 1, 0, -1 };	 // Offset za x-oska: Gore, Desno, Dole, Levo
            int[] dy = new int[] { 1, 0, -1, 0 };	 // Offset za y-oska: Gore, Desno, Dole, Levo
            Graph g = new Graph(brojPolinja * brojPolinja);
            // Mapiranje na sekoj par koordinati od matricata vo soodveten id na jazel, i obratno
            int jazelId = 0;
            for (int i = 0; i < brojPolinja; i++)
            {
                for (int j = 0; j < brojPolinja; j++)
                {
                    koordinati.put(kodirajKoordinati(i, j), jazelId);
                    polinja.put(jazelId, kodirajKoordinati(i, j));
                    jazelId++;
                }
            }
            // Nadvoresnata ramka e celosen dzid (ograda), osven START i EXIT, pa zatoa ke pocneme od indeks lavirint[1][1]
            for (int i = 1; i < brojPolinja - 1; i++)
            {
                for (int j = 1; j < brojPolinja - 1; j++)
                {
                    if (lavirint[i][j] != '#')
                    {	 //ako poleto e pristapno...
                        int jazelID = koordinati.get(kodirajKoordinati(i, j));
                        for (int k = 0; k < 4; k++)
                        {
                            int iSosed = i + dy[k];
                            int jSosed = j + dx[k];
                            if (lavirint[iSosed][jSosed] == 'S')
                            {
                                start = koordinati.get(kodirajKoordinati(iSosed, jSosed));
                            }
                            if (lavirint[iSosed][jSosed] == 'E')
                            {
                                exit = koordinati.get(kodirajKoordinati(iSosed, jSosed));
                            }
                            // Ako moze da se premine od segasnoto pole do sosednoto, a ako sosedot e #, togas ne dodavame rebro
                            if (lavirint[iSosed][jSosed] == 'S' || lavirint[iSosed][jSosed] == 'E'
                                    || lavirint[iSosed][jSosed] == ' ')
                            {
                                int sosedID = koordinati.get(kodirajKoordinati(iSosed, jSosed));
                                g.dodadiRebro_Directed(jazelID, sosedID);
                                g.dodadiRebro_Directed(sosedID, jazelID);
                            }
                        }
                    }
                }
            }
            return g;
        }

        //Resenie na lavirintot so pomos na DFS
        //Sustinata e vo toa sto grafot gi pretstavuva slobodnite polinja vo lavirintot, so toa ni obezbeduva izbegnuvanje na preprekite
        //Napomena: DFS ne garantira najkratok pat
        //Metodov generira graf od lavirintot zadaden so matrica i go resava so DFS, a na izlez go vrakja resenieto
        //Patekata na ednostaven nacin se rekornstruira so izminuvanje na stekot vo metodot oznaciPatekaVoLavirint_DFS
        public static Stack<int> resiLavirint_DFS(char[][] lavirintMatrica)
        {
            Graph lavirint = generirajGraph(lavirintMatrica);
            Stack<int> s = new Stack<int>();
            bool[] visited = new bool[lavirint.N];
            s.Push(start);
            visited[start] = true;
            while (s.Count != 0)
            {
                int jazel = s.Peek();
                if (jazel == exit)
                {
                    return s;
                }
                bool kjorsokak = true;
                foreach (int sosed in lavirint.sosedi[jazel])
                {
                    if (!visited[sosed])
                    {
                        visited[sosed] = true;
                        s.Push(sosed);
                        kjorsokak = false;
                        break;
                    }
                }
                if (kjorsokak)
                {
                    s.Pop();	 //kjorsokak, vrati se nazad
                }
            }
            return null;	 //nikogas nema da stigne tuka
        }

        // Ovoj metod generira graf od lavirintot pretstaven so matrica i na izlez vrakja niza od prethodno-poseteni jazli so BFS
        // Od ovaa niza mnogu lesno se rekonstruira patekata so metotod oznaciPatekaVoLavirint_BFS
        // Bidejki grafot e netezinski, BFS garantirano ke go najde najkratkiot pat od Start to Exit po broj na izminati jazli
        public static int[] resiLavirint_BFS(char[][] lavirintMatrica)
        {
            Graph lavirint = generirajGraph(lavirintMatrica);
            int[] prethodnik = new int[lavirint.N];
            bool[] visited = new bool[lavirint.N];
            for (int i = 0; i < prethodnik.Length; i++)
            {
                prethodnik[i] = -1;
                visited[i] = false;
            }
            Queue<int> q = new Queue<int>();
            prethodnik[start] = start;
            visited[start] = true;
            q.Enqueue(start);
            while (q.Count != 0)
            {
                int pole = q.Dequeue();
                foreach (int sosed in lavirint.sosedi[pole])
                {
                    if (!visited[sosed])
                    {
                        prethodnik[sosed] = pole;
                        visited[sosed] = true;
                        if (sosed == exit)
                        {
                            return prethodnik;
                        }
                        else
                        {
                            q.Enqueue(sosed);
                        }
                    }
                }
            }
            return null;
        }

        // Pecatenje na lavirint
        private static void pecatiLavirint(char[][] lavirint)
        {
            for (int i = 0; i < lavirint.Length; i++)
            {
                for (int j = 0; j < lavirint[0].Length; j++)
                {
                    //Console.Write(lavirint[i][j] + " ");
                }
                //Console.WriteLine();
            }
        }
        // Funkcija za ednostaven povik i organizacija na metodite definirani pogore, DFS
        public static char[][] najdiPatekaVoRandomLavirint_DFS(char[][] lavirint)
        {
            Stack<int> patekaResenie = resiLavirint_DFS(lavirint);
            char[][] reshen = oznaciPatekaVoLavirint_DFS(patekaResenie, lavirint);
            //Console.WriteLine("\nResenie na lavirintot so DFS...");
            //pecatiLavirint(reshen);
            return reshen;
        }
        // Funkcija za ednostaven povik i organizacija na metodite definirani pogore, BFS
        public static void najdiPatekaVoRandomLavirint_BFS(char[][] lavirint)
        {
            int[] prethodnik = resiLavirint_BFS(lavirint);
            char[][] reshen = oznaciPatekaVoLavirint_BFS(prethodnik, lavirint);
            //Console.WriteLine("\nResenie na lavirintot so BFS...");
            pecatiLavirint(reshen);
        }
        // Reshavanje na eden ist lavirint so DFS i BFS
        public static void resiRandomLavirint(int N)
        {
            char[][] lavirint = generirajLavirint(N);
            pecatiLavirint(lavirint);
            najdiPatekaVoRandomLavirint_DFS(lavirint);
            najdiPatekaVoRandomLavirint_BFS(lavirint);
        }
    }
}
