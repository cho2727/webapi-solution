using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationEngine
{
    public class Evaluator
    {
        #region 로컬변수

        private CalculationEngine.Formula formula;

        //private double tokenEvalTime = 0;

        #endregion

        #region 생성자

        public Evaluator(CalculationEngine.Formula syntax)
        {
            formula = syntax;
        }

        #endregion

        #region 프로퍼티

        //public double TokenEvalTime
        //{
        //    get
        //    {
        //        return tokenEvalTime;
        //    }
        //}

        #endregion

        #region 함수

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RPNQueue"></param>
        /// <param name="sValue"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool Evaluate(CalculationEngine.Utility.WTSQueue<TokenItem> RPNQueue, out string sValue, out string ErrorMsg)
        {
            bool bRet;
            // 출력변수 초기화
            ErrorMsg = "";
            sValue = "";

            // 최종 계산결과 초기화
            formula.LastEvaluationResult = "";

            // 계산처리 소요시간 측정 스톱워치 시작
            System.Diagnostics.Stopwatch evalTime = System.Diagnostics.Stopwatch.StartNew();

            // 계산식의 유효성 검사.
            if (formula.AnyErrors == true)
            {
                // 계산식에 오류가 있는 경우, 계산식 오류를 출력한다.
                ErrorMsg = formula.LastErrorMessage;
                return false;
            }

            // 계산을 위한 스택을 생성한다.
            CalculationEngine.Utility.WTSStack<TokenItem> eval = new CalculationEngine.Utility.WTSStack<TokenItem>(formula.TokenItems.Count);

            // 토큰 개수
            int count = RPNQueue.Count;
            // RPN 큐에있는 현재 토큰의 인덱스
            int index = 0;  


            while (index < count)
            {
                // 계산식 토큰 취득
                TokenItem item = RPNQueue[index];
                index++;

                // 데이터 포인트를 위한 변수 토큰인 경우.
                if (item.TokenDataType == CalculationEngine.TokenDataType.Token_DataType_Variable)
                {
                    #region Token_DataType_Variable

                    // determine if we need to assign the variable represented by the formula
                    // or the rule syntax is doing the assignment
                    if ( item.WillBeAssigned == false )
                    {
                        // The rule syntax is not doing the assignment, we are doing it.
                        // lookup the value of the variable and push it onto the evaluation stack
                        if (formula.Variables.VariableExists(item.TokenName) == true)
                        {
                            // the variable exists, push it on the stack
                            eval.Push(new TokenItem(formula.Variables[item.TokenName].VariableValue,
                                                    CalculationEngine.TokenType.Token_Operand,
                                                    item.InOperandFunction, item.TokenName));
                        }
                        else
                        {
                            // the variable does not exist...push an empty string on the stack
                            eval.Push(new TokenItem("", 
                                                    CalculationEngine.TokenType.Token_Operand,
                                                    item.InOperandFunction, item.TokenName));
                        }
                    }
                    else
                    {
                        // the rule syntax is doing the assignment, add the formula item to the evaluation stack
                        eval.Push(item);
                    }

                    #endregion
                }
                else if (item.TokenType == CalculationEngine.TokenType.Token_Operator)
                {
                    #region 연산자(Token_Operator)

                    // 스택에서 두개의 피연산자(데이터값)를 취득하여 계산을 수행하여
                    // 결과를 계산스택에 넣는다.
                    TokenItem rightOperand = null;
                    TokenItem leftOperand = null;
                    try
                    {
                        if (eval.Count > 0) 
                            rightOperand    = eval.Pop();
                        if (eval.Count > 0) 
                            leftOperand     = eval.Pop();
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Evaluate Error : Exception while getting  2 tokens for an operator: " + err.Message;
                        return false;
                    }

                    // 연사자는 반드시 두개의 피연산자가 있어야 한다.
                    if (rightOperand == null)
                    {
                        ErrorMsg = string.Format("( {0} ) 연산은 두개의 피연산자(값)이 필요합니다.", item.TokenName);
                        //ErrorMsg = string.Format("The right operand is null. while ( {0} ) operation.", item.TokenName);
                        return false;
                    }
                    if (leftOperand == null)
                    {
                        ErrorMsg = string.Format("( {0} ) 연산은 두개의 피연산자(값)이 필요합니다.", item.TokenName);
                        //ErrorMsg = string.Format("The left operand is null. while ( {0} ) operation.", item.TokenName);
                        return false;
                    }

                    // process the operator
                    try
                    {
                        TokenItem result = null;
                        bRet = EvaluateTokens(leftOperand, rightOperand, item, out result, out ErrorMsg);
                        if ( bRet == false)
                            return false;
                        else
                        {
                            // 결과에 대한 이중검사.
                            if (result == null)
                            {
                                ErrorMsg = "Evaluate Error : The result of an operator is null.";
                                return false;
                            }
                            else
                            {
                                eval.Push(result);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Evaluate Error : Exception while evaluate token : " + err.Message;
                        return false;
                    }


                    #endregion
                }
                else if (item.TokenType == CalculationEngine.TokenType.Token_Operand_Function_Stop)
                {
                    #region 함수종료토큰 Token_Operand_Function_Stop

                    // 함수시작 토큰이 발견될때까지 토큰을 파라미터 토큰에 넣고
                    // 함수 시작을 발견하면 해당 함수를 실행하여 계산 결과를 계산 스택에 넣는다.

                    // 계산 스택에 있는 토큰 개수
                    int evalCount = eval.Count;
                    TokenItems parameters = new TokenItems(formula);

                    try
                    {
                        for (int j = 0; j < evalCount; j++)
                        {
                            // 계산식 토큰을 얻는다.
                            TokenItem opItem = eval.Pop();

                            // 함수의 시작 토큰인지 확인한다.
                            if (opItem.TokenType == CalculationEngine.TokenType.Token_Operand_Function_Start)
                            {
                                // 함수의 계산을 시작한다.
                                TokenItem result = null;
                                bRet = EvaluateOperandFunction(opItem, parameters, out result, out ErrorMsg);
                                if ( bRet == false)
                                    return false;
                                else
                                {
                                    // 함수처리 결과에 대한 이중검사.
                                    if (result == null)
                                    {
                                        ErrorMsg = String.Format( "Evaluate Error : {0} result is null.\n    MSG : {1}", opItem.TokenName, ErrorMsg );
                                        return false;
                                    }
                                    else
                                        eval.Push(result);
                                }
                                break;
                            }
                            else if (opItem.TokenType != CalculationEngine.TokenType.Token_Operand_Function_Delimiter)
                            {
                                // 함수구분자(,)를 제외한 토큰을 파라미터로 넣는다.
                                parameters.AddToFront(opItem);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Evaluate Error : The evaluation of an operand function threw an error: " + err.Message;
                        return false;
                    }

                    #endregion
                }
                else if (item.TokenType == CalculationEngine.TokenType.Token_Operand_Function_Start)
                {
                    #region 함수 시작 Token_Operand_Function_Start

                    eval.Push(item);

                    #endregion
                }
                else
                {
                    // push the item on the evaluation stack
                    eval.Push(item);
                }
            }

            if (eval.Count == 1)
            {
                // just 1 item on the stack; should be our answer
                try
                {
                    TokenItem final = eval.Pop();
                    sValue = final.TokenName;

                    // set the results in the formula
                    formula.LastEvaluationResult = sValue;
                }
                catch (Exception err)
                {
                    ErrorMsg = "Failed to evaluate the rule expression after all the tokens have been considered: " + err.Message;
                    return false;
                }
            }
            else if (eval.Count == 0)
            {
                // there is no result in the evaluation stack because it my have been assigned
                // do nothing here
            }
            else
            {
                ErrorMsg = "Invalid Rule Syntax";
                return false;
            }


            // stop the timer
            evalTime.Stop();
            //tokenEvalTime = evalTime.Elapsed.TotalMilliseconds;
            //formula.LastEvaluationTime = tokenEvalTime; // set this evaluation time in the formula object.
            formula.LastEvaluationTime = evalTime.Elapsed.TotalMilliseconds; // set this evaluation time in the formula object.
            return true;

        }

        /// <summary>
        /// This new evaluate function includes support to assignment and short circuit of the IIF[] operand function
        /// </summary>
        /// <param name="sValue"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool Evaluate(out string sValue, out string ErrorMsg)
        {
            return Evaluate(formula.RPNQueue, out sValue, out ErrorMsg);
        }

        #endregion

        #region Private Methods

        private bool EvaluateTokens(TokenItem LeftOperand, TokenItem RightOperand, TokenItem Operator, out TokenItem? Result, out string ErrorMsg)
        {
            // 출력 결과 초기화.
            Result = null;
            ErrorMsg = "";

            double dResult = 0;
            bool boolResult = false;

            // 연산자와 피연산자 유효성 검사.
            if (LeftOperand == null)
            {
                ErrorMsg = "Syntax Error : The left formula is null.";
                return false;
            }

            if (RightOperand == null)
            {
                ErrorMsg = "Syntax Error : The right formula is null.";
                return false;
            }

            if (Operator == null)
            {
                ErrorMsg = "Syntax Error : The operator formula is null.";
                return false;
            }


            switch (Operator.TokenName.Trim().ToLower())
            {
                case "^":
                    #region Exponents

                    // Exponents require that both operands can be converted to doubles
                    try
                    {
                        if ((CalculationEngine.Utility.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (CalculationEngine.Utility.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            dResult = Math.Pow(LeftOperand.TokenName_Double, RightOperand.TokenName_Double);
                            Result = new TokenItem(dResult.ToString(), TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values for exponents.";
                            return false;
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Exponent operator: " + err.Message;
                        return false;
                    }
                    break;
                    #endregion

                case "*":
                    #region Multiplication

                    //  multiplication expects that the operands can be converted to doubles
                    try
                    {
                        if ((CalculationEngine.Utility.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (CalculationEngine.Utility.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            dResult = LeftOperand.TokenName_Double * RightOperand.TokenName_Double;
                            Result = new TokenItem(dResult.ToString(), TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values to multiply.";
                            return false;
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Multiplication operator: " + err.Message;
                        return false;
                    }
                    break;
                    #endregion

                case "/":
                    #region Division

                    // divison requires that both operators can be converted to doubles and the denominator is not 0

                    try
                    {
                        if ((CalculationEngine.Utility.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (CalculationEngine.Utility.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            double denominator = RightOperand.TokenName_Double;

                            if (denominator != 0)
                            {
                                dResult = LeftOperand.TokenName_Double / denominator;
                                Result = new TokenItem(dResult.ToString(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                            }
                            else
                            {
                                ErrorMsg = "Syntax Error: Division by zero.";
                                return false;
                            }
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values to divide.";
                            return false;
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Division operator: " + err.Message;
                        return false;
                    }
                    break;
                    #endregion

                case "%":
                    #region Modulus
                    try
                    {
                        // modulus expects that both operators are numeric and the right operand is not zero
                        if ((CalculationEngine.Utility.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (CalculationEngine.Utility.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            double denominator = RightOperand.TokenName_Double;

                            if (denominator != 0)
                            {

                                dResult = LeftOperand.TokenName_Double % RightOperand.TokenName_Double;
                                Result = new TokenItem(dResult.ToString(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                            }
                            else
                            {
                                ErrorMsg = "Syntax Error: Modulus by zero.";
                                return false;
                            }
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values to modulus.";
                            return false;
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Modulus operator: " + err.Message;
                        return false;
                    }
                    break;
                    #endregion

                case "+":
                    #region Addition

                    try
                    {
                        // addition only works on numeric operands
                        if ((CalculationEngine.Utility.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (CalculationEngine.Utility.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            dResult = LeftOperand.TokenName_Double + RightOperand.TokenName_Double;
                            Result = new TokenItem(dResult.ToString(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values to add.";
                            return false;
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Addition operator: " + err.Message;
                        return false;
                    }

                    break;
                    #endregion

                case "-":
                    #region Subtraction
                    try
                    {
                        // subtraction only works on numeric operands
                        if ((CalculationEngine.Utility.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (CalculationEngine.Utility.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            dResult = LeftOperand.TokenName_Double - RightOperand.TokenName_Double;
                            Result = new TokenItem(dResult.ToString(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values to subtract.";
                            return false;
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Subtraction operator: " + err.Message;
                        return false;
                    }
                    break;
                    #endregion

                case "<":
                    #region Less Than
                    if ((CalculationEngine.Utility.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (CalculationEngine.Utility.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a numeric comparison
                            boolResult = (LeftOperand.TokenName_Double < RightOperand.TokenName_Double);
                            Result = new TokenItem(boolResult.ToString().ToLower(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Less Than operator on double operands: " + err.Message;
                            return false;
                        }
                    }
                    else if ((CalculationEngine.Utility.DataTypeCheck.IsDate(LeftOperand.TokenName) == true) && (CalculationEngine.Utility.DataTypeCheck.IsDate(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a date comparison
                            TimeSpan ts = LeftOperand.TokenName_DateTime.Subtract(RightOperand.TokenName_DateTime);
                            boolResult = (ts.TotalDays < 0);
                            Result = new TokenItem(boolResult.ToString().ToLower(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Less Than operator on date operands: " + err.Message;
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            // do a string comparison
                            string lText = CalculationEngine.Utility.DataTypeCheck.RemoveTextQuotes(LeftOperand.TokenName);
                            string rText = CalculationEngine.Utility.DataTypeCheck.RemoveTextQuotes(RightOperand.TokenName);

                            boolResult = (lText.CompareTo(rText) < 0);
                            Result = new TokenItem(boolResult.ToString().ToLower(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Less Than operator on string operands: " + err.Message;
                            return false;
                        }
                    }

                    break;
                    #endregion

                case "and":
                    #region and
                    {
                        bool bLeft = false;
                        bool bRigth = false;

                        if (Convert.ToDouble(LeftOperand.TokenName) == 1.0)
                            bLeft = true;
                        if (Convert.ToDouble(RightOperand.TokenName) == 1.0)
                            bRigth = true;
                        try
                        {
                            boolResult = bLeft && bRigth;
                            double dRet = boolResult ? 1.0 : 0.0;
                            Result = new TokenItem( dRet.ToString().ToLower(), 
                                                    CalculationEngine.TokenType.Token_Operand, 
                                                    CalculationEngine.TokenDataType.Token_DataType_Double, 
                                                    LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the AND operator on boolean operands: " + err.Message;
                            return false;
                        }
                    }

#if false
                    // the and operator must be performed on boolean operators
                    if ((CalculationEngine.Utility.DataTypeCheck.IsBoolean(LeftOperand.TokenName) == true) && (CalculationEngine.Utility.DataTypeCheck.IsBoolean(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            boolResult = LeftOperand.TokenName_Boolean && RightOperand.TokenName_Boolean;
                            Result = new TokenItem(boolResult.ToString().ToLower(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the AND operator on boolean operands: " + err.Message;
                            return false;
                        }
                    }
                    else
                    {
                        ErrorMsg = "Syntax Error: Expecting boolean operands to AND.";
                        return false;
                    }
#endif
                    break;

                    #endregion

                case "or":
                    #region or
                    {
                        bool bLeft = false;
                        bool bRigth = false;

                        if (Convert.ToDouble(LeftOperand.TokenName) == 1.0)
                            bLeft = true;
                        if (Convert.ToDouble(RightOperand.TokenName) == 1.0)
                            bRigth = true;
                        try
                        {
                            boolResult = bLeft || bRigth;
                            double dRet = boolResult ? 1.0 : 0.0;
                            Result = new TokenItem(dRet.ToString().ToLower(),
                                                    CalculationEngine.TokenType.Token_Operand,
                                                    CalculationEngine.TokenDataType.Token_DataType_Double,
                                                    LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the AND operator on boolean operands: " + err.Message;
                            return false;
                        }
                    }

#if false
                    if ((CalculationEngine.Utility.DataTypeCheck.IsBoolean(LeftOperand.TokenName) == true) && (CalculationEngine.Utility.DataTypeCheck.IsBoolean(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            boolResult = LeftOperand.TokenName_Boolean || RightOperand.TokenName_Boolean;
                            Result = new TokenItem(boolResult.ToString().ToLower(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the OR operator on boolean operands: " + err.Message;
                            return false;
                        }
                    }
                    else
                    {
                        ErrorMsg = "Syntax Error: Expecting boolean operands to OR.";
                        return false;
                    }
#endif
                    break;

                    #endregion

                case "nand":
                    #region NAND
                    {
                        bool bLeft = false;
                        bool bRigth = false;

                        if (Convert.ToDouble(LeftOperand.TokenName) == 1.0)
                            bLeft = true;
                        if (Convert.ToDouble(RightOperand.TokenName) == 1.0)
                            bRigth = true;
                        try
                        {
                            boolResult = !(bLeft && bRigth);
                            double dRet = boolResult ? 1.0 : 0.0;
                            Result = new TokenItem(dRet.ToString().ToLower(),
                                                    CalculationEngine.TokenType.Token_Operand,
                                                    CalculationEngine.TokenDataType.Token_DataType_Double,
                                                    LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the AND operator on boolean operands: " + err.Message;
                            return false;
                        }
                    }
                    break;
                    #endregion

                case "nor":
                    #region NOR
                    {
                        bool bLeft = false;
                        bool bRigth = false;

                        if (Convert.ToDouble(LeftOperand.TokenName) == 1.0)
                            bLeft = true;
                        if (Convert.ToDouble(RightOperand.TokenName) == 1.0)
                            bRigth = true;
                        try
                        {
                            boolResult = !(bLeft || bRigth);
                            double dRet = boolResult ? 1.0 : 0.0;
                            Result = new TokenItem(dRet.ToString().ToLower(),
                                                    CalculationEngine.TokenType.Token_Operand,
                                                    CalculationEngine.TokenDataType.Token_DataType_Double,
                                                    LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the AND operator on boolean operands: " + err.Message;
                            return false;
                        }
                    }
                    break;

                    #endregion

                case "xor":
                #region XOR
                    {
                        bool bLeft = false;
                        bool bRigth = false;

                        if (Convert.ToDouble(LeftOperand.TokenName) == 1.0)
                            bLeft = true;
                        if (Convert.ToDouble(RightOperand.TokenName) == 1.0)
                            bRigth = true;
                        try
                        {
                            boolResult = bLeft ^ bRigth;
                            double dRet = boolResult ? 1.0 : 0.0;
                            Result = new TokenItem(dRet.ToString().ToLower(),
                                                    CalculationEngine.TokenType.Token_Operand,
                                                    CalculationEngine.TokenDataType.Token_DataType_Double,
                                                    LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the AND operator on boolean operands: " + err.Message;
                            return false;
                        }
                    }
                    break;

                #endregion
                default:
                    #region Unknown Operator

                    ErrorMsg = "Failed to evaluate the operator: The operator formula is null.";
                    return false;

                    #endregion
            }

            if (Result == null)
            {
                ErrorMsg = "Syntax Error: Failed to evaluate the expression.";
                return false;
            }
            else
                return true;
        }

        private bool EvaluateOperandFunction(TokenItem OperandFunction, TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // intitialize the outgoing variables
            Result = null;  // assume a failure by setting the result to null
            ErrorMsg = "";

            // local variables
            bool success = true;

            // validate the parameters
            if (OperandFunction == null)
            {
                ErrorMsg = "Failed to evaluate the operand function: The operand function formula is null.";
                return false;
            }

            if (Parameters == null)
            {
                ErrorMsg = "Failed to evaluate the operand function: The parameters collection is null.";
                return false;
            }

            // launch the correct operand function
            switch (OperandFunction.TokenName.Trim().ToLower())
            {

                #region 삼각함수
                case "cos(":
                    try
                    {
                        success = Cos(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "sin(":
                    try
                    {
                        success = Sin(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "tan(":
                    try
                    {
                        success = Tan(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "acos(":
                    try
                    {
                        success = ACos(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "asin(":
                    try
                    {
                        success = ASin(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "atan(":
                    try
                    {
                        success = ATan(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                #endregion

                #region 통계함수

                case "avg(":
                    try
                    {
                        success = Avg(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "sum(":
                    try
                    {
                        success = Sum(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "max(":
                    try
                    {
                        success = Max(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "min(":
                    try
                    {
                        success = Min(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                #endregion

                #region 공학함수

                case "abs(":
                    try
                    {
                        success = Abs(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "round(":
                    try
                    {
                        success = Round(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "ceil(":
                    try
                    {
                        success = Ceil(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "floor(":
                    try
                    {
                        success = Floor(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "sqrt(":
                    try
                    {
                        success = Sqrt(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "pow(":
                    try
                    {
                        success = Pow(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "exp(":
                    try
                    {
                        success = Exp(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "log(":
                    try
                    {
                        success = Log(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "log10(":
                    try
                    {
                        success = Log10(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                #endregion

                #region 논리함수

                case "not(":
                    try
                    {
                        success = Not(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                #endregion
                
                default:
                    ErrorMsg = "Unknown operand function";
                    return false;

            }

            return success;

        }

        #endregion

        #region Operand Functions

        #region 삼각함수

        private bool Cos(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Cos(): Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a Cos of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                // 입력변수는 Degree
                double temp = Parameters[0].TokenName_Double;
                // Radian으로 변환.
                double radian = temp * Math.PI / 180.0;
                double cos_temp = Math.Cos(radian);

                Result = new TokenItem(cos_temp.ToString(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Error in operand function Cos[]: Operand Function can only evaluate parameters that can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool Sin(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Sin[] Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                // Radian으로 변환.
                double radian = temp * Math.PI / 180.0;
                double sin_temp = Math.Sin(radian);

                Result = new TokenItem( sin_temp.ToString(), CalculationEngine.TokenType.Token_Operand, 
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Sin[] can only evaluate parameters that can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool Tan(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Tan() Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                // Radian으로 변환.
                double radian = temp * Math.PI / 180.0;
                double tan_temp = Math.Tan(radian);

                Result = new TokenItem( tan_temp.ToString(), CalculationEngine.TokenType.Token_Operand,
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Tan() can only evaluate parameters that can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool ACos(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "ACos(): Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a Cos of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                if ((-1.0 > temp) || (temp > 1.0))
                {
                    ErrorMsg = "ACos() : parameter must be greater than -1 and less than 1.\nRange( -1.0 <= x <= 1.0 ).";
                    return false;
                }

                double cos_temp = Math.Acos(temp) * ( 180.0 / Math.PI );

                Result = new TokenItem( cos_temp.ToString(), CalculationEngine.TokenType.Token_Operand, 
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "ACos(): Operand Function can only evaluate parameters that can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool ASin(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "ASin() Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                if ((-1.0 > temp) || (temp > 1.0))
                {
                    ErrorMsg = "ASin() : parameter must be greater than -1 and less than 1.\nRange( -1.0 <= x <= 1.0 ).";
                    return false;
                }

                double sin_temp = Math.Asin(temp) * (180.0 / Math.PI);

                Result = new TokenItem(sin_temp.ToString(), CalculationEngine.TokenType.Token_Operand,
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Sin[] can only evaluate parameters that can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool ATan(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "ATan() Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double tan_temp = Math.Atan(temp) * (180.0 / Math.PI); 

                Result = new TokenItem(tan_temp.ToString(), CalculationEngine.TokenType.Token_Operand,
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "ATan() can only evaluate parameters that can be converted to a double.";
                return false;
            }

            return true;
        }

        #endregion

        #region 통계함수

        private bool Avg(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "Avg() : Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the average of items that can be convert to double
            double total = 0;
            try
            {
                foreach (TokenItem tItem in Parameters)
                {
                    if (CalculationEngine.Utility.DataTypeCheck.IsDouble(tItem.TokenName) == true)
                    {
                        total += tItem.TokenName_Double;
                    }
                    else
                    {
                        ErrorMsg = "Error in operand function Avg[]: Operand Function can only calculate the average of parameters that can be converted to double.";
                        return false;
                    }
                }
            }
            catch (Exception err)
            {
                ErrorMsg = "Error in operand function Avg[]: " + err.Message;
                return false;
            }

            double dAvg = 0;
            try
            {
                dAvg = total / Convert.ToDouble(Parameters.Count);
            }
            catch (Exception err)
            {
                ErrorMsg = "Error in operand function Avg[] while calcuating the average: " + err.Message;
                return false;
            }

            Result = new TokenItem( dAvg.ToString(), CalculationEngine.TokenType.Token_Operand, 
                                    CalculationEngine.TokenDataType.Token_DataType_Double, false);

            return true;
        }

        private bool Sum(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "Sum() : Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the average of items that can be convert to double
            double total = 0;
            try
            {
                foreach (TokenItem tItem in Parameters)
                {
                    if (CalculationEngine.Utility.DataTypeCheck.IsDouble(tItem.TokenName) == true)
                    {
                        total += tItem.TokenName_Double;
                    }
                    else
                    {
                        ErrorMsg = "Sum(): Parameters can be converted to double.";
                        return false;
                    }
                }

                Result = new TokenItem( total.ToString(), CalculationEngine.TokenType.Token_Operand,
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);

            }
            catch (Exception err)
            {
                ErrorMsg = "Error in operand function Sum() : " + err.Message;
                return false;
            }

            return true;
        }

        private bool Max(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "Max() : Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the average of items that can be convert to double
            double dMax = 0;
            bool bFirst = true;
            try
            {
                foreach (TokenItem tItem in Parameters)
                {
                    if (CalculationEngine.Utility.DataTypeCheck.IsDouble(tItem.TokenName) == true)
                    {
                        if (bFirst == true)
                        {
                            dMax = tItem.TokenName_Double;
                            bFirst = false;
                        }
                        else
                        {
                            double temp = tItem.TokenName_Double;
                            if (temp > dMax)
                                dMax = temp;
                        }
                    }
                    else
                    {
                        ErrorMsg = "Max(): Parameters can be converted to double.";
                        return false;
                    }
                }

                Result = new TokenItem( dMax.ToString(), CalculationEngine.TokenType.Token_Operand,
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);

            }
            catch (Exception err)
            {
                ErrorMsg = "Error in operand function Max(): " + err.Message;
                return false;
            }

            return true;
        }

        private bool Min(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "Max() : Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the average of items that can be convert to double
            double dMin = 0;
            bool bFirst = true;
            try
            {
                foreach (TokenItem tItem in Parameters)
                {
                    if (CalculationEngine.Utility.DataTypeCheck.IsDouble(tItem.TokenName) == true)
                    {
                        if (bFirst == true)
                        {
                            dMin = tItem.TokenName_Double;
                            bFirst = false;
                        }
                        else
                        {
                            double temp = tItem.TokenName_Double;
                            if (temp < dMin)
                                dMin = temp;
                        }
                    }
                    else
                    {
                        ErrorMsg = "Min(): Parameters can be converted to double.";
                        return false;
                    }
                }

                Result = new TokenItem( dMin.ToString(), CalculationEngine.TokenType.Token_Operand,
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);

            }
            catch (Exception err)
            {
                ErrorMsg = "Error in operand function Min(): " + err.Message;
                return false;
            }

            return true;
        }

        #endregion

        #region 공학함수

        private bool Abs(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Abs(): Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double abs_temp = Math.Abs(temp);

                Result = new TokenItem(abs_temp.ToString(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Abs(): Parameters can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool Round(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 2 parameters
            if (Parameters.Count != 2)
            {
                ErrorMsg = "Round[] Operand Function requires 2 parameter.";
                return false;
            }

            // the first parameters must be a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == false)
            {
                ErrorMsg = "Round[] Operand Function requires the first parameter to be a double.";
                return false;
            }

            // the second parameter must be a integer
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[1].TokenName) == false)
            {
                ErrorMsg = "Round[] Operand Function requires the second parameter to be a integer.";
                return false;
            }

            double roundItem = Parameters[0].TokenName_Double;
            int roundAmt = (int)Parameters[1].TokenName_Double;
            if (roundAmt < 0)
            {
                ErrorMsg = "Round[] Operand Function requires the second parameter to be a positive integer.";
                return false;
            }

            double final = Math.Round(roundItem, roundAmt);

            string format = "#";
            if (roundAmt > 0)
            {
                format += ".";
                for (int i = 0; i < roundAmt; i++) format += "#";
            }

            Result = new TokenItem(final.ToString(format), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Double, false);

            return true;
        }

        private bool Sqrt(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Sqrt[] Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double sqrt_temp = Math.Sqrt(temp);

                Result = new TokenItem(sqrt_temp.ToString(), CalculationEngine.TokenType.Token_Operand, CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Sqrt[] can only evaluate parameters that can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool Ceil(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Ceil(): Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double ceil_temp = Math.Ceiling(temp);

                Result = new TokenItem( ceil_temp.ToString(), CalculationEngine.TokenType.Token_Operand, 
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Ceil(): Parameters can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool Floor(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Floor(): Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double val_temp = Math.Floor(temp);

                Result = new TokenItem( val_temp.ToString(), CalculationEngine.TokenType.Token_Operand,
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Floor(): Parameters can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool Exp(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Exp(): Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double val_temp = Math.Exp(temp);

                Result = new TokenItem(val_temp.ToString(), CalculationEngine.TokenType.Token_Operand,
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Exp(): Parameters can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool Pow(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 2 parameters
            if (Parameters.Count != 2)
            {
                ErrorMsg = "Pow() : Operand Function requires 2 parameter.";
                return false;
            }

            // the first parameters must be a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == false)
            {
                ErrorMsg = "Pow() : The first parameter to be a double.";
                return false;
            }

            // the second parameter must be a integer
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[1].TokenName) == false)
            {
                ErrorMsg = "Pow() : The second parameter to be a double.";
                return false;
            }

            double powItem = Parameters[0].TokenName_Double;
            double powAmt = Parameters[1].TokenName_Double;

            double final = Math.Pow(powItem, powAmt);

            Result = new TokenItem( final.ToString(), CalculationEngine.TokenType.Token_Operand, 
                                    CalculationEngine.TokenDataType.Token_DataType_Double, false);

            return true;
        }

        private bool Log(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Log(): Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double val_temp = Math.Log(temp);

                Result = new TokenItem(val_temp.ToString(), CalculationEngine.TokenType.Token_Operand,
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Log(): Parameters can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool Log10(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Log10(): Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double val_temp = Math.Log10(temp);

                Result = new TokenItem(val_temp.ToString(), CalculationEngine.TokenType.Token_Operand,
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Log10(): Parameters can be converted to a double.";
                return false;
            }

            return true;
        }

        #endregion

        #region 논리함수

        private bool Not(CalculationEngine.TokenItems Parameters, out TokenItem? Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Not(): Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a Not of a boolean formula
            if (CalculationEngine.Utility.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                bool temp = Parameters[0].TokenName_Boolean;
                double dNot = Parameters[0].TokenName_Double;

                if ( dNot == 1)
                    temp = true;
                else if (dNot == 0)
                    temp = false;
                else
                {
                    ErrorMsg = "Not(): The parameter must be 1 or 0.";
                    return false;
                }

                temp = !temp;
                if (temp)
                    dNot = 1.0;
                else
                    dNot = 0.0;

                Result = new TokenItem( dNot.ToString(), CalculationEngine.TokenType.Token_Operand, 
                                        CalculationEngine.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Not(): Operand Function can only evaluate parameters that are boolean.";
                return false;
            }

            return true;
        }

        #endregion             

        #endregion

    }
}
