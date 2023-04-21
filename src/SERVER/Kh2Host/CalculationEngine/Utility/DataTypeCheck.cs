using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationEngine.Utility
{
    public class DataTypeCheck
    {
        public const double PI = 3.14159265358979323846;
        public static string[] OperandFunctions = { "avg", "avg20", "abs", "pow", "sqrt", "not", "sum",
                                                    "sin", "cos", "tan", "acos", "asin", "atan",
                                                    /*"sinr", "cosr", "tanr", "acosr", "asinr", "atanr",*/
                                                    "max", "min", "exp", "log", "log10",
                                                    "ceil", "floor", "round",
                                                    "meterh", "meterd", "meterm"};
        public static string[] ArithOperators = { "*", "/", "%", "+", "-" };
        public static string[] LogicalOperators = { "and", "or", "nor", "nand", "xor" };


        /// <summary>
        /// All digits....no decimals, commas, or -
        /// </summary>
        /// <param name="CheckString"></param>
        /// <returns></returns>
        public static bool IsAllDigits(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return false;

            CheckString = CheckString.Trim();

            bool allDigits = true;
            foreach (char c in CheckString)
            {
                if (Char.IsDigit(c) == false)
                {
                    allDigits = false;
                    break;
                }
            }
            
            return allDigits;
        }

        /// <summary>
        /// The number of '.' in a string
        /// </summary>
        /// <param name="CheckString"></param>
        /// <returns></returns>
        public static int DecimalCount(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return 0;

            CheckString = CheckString.Trim();

            int decimalCount = 0;
            foreach (char c in CheckString)
            {
                if (c == '.') decimalCount++;
            }

            return decimalCount;
        }


        /// <summary>
        /// Text items are surronded by strings.
        /// </summary>
        /// <param name="CheckString"></param>
        /// <returns></returns>
        public static bool IsText(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return false;

            CheckString = CheckString.Trim();

            if (CheckString.Length == 1) return false;
            if (CheckString.StartsWith("\"") == false) return false;
            if (CheckString.EndsWith("\"") == false) return false;

            return true;
        }


        /// <summary>
        /// integers have all digits and no decimals.  they can also start with -
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static bool IsInteger(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return false;

            CheckString = CheckString.Trim();

            bool isInteger = true;  // assume the value is an integer
            int intPosition = 0; // the current position we are checking

            foreach (char c in CheckString)
            {
                if (Char.IsNumber(c) == false)
                {
                    if (c != '-')
                    {
                        isInteger = false;
                        break;
                    }
                    else if (intPosition != 0)
                    {
                        isInteger = false;
                        break;
                    }
                }

                intPosition++;
            }

            return isInteger;
        }

        public static bool IsDate(string CheckString)
        {

            if (String.IsNullOrEmpty(CheckString) == true) return false;
            CheckString = CheckString.Trim();

            DateTime d = DateTime.MinValue;
            return DateTime.TryParse(CheckString, out d);
        }

        public static bool IsDouble(string CheckString)
        {
            bool isDouble = true; // assume the item is a double

            if (String.IsNullOrEmpty(CheckString) == true) return false;

            CheckString = CheckString.Trim();

            double dRet;
            isDouble = double.TryParse(CheckString, out dRet);

            return isDouble;
#if false

            bool isDouble = true; // assume the item is a double
            int decimalCount = 0; // the number of decimals

            int intPosition = 0;

            foreach (char c in CheckString)
            {
                if (Char.IsNumber(c) == false)
                {
                    if (c == '.')
                    {
                        decimalCount++;
                    }
                    else if ((c == '-') && (intPosition == 0))
                    {
                        // this is valid, keep going
                    }
                    else
                    {
                        isDouble = false;
                        break;
                    }
                }

                intPosition++;
            }

            if (isDouble == true)
            {
                // make sure there is only 1 decimal

                if (decimalCount <= 1)
                    return true;
                else
                    return false;

            }
            else
                return false;
#endif
        }

        public static bool AnyPuncuation(string Text, out Char PuncMark)
        {
            PuncMark = ' ';
            foreach (char c in Text)
            {
                if (Char.IsPunctuation(c) == true)
                {
                    PuncMark = c;
                    return true;
                }
            }

            return false;
        }

        public static bool IsOperandFunction(string OperandText)
        {
            if (String.IsNullOrEmpty(OperandText) == true) return false;

            OperandText = OperandText.Trim().ToLower();

            bool isOperandFunnction = false;

            for (int i = 0; i < OperandFunctions.Length; i++)
            {
                if (OperandText == OperandFunctions[i])
                {
                    isOperandFunnction = true;
                    break;
                }
            }

            return isOperandFunnction;
        }

        public static bool IsOperator(string OperatorText)
        {

            if (String.IsNullOrEmpty(OperatorText) == true) return false;

            OperatorText = OperatorText.Trim().ToLower();
            bool isOperator = false;

            for (int i = 0; i < ArithOperators.Length; i++)
            {
                if (OperatorText == ArithOperators[i])
                {
                    isOperator = true;
                    break;
                }
            }
            if (isOperator == true) return isOperator;



            for (int i = 0; i < LogicalOperators.Length; i++)
            {
                if (OperatorText == LogicalOperators[i])
                {
                    isOperator = true;
                    break;
                }
            }
            if (isOperator == true) return isOperator;

            return false;
        }

        public static bool IsBoolean(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return false;

            CheckString = CheckString.Trim().ToLower();

            if (CheckString == "true") return true;
            if (CheckString == "false") return true;

            return false;

        }

        public static bool IsPointSelection(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) 
                return false;
            string tempString = CheckString.Trim();
            if (string.IsNullOrEmpty(tempString) == true)
                return false;

            // 처음과 마지막이 Double quotantion mark(")이어야 한다.
            if (tempString.StartsWith("\"") == false)
                return false;
            if (tempString.EndsWith("\"") == false )
                return false;

            // 포인트 Token은 4810/DL/GA11481000001/current_a 형식이어야 한다. 
            // 계산포인트 CAL/CAL1234/value
            tempString = tempString.Substring(1, tempString.Length - 2);
            string[] elements = tempString.Split('/');
            string strFirst = elements[0].ToUpper();
            if (elements.Length != 3)
                return false;
#if false
            if (strFirst.Equals("CAL"))
            {
                if (elements.Length != 3)
                    return false;
            }
            else
            {
                if (elements.Length != 4)
                    return false;
            }
#endif

            return true;
        }

        public static string RemoveTextQuotes(string CheckString)
        {
            if (IsText(CheckString) == false) return CheckString;

            return CheckString.Substring(1, CheckString.Length - 2);
        }

        public static void FunctionDescription(string FunctionName, out string Syntax, out string Description, out string Example)
        {
            Syntax = "Unknown function";
            Description = "";
            Example = "";

            switch (FunctionName.Trim().ToLower())
            {
#region 삼각함수

                case "cos":
                    Description = "Returns the cosine of an angle of x degrees.";
                    Syntax = "cos(x)\nx representing an angle expressed in degrees.\n";
                    Example = "cos(60) = 0.5";
                    break;

                case "sin":
                    Description = "Returns the sine of an angle of x degrees.";
                    Syntax = "sin(x)\nx representing an angle expressed in degrees.\n";
                    Example = "sin(30) = 0.5";
                    break;

                case "tan":
                    Description = "Returns the tangent of an angle of x degrees.";
                    Syntax = "tan(x)\nx representing an angle expressed in degrees.\n";
                    Example = "tan(45) = 1.00";
                    break;

                case "acos":
                    Description = "Returns the value of arc cosine of x, in the interval [0,180] degrees";
                    Syntax = "acos(x)\nx whose arc cosine is computed, in the interval [-1,+1]\n";
                    Example = "acos(0.5) = 60";
                    break;

                case "asin":
                    Description = "Returns the value of arc sine of x, in the interval [-90,90] degrees";
                    Syntax = "asin(x)\nx whose arc cosine is computed, in the interval [-1,+1]\n";
                    Example = "asin(0.5) = 30";
                    break;

                case "atan":
                    Description = "Returns the value of arc tangent of x, in the interval [-90,90] degree";
                    Syntax = "atan(x)\nx whose arc tangent is computed.";
                    Example = "atan(1.0) = 45";
                    break;

                case "cosr":
                    Description = "Returns the cosine of an angle of x radians.";
                    Syntax = "cos(x)\nx representing an angle expressed in radians.";
                    Example = "cosr(60*PI/180) = 0.5, PI = 3.1415926";
                    break;

                case "sinr":
                    Description = "Returns the sine of an angle of x radians.";
                    Syntax = "sin(x)\nx representing an angle expressed in radians.";
                    Example = "sinr(30*PI/180) = 0.5, PI = 3.1415926";
                    break;

                case "tanr":
                    Description = "Returns the tangent of an angle of x radians.";
                    Syntax = "tan(x)\nx representing an angle expressed in radians.\n";
                    Example = "tanr(45*PI/180) = 1.00, PI = 3.1415926";
                    break;

                case "acosr":
                    Description = "Returns the value of arc cosine of x, in the interval [0,PI] radians";
                    Syntax = "acos(x)\nx whose arc cosine is computed, in the interval [-1,+1]\n";
                    Example = "acosr(0.5) = 60*PI/180, PI = 3.1415926";
                    break;

                case "asinr":
                    Description = "Returns the value of arc sine of x, in the interval [-PI/2,PI/2] radians";
                    Syntax = "asin(x)\nx whose arc cosine is computed, in the interval [-1,+1]\n";
                    Example = "asinr(0.5) = 30*PI/180, PI = 3.1415926";
                    break;

                case "atanr":
                    Description = "Returns the value of arc tangent of x, in the interval [-PI/2,PI/2] radians";
                    Syntax = "atan(x)\nx whose arc tangent is computed.";
                    Example = "atanr(1.0) = 45*PI/180, PI = 3.1415926";
                    break;

#endregion

#region 통계함수

                case "avg":
                    Description = "Returns the average of values which is in a list.";
                    Syntax = "avg(p1, ..., pn)";
                    Example = "avg(1, 2, 3) = 2";
                    break;

                case "max":
                    Description = "Returns the maximum value in a list";
                    Syntax = "max(p1, ..., pn)";
                    Example = "max(2,10,4) = 10";
                    break;

                case "min":
                    Description = "Returns the minimum value in a list";
                    Syntax = "min(p1, ..., pn)";
                    Example = "min(2,10,4) = 2";
                    break;

                case "sum":
                    Description = "Returns the sum of values which is in a list";
                    Syntax = "sum(p1, ..., pn)";
                    Example = "sum(2,10,4) = 16";
                    break;

#endregion

#region 공학함수

                case "abs":
                    Description = "Returns the absolute value of x: |x|.";
                    Syntax = "abs(x)";
                    Example = "abs(-10) = 10";
                    break;

                case "round":
                    Description = "Returns the integral value that is nearest to x, with halfway cases rounded away from zero.";
                    Syntax = "round(x, d)/nx is the value to round/nd is the number of decimal places.";
                    Example = "round(123.45,1) = 123.5";
                    break;

                case "ceil" :
                    Description = "Rounds x upward, retuning the smallest integral value that is not less than x.";
                    Syntax = "ceil(x)/nx is value of round up.";
                    Example = "ceil(2.3) = 3";
                    break;

                case "floor" :
                    Description = "Rounds x downward, retuning the largest integral value that is not greater than x.";
                    Syntax = "floor(x)/nx is value of round down.";
                    Example = "floor(2.8) = 2";
                    break;

                case "sqrt":
                    Description = "Returns the square root of x.";
                    Syntax = "sqrt(a)/nx is Value whose square root is computed.\nIf x is negative, error occurs.";
                    Example = "sqrt(25) = 5";
                    break;

                case "pow":
                    Description = "Returns x raised to the power e.\nIf e is 0 then reruns 1.\nIf both x and e are zero, error occrues.";
                    Syntax = "pow(x,e)\nx is the value to pow\ne is the exponent value";
                    Example = "pow(-3,3) = -27";
                    break;

                case "exp":
                    Description = "Returns the base-e exponential function of x, which is e raised to the power x: e^x.";
                    Syntax = "exp(x)/nx is the value of the exponent.";
                    Example = "exp(5) = 148.413159";
                    break;

                case "log":
                    Description = "Returns the base-e logarithm of x.";
                    Syntax = "log(x)/nx is the value whose logarithm is calculated.\nIf x is negative, error occurs.";
                    Example = "log(5.5) = 1.704748";
                    break;

                case "log10":
                    Description = "Returns the base-10 logarithm of x.";
                    Syntax = "log10(x)/nx is the value whose logarithm is calculated.\nIf x is negative, error occurs.";
                    Example = "log10(1000) = 3";
                    break;

#endregion

#region 논리함수

                case "not":
                    Description = "Performs a NOT of x";
                    Syntax = "not(x)";
                    Example = "not(0) = 1";
                    break;

#endregion

#region 계량함수

                case "meterh":
                    Description = "Perfoms the metering of x hourly.";
                    Syntax = "meterh(x)\nx is the realtime data point.";
                    Example = "meterh(\"DL/GA00001/total_gen_power\")";
                    break;

                case "meterd":
                    Description = "Perfoms the metering of x at the sepecific hour per daily.";
                    Syntax = "meterd(x,h)\nx is the realtime data point.\nh is the hour";
                    Example = "meterd(\"DL/GA00001/total_gen_power\", 1)\nPerfors the metering at 1 AM per daily.";
                    break;

                case "meterm":
                    Description = "Perfoms the metering of x at the sepecific day/hour per daily.";
                    Syntax = "meterm(x,d,h)\nx is the realtime data point.\nd and h is the day and hour.";
                    Example = "meterm(\"DL/GA00001/total_gen_power\", 1, 1)\nPerfors the metering at first day and 1 AM per monthly.";
                    break;

#endregion

            }

        }


        public static bool IsNULL(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return false;
            CheckString = CheckString.Trim().ToLower();

            return (CheckString == "null");

        }

        public static bool ContainsOperator(string CheckString, out string sOperand, out string sOperator)
        {
            // initialize the outgoing variables
            sOperand = "";
            sOperator = "";

            // clean the formular
            if (String.IsNullOrEmpty(CheckString) == true) return false;
            CheckString = CheckString.Trim();

            // loop through the arith. operators
            bool containsOperator = false; // assume an operator is not in the opperand
            for (int i = 0; i < Utility.DataTypeCheck.ArithOperators.Length; i++)
            {
                if (CheckString.EndsWith(Utility.DataTypeCheck.ArithOperators[i]) == true)
                {
                    containsOperator = true;
                    sOperator = Utility.DataTypeCheck.ArithOperators[i];
                    sOperand = CheckString.Substring(0, CheckString.Length - sOperator.Length);
                    break;
                }
            }
            if (containsOperator == true) 
                return true;

            return false;
        }

    }
}
