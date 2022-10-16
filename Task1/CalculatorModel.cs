using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parser = org.mariuszgromada.math.mxparser;

namespace Task1
{
    public static class CalculatorModel
    {
        public static string ParserCalculate(string InProgressString, string InProgressNumber)
        {
            string s = InProgressString
                                .Replace('×', '*')
                                .Replace('÷', '/')
                                .Replace('−', '-')
                                .Replace(',', '.');
            parser.Expression ex = new parser.Expression(s);
            double result = ex.calculate();
            if (result.ToString() != double.NaN.ToString())
            {
                InProgressNumber = result.ToString(); 
            }
            else
            {               
                InProgressNumber = "Некорректный ввод";                
            }
            return InProgressNumber;
        }
    }
}
