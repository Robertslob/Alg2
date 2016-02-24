using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmiek2
{//Score geval aanpassen
    class Program
    {
        static void Main(string[] args)
        {
            //Console.SetIn(new StreamReader("voorbeeld.in"));
            string s = Console.ReadLine();
            string[] strings = s.Split(' ');
            ushort L = ushort.Parse(strings[0]);
            ushort modus = ushort.Parse(strings[1]);

            s = Console.ReadLine();
            strings = s.Split(' '); // De tekst
            opti opt = new opti(modus, L, strings);
            //Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            opt.calculate();
            if (modus == 1)
            {
                ushort word = 0;
                StringBuilder sb = new StringBuilder();
                string[] route = opt.finalRoute.Split(',');
                for (ushort counter = 0; counter < route.Length; counter++)
                {                    
                    ushort enter = ushort.Parse(route[counter]);
                    for (; word < enter; word++)
                    {
                        sb.Append(strings[word] + " ");
                        
                    }
                    sb.Append(strings[word]);
                    word++;
                    if(counter != route.Length -1)
                        sb.Append("\r\n");
                }
                Console.WriteLine(sb.ToString());
            }
            else Console.WriteLine(opt.finalScore);
            Console.ReadLine();
        }
    }

    class opti{
        ushort L, modus;
        string[] strings;
        ushort lastIndex;
        public ushort finalScore;
        public string finalRoute;
        ushort currentIndex;
        ushort nextIndex;
        ushort[,] scores;
        string[,] routes;

        public opti(ushort modus, ushort L, string[] strings)
        {
            this.L = L;
            this.modus = modus;
            this.strings = strings;
            lastIndex = (ushort)(strings.Length - 1);
            scores = new ushort[2, L + 1];
            /*for (ushort i = 0; i <= lastIndex; i++)
            {
                for (ushort k = 0; k <= L; k++)
                    scores[i, k] = 65535;
            }*/
            routes = new string[2, L + 1];
            currentIndex = 0;
            nextIndex = 1;
        }

        void score(ushort n, ushort regelLengte){            
            if (modus == 1)
            {
                if (n < lastIndex)
                {
                    string woord = strings[n];
                    short nieuweLengte = (short)(regelLengte - woord.Length - 1); // -1 voor spatie
                    ushort firstScore;
                    char c = woord[woord.Length - 1];
                    ushort isLeesteken = (ushort)((c == '?' | c == '.' | c == '!') ? 0 : 1);
                    ushort enterScore = (ushort)(scores[currentIndex,L] + isLeesteken * (nieuweLengte + 1) * (nieuweLengte + 1)); // enter erbij
                    if (nieuweLengte >= strings[n + 1].Length) 
                    {
                        firstScore = scores[currentIndex, nieuweLengte];
                        if (firstScore < enterScore)
                        {
                            scores[nextIndex, regelLengte] = firstScore;
                            routes[nextIndex, regelLengte] = routes[currentIndex, nieuweLengte];
                        }
                        else
                        {
                            scores[nextIndex, regelLengte] = enterScore;
                            routes[nextIndex, regelLengte] = n + "," + routes[currentIndex, L];
                        }
                    }
                    else
                    {
                        scores[nextIndex, regelLengte] = enterScore;
                        routes[nextIndex, regelLengte] = n + "," + routes[currentIndex, L];
                    }
                    
                }
                else if (n == lastIndex)
                {
                    string woord = strings[n];
                    short nieuweLengte = (short)(regelLengte - woord.Length - 1);
                    char c = woord[woord.Length - 1];
                    ushort isLeesteken = (ushort)((c == '?' | c == '.' | c == '!') ? 0 : 1);
                    ushort enterScore = (ushort)(isLeesteken * (nieuweLengte + 1) * (nieuweLengte + 1));
                    scores[currentIndex, regelLengte] = enterScore;
                    routes[currentIndex, regelLengte] = n.ToString();
                }
            }
            else
            {
                if (n < lastIndex)
                {
                    string woord = strings[n];
                    short nieuweLengte = (short)(regelLengte - woord.Length - 1); // -1 voor spatie
                    ushort firstScore;
                    char c = woord[woord.Length - 1];
                    ushort isLeesteken = (ushort)((c == '?' | c == '.' | c == '!') ? 0 : 1);
                    ushort enterScore = (ushort)(scores[currentIndex, L] + isLeesteken * (nieuweLengte + 1) * (nieuweLengte + 1)); // enter erbij
                    if (nieuweLengte >= strings[n + 1].Length) 
                    {
                        firstScore = scores[currentIndex, nieuweLengte];
                        if (firstScore < enterScore)
                        {
                            scores[nextIndex, regelLengte] = firstScore;
                        }
                        else
                        {
                            scores[nextIndex, regelLengte] = enterScore;
                        }
                    }
                    else
                    {
                        scores[nextIndex, regelLengte] = enterScore;
                    }
                }
                else if (n == lastIndex)
                {
                    string woord = strings[n];
                    short nieuweLengte = (short)(regelLengte - woord.Length - 1);
                    char c = woord[woord.Length - 1];
                    ushort isLeesteken = (ushort)((c == '?' | c == '.' | c == '!') ? 0 : 1);
                    ushort enterScore = (ushort)(isLeesteken * (nieuweLengte + 1) * (nieuweLengte + 1));
                    scores[currentIndex, regelLengte] = enterScore;
                }                
            }
        }

        public void calculate()
        {
            for (int i = strings[lastIndex].Length; i <= L; i++)
            {
                score((ushort)lastIndex, (ushort)i);
            }
            ushort temp;
            for (int i = lastIndex - 1; i >= 0; i--)
            {
                for (int k = L; k >= 0; k--)
                    score((ushort)i, (ushort)k);
                temp = currentIndex;
                currentIndex = nextIndex;
                nextIndex = temp;
            }
            finalScore = scores[currentIndex, L];
            if(modus == 1)
                finalRoute = routes[currentIndex, L];
        }        
    }
}
