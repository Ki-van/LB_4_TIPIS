using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LB_4_TIPIS;

namespace LB_2_TIPIS
{
    class ApriorEntropy
    {
        private struct Symbol{
            public string code;
            public double probability;
            public string symbol;
            public string antiJammingCode;
        };

        private double ZeroToZero, ZeroToOne, OneToZero, OneToOne;
        private Symbol[] Symbols;
        public double SourceEntropy, ReceiverEntropy, EntropyOfNoise, PosteriorEntropy, UsefulInformation, Speed;
        public double[,] channelMatrix;

        public ApriorEntropy(double zeroToZero, double zeroToOne, double oneToZero, double oneToOne, string[] symbols, double[] probabilitys)
        {
            ZeroToZero = zeroToZero;
            ZeroToOne = zeroToOne;
            OneToZero = oneToZero;
            OneToOne = oneToOne;

            InitSymbols(symbols, probabilitys);
            GetChannelMatrix(symbols);
            CalcSourceEntropy();
            CalcReceiverEntropy();
            CalcEntropyOfNoise();
            CalcUsefulInformation();
            CalcPosteriorEntropy();
            CalcSpeed();
        }

        private void CalcSpeed()
        {
            Speed = UsefulInformation / 0.0012;
        }

        private void CalcUsefulInformation()
        {
            UsefulInformation = ReceiverEntropy - EntropyOfNoise;
        }

        private void CalcPosteriorEntropy()
        {
            PosteriorEntropy = SourceEntropy - UsefulInformation;
        }

        private void CalcEntropyOfNoise()
        {
            double H = 0, Hvui;
            for(int i = 0; i < channelMatrix.GetUpperBound(0) + 1; i++)
            {
                Hvui = 0;
                for (int j = 0; j < channelMatrix.GetUpperBound(1) + 1; j++)
                {
                    if (channelMatrix[i, j] == 0)
                        continue;
                    Hvui += -channelMatrix[i, j] * Math.Log2(channelMatrix[i, j]);
                }
                H += Symbols[i].probability * Hvui;
            }
            EntropyOfNoise = H;
        }

        private void CalcReceiverEntropy()
        {

            double p, H = 1;
            for(int i = 0; i < channelMatrix.GetUpperBound(1) + 1; i++)
            {
                p = 0;
                for(int j = 0; j < channelMatrix.GetUpperBound(0) + 1; j++)
                {
                    p += Symbols[j].probability*channelMatrix[j, i];
                }
                H += -p * Math.Log2(p);
            }
            ReceiverEntropy = H;
        }

        private void CalcSourceEntropy()
        {
            double H = 0;
            foreach (var c in Symbols)
                H += -c.probability * Math.Log2(c.probability);

            SourceEntropy = H;
        }

        private bool InitSymbols(string[] symbols, double[] probabilitys)
        {
            if (symbols.Length != probabilitys.Length)
                return false;

            Symbols = new Symbol[symbols.Length];
            int codeLenght = (int)Math.Ceiling(Math.Log2(symbols.Length));

            for (int i = 0; i < Symbols.Length; i++)
            {
                Symbols[i].symbol = symbols[i];
                Symbols[i].probability = probabilitys[i];
                Symbols[i].code = ToBinaryStr(i + 1, codeLenght);
                Symbols[i].antiJammingCode = MatrixCode.Encode(Symbols[i].code);
            }

            return true;
        }

        /*private  void GetChannelMatrix(string[] symbols)
        {
            if (symbols.Length < 1)
            {
                channelMatrix = null;
                return;
            }
            int minCodeLenght = (int)Math.Ceiling(Math.Log2(symbols.Length));
            this.channelMatrix = new double[symbols.Length, (int) Math.Pow(2, minCodeLenght)];

            string fromCode, toCode;
            for(int i = 0; i < channelMatrix.GetUpperBound(0) + 1; i++)
            {
                fromCode = Symbols[i].antiJammingCode;
                int maxV = (int)Math.Pow(2, Symbols[i].antiJammingCode.Length);
                int maxU = (int)Math.Pow(2, Symbols[i].code.Length);
                for (int j = 0; j < maxV; j++)
                {
                    double pvu = 1;
                    
                    toCode = ToBinaryStr(j + 1, Symbols[i].antiJammingCode.Length);

                    for (int k = 0; k < fromCode.Length; k++)
                    {
                        if(fromCode[k] == '0')
                        {
                            if (toCode[k] == '0')
                                pvu *= this.ZeroToZero;
                            else
                                pvu *= this.ZeroToOne;
                        }
                        else
                        {
                            if (toCode[k] == '0')
                                pvu *= this.OneToZero;
                            else
                                pvu *= this.OneToOne;
                        }
                    }
                    channelMatrix[i, (Convert.ToInt32(MatrixCode.Decode(toCode), 2) - 1 + maxU) % maxU] += pvu;
                    
                }
            }
        }*/
        private void GetChannelMatrix(string[] symbols)
        {
            if (symbols.Length < 1)
            {
                channelMatrix = null;
                return;
            }
            int minCodeLenght = (int)Math.Ceiling(Math.Log2(symbols.Length));
            this.channelMatrix = new double[symbols.Length, (int)Math.Pow(2, minCodeLenght)];

            string fromCode, toCode;
            for (int i = 0; i < channelMatrix.GetUpperBound(0) + 1; i++)
            {
                fromCode = Symbols[i].code;
                for (int j = 0; j < channelMatrix.GetUpperBound(1) + 1; j++)
                {
                    double pvu = 1;
                    //channelMatrix[i, j] = 1;
                    toCode = ToBinaryStr(j + 1, minCodeLenght);

                    for (int k = 0; k < fromCode.Length; k++)
                    {
                        if (fromCode[k] == '0')
                        {
                            if (toCode[k] == '0')
                                pvu *= this.ZeroToZero;
                            else
                                pvu *= this.ZeroToOne;
                        }
                        else
                        {
                            if (toCode[k] == '0')
                                pvu *= this.OneToZero;
                            else
                                pvu *= this.OneToOne;
                        }
                    }

                    if (HemmingDistahce(fromCode, toCode) <= 1)
                    {
                        channelMatrix[i, i] += pvu;
                    }
                    else
                        channelMatrix[i, j] += pvu;

                }
            }
        }

        private string ToBinaryStr(int a, int strLength)
        {
            string str = Convert.ToString(a, 2);
            string strAlign = "";
            for (int j = 0; j <  strLength - str.Length; j++)
                strAlign += "0";
            str = strAlign + str;

            if (strLength < str.Length)
                str = str.Substring(str.Length - strLength, strLength);

            return str;
        }

        private int HemmingDistahce(string code1, string code2)
        {
            if (code1.Length != code2.Length)
                return -1;

            int d = 0;
            for (int i = 0; i < code1.Length; i++)
            {
                d += code1[i] != code2[i] ? 1 : 0;
            }

            return d;
        }
    }
}
