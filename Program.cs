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
            opt.calculate();
            if (modus == 1)
            {
                ushort word = 0;
                //string copy = "";
                StringBuilder sb = new StringBuilder();
                string[] route = opt.finalRoute.Split(',');
                for (ushort counter = 0; counter < route.Length; counter++)
                {
                    ushort enter = ushort.Parse(route[counter]);
                    for (; word < enter; word++)
                    {
                        //copy += strings[word];                        
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
                    /*if (scores[n, regelLengte] != 0)
                        return;*/
                    string woord = strings[n];
                    short nieuweLengte = (short)(regelLengte - woord.Length - 1); // -1 voor spatie
                    ushort firstScore;
                    char c = woord[woord.Length - 1];
                    ushort isLeesteken = (ushort)((c == '?' | c == '.' | c == '!') ? 0 : 1);
                    ushort enterScore = (ushort)(scores[currentIndex,L] + isLeesteken * (nieuweLengte + 1) * (nieuweLengte + 1)); // enter erbij
                    if (nieuweLengte >= strings[n + 1].Length) // 1 spatie te veel, dus plaats voor 1 letter, groter dan 0 denk ik (>=0 safe)
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

        /*ushort opt(ushort i, ushort j)
        {
            if (i == j)
                return Functie(strings[i]);
            ushort sum = 0;
            for (ushort k = i; k <= j; k++)
            {
                sum += (ushort)(strings[k].Length +1); //+1 voor de spatie
                if(sum -1 > L)
                    break;
            }
            sum -= 1;
            if(sum>L){
                ushort min = (ushort)(opt(i, i) + opt((ushort)(i+1), j));
                for(ushort r = (ushort)(i+1); r<j; r++)
                {
                    ushort score = (ushort)(opt(i, r) + opt((ushort)(r+1), j));
                    if(score < min){
                        min = score;
                    }
                }
                return min;
            }
            else{
                // Denk dat het zonder onderstaande al werkt, maar not sure
                 ushort min = (ushort)(opt(i, i) + opt((ushort)(i+1), j));
                for(ushort r = (ushort)(i+1); r<j; r++)
                {
                    ushort score = (ushort)(opt(i, r) + opt((ushort)(r+1), j));
                    if(score < min){
                        min = score;
                    }
                }
                return min; 
                char c = strings[j].Last();
                ushort isLeesteken = (ushort)((c == '?' | c == '.' | c == '!') ? 0 : 1);
                return (ushort)((L - sum) * isLeesteken);
            }
        }*/

        /*public void calculator()
        {
            ushort[,] scores = new ushort[lastIndex + 1, lastIndex + 1];
            if (modus == 1)
            {
                string[,] routes = new string[lastIndex + 1, lastIndex + 1];
                for (ushort r = 0; r <= lastIndex; r++)
                {
                    for (ushort s = 0; s + r <= lastIndex; s++)
                    {
                        if (0 == r)
                        {
                            scores[s, s + r] = Functie(strings[s]);
                            routes[s, s + r] = s.ToString() + ",";
                        }

                        else
                        {
                            ushort sum = 0;
                            for (ushort k = s; k <= s + r; k++)
                            {
                                sum += (ushort)(strings[k].Length + 1); //+1 voor de spatie
                                if (sum - 1 > L)
                                    break;
                            }
                            sum -= 1;
                            if (sum > L)
                            {
                                ushort min = (ushort)(scores[s, s] + scores[s + 1, s + r]);
                                ushort counter = s;
                                for (ushort i = (ushort)(s + 1); i < s + r; i++)
                                {
                                    ushort score = (ushort)(scores[s, i] + scores[i + 1, s + r]);
                                    if (score < min)
                                    {
                                        min = score;
                                        counter = i;
                                    }
                                }
                                scores[s, s + r] = min;
                                routes[s, s + r] = routes[s, counter] + routes[counter + 1, s + r];// counter.ToString() + "," + routes[counter + 1, s + r];
                            }
                            else
                            {
                                /* Denk dat het zonder onderstaande al werkt, maar not sure
                                 * 
                                 * ushort min = (ushort)(opt(i, i) + opt((ushort)(i+1), j));
                                for(ushort r = (ushort)(i+1); r<j; r++)
                                {
                                    ushort score = (ushort)(opt(i, r) + opt((ushort)(r+1), j));
                                    if(score < min){
                                        min = score;
                                    }
                                }
                                return min; 
                                char c = (strings[r + s])[strings[r + s].Length - 1];
                                ushort isLeesteken = (ushort)((c == '?' | c == '.' | c == '!') ? 0 : 1);
                                ushort tempScore = (ushort)(L - sum);
                                ushort min = (ushort)((tempScore * tempScore) * isLeesteken);
                                //ushort min = (ushort)(scores[s, s] + scores[s + 1, s + r]);
                                ushort counter = (ushort)(s+r+1);
                                for (ushort i = s; i < s + r; i++)
                                {
                                    ushort score = (ushort)(scores[s, i] + scores[i + 1, s + r]);
                                    if (score < min)
                                    {
                                        min = score;
                                        counter = i;
                                    }
                                }
                                scores[s, s + r] = min;
                                /*if (counter != s + r + 1)
                                    routes[s, s + r] = routes[s, counter] + routes[counter + 1, s + r];
                                else routes[s, s + r] = (s + r).ToString() + ",";


                                /*char c = strings[r + s].Last();
                                ushort isLeesteken = (ushort)((c == '?' | c == '.' | c == '!') ? 0 : 1);
                                scores[s, s + r] = (ushort)((L - sum) * isLeesteken);
                                routes[s, s + r] = (s + r).ToString() + ",";
                            }
                        }

                    }
                }
                finalScore = scores[0, lastIndex];
                finalRoute = routes[0, lastIndex];
            }
            else
            {
                for (ushort r = 0; r <= lastIndex; r++)
                {
                    for (ushort s = 0; s + r <= lastIndex; s++)
                    {
                        if (0 == r)
                        {
                            scores[s, s + r] = Functie(strings[s]);
                        }

                        else
                        {
                            ushort sum = 0;
                            for (ushort k = s; k <= s + r; k++)
                            {
                                sum += (ushort)(strings[k].Length + 1); //+1 voor de spatie
                                if (sum - 1 > L)
                                    break;
                            }
                            sum -= 1;
                            if (sum > L)
                            {
                                ushort min = (ushort)(scores[s, s] + scores[s + 1, s + r]);
                                for (ushort i = (ushort)(s + 1); i < s + r; i++)
                                {
                                    ushort score = (ushort)(scores[s, i] + scores[i + 1, s + r]);
                                    if (score < min)
                                    {
                                        min = score;
                                    }
                                }
                                scores[s, s + r] = min;
                            }
                            else
                            {
                                /* Denk dat het zonder onderstaande al werkt, maar not sure
                                    * 
                                    * ushort min = (ushort)(opt(i, i) + opt((ushort)(i+1), j));
                                for(ushort r = (ushort)(i+1); r<j; r++)
                                {
                                    ushort score = (ushort)(opt(i, r) + opt((ushort)(r+1), j));
                                    if(score < min){
                                        min = score;
                                    }
                                }
                                return min; 
                                char c = (strings[r + s])[strings[r + s].Length - 1];
                                ushort isLeesteken = (ushort)((c == '?' | c == '.' | c == '!') ? 0 : 1);
                                ushort tempScore = (ushort)(L - sum);
                                ushort min = (ushort)((tempScore * tempScore) * isLeesteken);
                                //ushort min = (ushort)(scores[s, s] + scores[s + 1, s + r]);
                                //ushort counter = (ushort)(s + r + 1);
                                /*for (ushort i = s; i < s + r; i++)
                                {
                                    ushort score = (ushort)(scores[s, i] + scores[i + 1, s + r]);
                                    if (score < min)
                                    {
                                        min = score;
                                        //counter = i;
                                    }
                                }
                                scores[s, s + r] = min;
                            }
                        }

                    }
                }
                finalScore = scores[0, lastIndex];                
            }
        }

        ushort Functie(string k)
        {
            char c = k[k.Length - 1];
            ushort isLeesteken = (ushort)((c == '?' | c == '.' | c == '!') ? 0 : 1);
            ushort score = (ushort)(L - k.Length);
            return (ushort)((score * score) * isLeesteken);
        }*/
    }
}
