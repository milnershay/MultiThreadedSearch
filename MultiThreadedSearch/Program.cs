using System;
using System.IO;
using System.Threading;

namespace MultiThreadedSearch
{
    class Program
    {
        private static string[] lines;
        private static char[] target;
        private static int n, d, length;
        private static bool flag = false;
        static void Main(string[] args)
        {
            lines = File.ReadAllLines(args[0]);
            target = args[1].ToCharArray();
            length = lines.Length;
            int.TryParse(args[2], out n);
            int.TryParse(args[3], out d);
         //   Console.WriteLine("********\nNumber of lines = {0}\nTarget string = {1}\nnThreads = {2}\nDelta = {3}\n**********", lines.Length, (target.ToString()), n, d );
            int jmp = length / n;
            int strt = 0, fin = jmp ;
            Thread[] threads = new Thread[n];
            for (int i = 0; i < n; i++)
            {
                if ((fin) < (length -1))
                {
                    threads[i] = new Thread(() => SearchLines(strt, strt + jmp));
                    threads[i].Start();
                    strt = fin +1;
                    fin = fin + jmp;
                }
                else
                {
                    threads[i] = new Thread(() => SearchLines(strt, (length - 1)));
                    threads[i].Start();
                }
            }


            Thread.Sleep(300);
            if(flag == false)
            {
                Console.WriteLine("Not found.");
            }
        }


        static async void SearchLines(int s, int f)
        {
 
            for (int r = s; r <= f; r++)
            {
                //Console.WriteLine("********\nr= {0}\nf= {1}\n lines.lenght = {2}*********", r, f, lines.Length);
                int i = 0;
                foreach (char c in lines[r])
                {
                    if (c == target[0])
                    {
                        SearchFrom(r, i);
                    }
                    i++;
                }
            }

        }

        //          [a,b,c,d,e]  index = 1 ('b), length = 5, delta = 4,     if(index+delta >= length - 1) go down a row and go to index = length - index - delta


        static void SearchFrom(int r, int i)
        {
            int j = 1;
            int t = i;
            while(j < target.Length)
            {
                if(i + d + 1 >= lines[r].Length)
                {
                  //  Console.WriteLine("r= {3} \t r.length = {0} \t d = {1} \t i = {2}", lines[r].Length, d, i, r);

                    i = i + d + 1 - lines[r].Length;
                    r++;

                 //   Console.WriteLine("r= {3} \t r.length = {0} \t d = {1} \t i = {2}", lines[r].Length, d, i, r);

                }
                else
                {
                        i += d + 1;
                }

                if (lines[r][i] != target[j])
                {
                    break;
                }

                j++;
            }
            if(j == target.Length)
            {
                Console.WriteLine("[{0},{1}]", r, t);
                flag = true;
            }
        }
    }
}
