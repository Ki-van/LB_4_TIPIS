using System;
using LB_2_TIPIS;

namespace LB_4_TIPIS
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] symbols = { "в", "ы", "о", "і", "с", "п", "у", "з", "е", "т", "э", "й", "г", "б", "ў" };
            double[] probs = { 0.076, 0.109, 0.068, 0.133, 0.129, 0.066, 0.065, 0.052, 0.064, 0.068, 0.025, 0.030, 0.039, 0.036, 0.041 };
            ApriorEntropy entropy = new ApriorEntropy(0.65, 0.35, 0.35, 0.65, symbols, probs);

            Console.WriteLine("Канальная матрица: ");
            for (int i = 0; i < entropy.channelMatrix.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < entropy.channelMatrix.GetUpperBound(1) + 1; j++)
                {
                    Console.Write(String.Format("{0:f3} ", entropy.channelMatrix[i, j]));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Энтропия источника: " + entropy.SourceEntropy);
            Console.WriteLine("Энтропия приемника: " + entropy.ReceiverEntropy);
            Console.WriteLine("Энтропия шума: " + entropy.EntropyOfNoise);
            Console.WriteLine("Апостериорная энтропия: " + entropy.PosteriorEntropy);
            Console.WriteLine("Полезная информация: " + entropy.UsefulInformation);
            Console.WriteLine("Скорость: " + entropy.Speed);


            Console.WriteLine("1011");
            

            string code2 = CyclicСode.Encode("1011");
            Console.WriteLine(code2);

        }
    }
}
