using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB_4_TIPIS
{
    class HammingCode
    {

        static char[,] G = new char[,] {
                { '1', '1', '1','0', '0','0','0'},
                { '1', '0', '0','1', '1','0','0'},
                { '0', '1', '0','1', '0','1','0'},
                { '1', '1', '0','1', '0','0','1'},
            };
        static char[,] H = new char[,] {
                { '0', '0', '0','1', '1','1','1'},
                { '0', '1', '1','0', '0','1','1'},
                { '1', '0', '1','0', '1','0','1'},
            };
        /// <summary>
        /// Возвращает закодированную строку
        /// </summary>
        /// <param name="code">code lenght must be 4</param>
        /// <returns></returns>
        public static string Encode(string code)
        {
            string result = "";

            if (code.Length == G.GetLength(0))
            {
                char c = ' ';
                for (int i = 0; i < G.GetLength(1); i++)
                {
                    c = ' ';
                    char tmp;
                    for (int j = 0; j < G.GetLength(0); j++)
                    {
                        if (G[j, i] == '1' && code[j] == '1') //AND
                            tmp = '1';
                        else
                            tmp = '0';

                        if (c == ' ')
                            c = tmp;
                        else
                        {
                            if (c != tmp) //XOR
                                c = '1';
                            else
                                c = '0';

                        }
                    }
                    result += c;
                }

            }
            else
                result = "Wrong lenght";

            return result;
        }
        /// <summary>
        ///  Декодироет строку
        /// </summary>
        /// <param name="code">code lenght must be 7</param>
        public static string Decode(string code)
        {
            string sindrom = "";
            string result = "";
            bool bad = false;

            if (code.Length == H.GetLength(1))
            {
                char c = ' ';
                for (int i = 0; i < H.GetLength(0); i++)
                {
                    c = ' ';
                    char tmp;
                    for (int j = 0; j < H.GetLength(1); j++)
                    {
                        if (H[i, j] == '1' && code[j] == '1') //AND
                            tmp = '1';
                        else
                            tmp = '0';

                        if (c == ' ')
                            c = tmp;
                        else
                        {
                            if (c != tmp) //XOR
                                c = '1';
                            else
                                c = '0';
                        }
                    }
                    sindrom += c;
                    if (c == '1')
                        bad = true;
                }
                int error = Convert.ToInt32(sindrom, 2);
                for (int i = 0; i < H.GetLength(1); i++)
                {
                    result += code[i];
                }

                if (bad)
                {
                    result = "";
                    for (int i = 0; i < H.GetLength(1); i++)
                    {
                        if (i + 1 == error)
                        {
                            if (code[i] == '1')
                                result += '0';
                            else
                                result += '1';
                        }
                        else
                            result += code[i];
                    }
                    
                }
                

                string res2 = "";
                for (int i = 0; i < result.Length; i++)
                {
                    if(((i+1 & (i)) != 0))
                    {
                        res2 += result[i];
                        
                    }
                }
                return res2;

            }
            else
                Console.WriteLine("Wrong lenght");
            return null;
        }

    }
}

