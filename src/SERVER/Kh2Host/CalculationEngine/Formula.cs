using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CalculationEngine
{
    public enum ParseState
    {
        Parse_State_Operand,            // the parser is looking for an operand
        Parse_State_Operator,           // the parser is looking for an operator
        Parse_State_Quote,              // the parser is looking for a quote
        Parse_State_OperandFunction,    // the parser is in an operand function
        Parse_State_OperandPoint,       // the parser is in realtime point
        Parse_State_Comment             // the parser is in a comment
    };


    public class Formula
    {
        #region 로컬 변수

        private string  ruleSyntax = "";
        private string  lastErrorMessage = "";
        private string  lastEvaluationResult = "";
        private int     charIndex = 0;
        private         TokenItems tokenItems = null;
        private         CalculationEngine.Variables variables = new Variables();
        private         CalculationEngine.Utility.WTSQueue<TokenItem> rpn_queue = null;

        // 계산식에 할당해야하는 변수가 존재하는지 여부를 설정한다.
        private bool    anyAssignments = false;
        // 토큰 분석 시간
        private double tokenParseTime = 0;
        // 마지막 계산 시간
        private double lastEvaluationTime = 0; 

        #endregion

        #region 프로퍼티

        public string RuleSyntax
        {
            get { return ruleSyntax; }
        }

        public string LastErrorMessage
        {
            get { return lastErrorMessage; }
            set { lastErrorMessage = value; }
        }

        public bool AnyErrors
        {
            get { return (String.IsNullOrEmpty(lastErrorMessage) == false); }
        }

        public TokenItems TokenItems
        {
            get { return tokenItems; }
        }

        public Utility.WTSQueue<TokenItem> RPNQueue
        {
            get { return rpn_queue; }
        }

        public Variables Variables
        {
            get { return variables; }
        }

        /// <summary>
        /// that index of the character that is being analyzed.  This also tells us the position of the error.
        /// </summary>
        public int CharIndex
        {
            get { return charIndex; }
        }

        /// <summary>
        /// 최종 계산 결과
        /// </summary>
        public string LastEvaluationResult
        {
            get { return lastEvaluationResult; }
            set { lastEvaluationResult = value; }
        }

        /// <summary>
        /// 계산기에서 최종 계산 시간 설정
        /// </summary>
        public double LastEvaluationTime
        {
            get
            {
                return lastEvaluationTime;
            }
            set
            {
                lastEvaluationTime = value;
            }
        }

        /// <summary>
        /// Indicates if the RuleSyntax has any assignments.
        /// </summary>
        public bool AnyAssignments
        {
            get { return anyAssignments; }
        }

        public double TokenParseTime
        {
            get
            {
                return tokenParseTime;
            }
        }

        #endregion

        #region 생성자
        
        public Formula( string syntax)
        {
            if(string.IsNullOrEmpty(syntax))
            {
                lastErrorMessage = "Formula is Empty";
                return;
            }
            tokenItems          = new TokenItems(this);
            ruleSyntax          = syntax.Trim();
            lastErrorMessage    = string.Empty;


            GetTokens();
        }

        #endregion

        #region 로컬 함수

        private bool CreateTokenItem(string CurrentToken, bool WaitForCreation, bool InOperandFunction, out ParseState NextParseState, out bool IsError, out string ErrorMsg)
        {
            // 출력변수 초기화.
            NextParseState = ParseState.Parse_State_Comment;
            ErrorMsg = string.Empty;
            IsError = false; 

            // 토큰 문자열이 할당되어 있는지 확인한다.
            if (String.IsNullOrEmpty(CurrentToken) == true) return false;
            // 앞뒤 공백을 없앤다.
            CurrentToken = CurrentToken.Trim();
            // 공백을 없앤 토큰 문자열이 할당되어 있는지 확인한다.
            if (String.IsNullOrEmpty(CurrentToken) == true) return false;

            // local variables
            string tempOperand = "";
            string tempOperator = "";

            // check if the formular is an operator
            if ( Utility.DataTypeCheck.IsOperator(CurrentToken) == true )
            {

                tempOperator = CurrentToken;

#if false       // 추후 비교 기능 추가시.
                // 향후 비교 기능  추가 시, 아래의 영역에서 처리
                // >=, <=, <>
                // ..................
                // 끝                            
                switch (CurrentToken)
                {
                    case "<":
                        // see if the next character is = or >
                        try
                        {
                            if (charIndex < ruleSyntax.Length - 1)
                            {
                                char nextChar = ruleSyntax[charIndex]; //charIndex has already been incremented

                                if (nextChar == '=')
                                {
                                    tempOperator += '=';  // adjust the current token
                                    charIndex++; // cause the '=' to be skipped on the next iteration
                                }
                                else if (nextChar == '>')
                                {
                                    tempOperator += '>';  // adjust the current token
                                    charIndex++; // cause the '>' to be skipped on the next iteration
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            IsError = true;
                            ErrorMsg = "Error while determining if the next character is <, location = 1, Error message = " + err.Message;
                            return false;
                        }

                        break;

                    case ">":
                        try
                        {
                            // see if the next character is =
                            if (charIndex < ruleSyntax.Length - 1)
                            {
                                char nextChar = ruleSyntax[charIndex];//charIndex has already been incremented

                                if (nextChar == '=')
                                {
                                    tempOperator += '=';  // adjust the current token
                                    charIndex++; // cause the '=' to be skipped on the next iteration
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            IsError = true;
                            ErrorMsg = "Error while determining if the next character is <, location = 2, Error message = " + err.Message;
                            return false;
                        }

                        break;
                }
#endif
                // 연산자 토큰 생성
                tokenItems.Add( new TokenItem(tempOperator, TokenType.Token_Operator, InOperandFunction));

                // 다음 단계에서는 피연사자를 찾아야 한다.
                NextParseState = ParseState.Parse_State_Operand;

            }
            else if (Utility.DataTypeCheck.ContainsOperator(CurrentToken, out tempOperand, out tempOperator) == true)
            {
                // 피연산자 마지막에 연산자가 붙어 있는 경우.
                // validate and add the new operand
                if (Utility.DataTypeCheck.IsInteger(tempOperand) == true)
                    tokenItems.Add(new TokenItem(tempOperand, TokenType.Token_Operand, TokenDataType.Token_DataType_Int, InOperandFunction));
                else if (Utility.DataTypeCheck.IsDouble(tempOperand) == true)
                    tokenItems.Add(new TokenItem(tempOperand, TokenType.Token_Operand, TokenDataType.Token_DataType_Double, InOperandFunction));
                else if (Utility.DataTypeCheck.IsPointSelection(tempOperand) == true)
                {
                    tokenItems.Add(new TokenItem(tempOperand, TokenType.Token_Operand, TokenDataType.Token_DataType_Variable, InOperandFunction));
                    variables.Add(tempOperand);
                }
                else
                {
                    IsError = true;
                    ErrorMsg = "Invalid token : " + CurrentToken;

                    return false;
                }

                // 마지막에 포함된 연산자를 추가한다.
                tokenItems.Add(new TokenItem(tempOperator, TokenType.Token_Operator, InOperandFunction));

                // 다음 단계에서는 피연사자를 찾아야 한다.
                NextParseState = ParseState.Parse_State_Operand;
            }
            else if (WaitForCreation == false)
            {
                // create a new operand
                if (Utility.DataTypeCheck.IsInteger(CurrentToken) == true)
                    tokenItems.Add(new TokenItem(CurrentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_Int, InOperandFunction));
                else if (Utility.DataTypeCheck.IsDouble(CurrentToken) == true)
                    tokenItems.Add(new TokenItem(CurrentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_Double, InOperandFunction));
                else if (Utility.DataTypeCheck.IsPointSelection(CurrentToken) == true)
                {
                    tokenItems.Add(new TokenItem(CurrentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_Variable, InOperandFunction));
                    variables.Add(CurrentToken);
                }
                else
                {
                    IsError = true;
                    ErrorMsg = "Invalid token : " + CurrentToken;

                    return false;
                }

                // 다음 단계는 연산자를 찾는다.
                NextParseState = ParseState.Parse_State_Operator;
            }
            else
            {
                return false;
            }

            return true;
        }

        private void GetTokens()
        {
            // local variables
            ParseState parseState       = ParseState.Parse_State_Operand;  // start be searching for an operand
            ParseState opFuncParseState = ParseState.Parse_State_Operand; // The parse state within the operand function
            ParseState tempParseState   = ParseState.Parse_State_Operand; // temporary variable to hold a parse state
            string currentToken = "";

            // 함수안에 함수를 포함하는 것을 파악하기 위한것
            // This is the depth of the operand functions
            int operandFunctionDepth = 0;
            // 괄호쌍의 개수
            int parenthesisMatch     = 0;
            // 따움표 구성된 개수
            int quoteMatch           = 0;    // the open and close quotes should match

            bool isError = false;
            bool tokenCreated = false;
            bool[] nc = new bool[256];
            for (int idx = 0; idx < 256; idx++)
                nc[idx] = false;
            // 개산식이 비어있는 경우
            if (String.IsNullOrEmpty(ruleSyntax) == true) 
                return;

            // 파싱하는데 소요되는 시간을 구하기 위한 스톱워치
            System.Diagnostics.Stopwatch parseTime = System.Diagnostics.Stopwatch.StartNew();

            // 파싱하는 문자 위치
            charIndex = 0;

            do
            {
                //  double check the exit condition, just in case
                if ( charIndex >= ruleSyntax.Length ) break;

                // the character to be processed
                char c = ruleSyntax[charIndex];

                // 다음 문자 위치를 설정한다.
                charIndex++;

                #region CR/LF 검사

                // check for new line and tab
                if ((c == '\n') || (c == '\t'))
                {
                    // new lines and tabs are always ignored.
                    c = ' ';  // force the new line or tab to be a space and continue processing.
                }
                #endregion

                // determine out current parse state
                switch (parseState)
                {
                    // 피연산자 분석단계
                    case ParseState.Parse_State_Operand:
                        #region Parse_State_Operand
                        // 실시간 포인트 이름 시작을 의미한다.
                        if (c == '"')
                        {
                            #region DoubleQuotantionMark Handling

                            quoteMatch++;
                            currentToken += c;
                            parseState = ParseState.Parse_State_Quote;

                            #endregion
                        }
                        //
                        else if (c == ' ')
                        {
                            #region Space Handling

                            try
                            {
                                // 공백을 없애지 않고 시작한 경우
                                if (currentToken.Length == 0)
                                    continue;
                                // 만약 현재까지 문자열이 함수이름과 같은 경우에는 
                                // 함수 시작 '('이 있을때까지 보류한다.
                                if (Utility.DataTypeCheck.IsOperandFunction(currentToken))
                                {
                                    continue;
                                }
                                // 피연산자 다음에 공백을 찾았다.
                                // Double Quotantion Mark에 대하여 검사는 하지 않는다.                 
                                // 피연사자가 완전하다는 가정하에 다음에는 연산자를 찾는다.
                                tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                                if (isError)
                                {
                                    return;
                                }

                                if ( tokenCreated == true )
                                {
                                    // set the next parse state
                                    parseState = tempParseState;

                                    // reset the formular
                                    currentToken = "";
                                }
                            }
                            catch (Exception err)
                            {
                                lastErrorMessage = "Error in GetTokens() in Operand Space Handling: " + err.Message;
                                return;
                            }

                            #endregion
                        }
                        // 함수의 시작 또는 묶음 구역 시작
                        else if (c == '(')
                        {
                            #region Open Parenthesis Handling
                            //'(' 와 ')'이 일치되어야하는 개수 증가.
                            parenthesisMatch++;

                            //'(' 앞에 함수가 있으면 함수 토큰.
                            if (Utility.DataTypeCheck.IsOperandFunction(currentToken))
                            {
                                nc[parenthesisMatch - 1] = true;
                                operandFunctionDepth++;

                                currentToken += c;
                                // 함수 시작 토큰을 생성한다.
                                tokenItems.Add( new TokenItem( currentToken, TokenType.Token_Operand_Function_Start, false ));
                                // 구문 분석 단계를 함수분석단계로 변경한다.
                                parseState = ParseState.Parse_State_OperandFunction;
                                // 함수 분석 세부단계는 피연산자 분석
                                opFuncParseState = ParseState.Parse_State_Operand;

                                currentToken = "";
                            }
                            else
                            {
                                // 그렇지 않으면 묶음 구역 시작 토큰.
                                // create the formular
                                tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                                if (isError == true)
                                    return;

                                // add the open parenthesis
                                tokenItems.Add(new TokenItem("(", TokenType.Token_Open_Parenthesis, false));

                                // clear the current formular
                                currentToken = "";

                                // after a parenthesis we need an operand
                                parseState = ParseState.Parse_State_Operand;
                            }
                            #endregion
                        }
                        else if (c == ')')
                        {
                            #region Close Parenthesis Handling
                            //'(' 와 ')'이 일치되어야하는 개수 감소.
                            parenthesisMatch--;

                            // 토큰 아이템을 생성하여 목록에 추가한다.
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) 
                                return;
                            // add the close parenthesis
                            tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, false));

                            // clear the current formular
                            currentToken = "";

                            // after a parenthesis we need an operator
                            parseState = ParseState.Parse_State_Operator;

                            #endregion
                        }
                        else if (c == ',')
                        {
                            #region Command Handling
                            // 피연산자 분석단계에서 콤마(,)가 나올수 없다.
                            lastErrorMessage = "Error in Rule Syntax: Found a , (comma) while looking for an operand.";
                            return;
                            #endregion
                        }
                        else if (c == '-')
                        {
                            #region Negative Operands
                            if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                            {
                                currentToken += c;
                            }
                            else
                            {
                                // 피연산자 토큰을 생성하고 연산자(-) 토큰이 생성된다.
                                // Utility.DataTypeCheck.ContainsOperator에서 처리됨
                                currentToken += c;
                                tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                                if (isError == true) return;
                                if (tokenCreated == true)
                                {
                                    // the new tokens were created

                                    // reset the current formular
                                    currentToken = "";

                                    // set the next parse state
                                    parseState = tempParseState;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Other Handling
                            // try and create the new formular
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true)
                            {
                                lastErrorMessage = "Invalid token : " + currentToken;
                                return;
                            }

                            if (tokenCreated == true)
                            {
                                // the new tokens were created
                                // reset the current formular
                                currentToken = "";

                                // set the next parse state
                                parseState = tempParseState;
                            }
                            #endregion
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_OperandFunction:
                        #region Parse_State_OperandFunction

                        switch (opFuncParseState)
                        {
                            case ParseState.Parse_State_Operand:
                                #region Parse_State_OperandFunction, Parse_State_Operand
                                if (c == '"')
                                {
                                    #region Quote Handling
                                    // we have a quote in the operand function...This is probably the start of a string operand
                                    quoteMatch++;
                                    currentToken += c;
                                    opFuncParseState = ParseState.Parse_State_Quote;
                                    #endregion
                                }
                                else if (c == ' ')
                                {
                                    #region Space Handling
                                    if (string.IsNullOrEmpty(currentToken))
                                        continue;
                                    
                                    // 만약 현재까지 문자열이 함수이름과 같은 경우에는 
                                    // 함수 시작 '('이 있을때까지 보류한다.
                                    if (Utility.DataTypeCheck.IsOperandFunction(currentToken))
                                    {
                                        continue;
                                    }

                                    // 피연산자 다음에 공백을 찾았다.
                                    // Double Quotantion Mark에 대하여 검사는 하지 않는다.                 
                                    // 피연사자가 완전하다는 가정하에 다음에는 연산자를 찾는다.
                                    tokenCreated = CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (tokenCreated == true)
                                    {
                                        // set the next parse state
                                        opFuncParseState = tempParseState;

                                        // reset the formular
                                        currentToken = "";
                                    }
                                    #endregion
                                }
                                else if (c == '(')
                                {
                                    #region Parenthesis Handling
                                    // increment the parenthesis count
                                    parenthesisMatch++;

                                    //'(' 앞에 함수가 있으면 함수 토큰.
                                    if (Utility.DataTypeCheck.IsOperandFunction(currentToken))
                                    {
                                        nc[parenthesisMatch - 1] = true;
                                        operandFunctionDepth++;

                                        currentToken += c;
                                        // 함수 시작 토큰을 생성한다.
                                        tokenItems.Add(new TokenItem(currentToken, TokenType.Token_Operand_Function_Start, true));
                                        // 구문 분석 단계를 함수분석단계로 변경한다.
                                        parseState = ParseState.Parse_State_OperandFunction;
                                        // 함수 분석 세부단계는 피연산자 분석
                                        opFuncParseState = ParseState.Parse_State_Operand;

                                        currentToken = "";
                                    }
                                    else
                                    {

                                        // create the formular
                                        CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                        if (isError == true) return;

                                        // add the open parenthesis
                                        tokenItems.Add(new TokenItem("(", TokenType.Token_Open_Parenthesis, true));

                                        // clear the current formular
                                        currentToken = "";

                                        // after a parenthesis we need an operand
                                        opFuncParseState = ParseState.Parse_State_Operand;
                                    }
                                    #endregion
                                }
                                else if (c == ')')
                                {
                                    #region Parenthesis Handling
                                    // decrement the parenthesis count
                                    parenthesisMatch--;

                                    // create the formular
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    if ( nc[parenthesisMatch] == true )
                                    {
                                        operandFunctionDepth--;
                                        tokenItems.Add(new TokenItem(")", TokenType.Token_Operand_Function_Stop, true));

                                        if (operandFunctionDepth <= 0)
                                        {
                                            operandFunctionDepth = 0;
                                            parseState = ParseState.Parse_State_Operator;
                                        }

                                        nc[parenthesisMatch] = false;
                                    }
                                    else
                                    {
                                        // add the close parenthesis
                                        tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, true));
                                    }
                                    // clear the current formular
                                    currentToken = "";

                                    // after a parenthesis we need an operator
                                    opFuncParseState = ParseState.Parse_State_Operator;

                                    #endregion
                                }
                                else if (c == ',')
                                {
                                    #region Command Handling
                                    // create a toke
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the delimiter formular
                                    tokenItems.Add(new TokenItem(",", TokenType.Token_Operand_Function_Delimiter, true));

                                    // reset the formular
                                    currentToken = "";

                                    opFuncParseState = ParseState.Parse_State_Operand;
                                    #endregion
                                }
                                else if (c == '-')
                                {
                                    #region Negative operands
                                    if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                                    {
                                        // we found a negative sign
                                        currentToken += c;
                                    }
                                    else
                                    {
                                        // try and create the new formular
                                        currentToken += c;
                                        tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                        if (isError == true) return;
                                        if (tokenCreated == true)
                                        {
                                            // the new tokens were created

                                            // reset the current formular
                                            currentToken = "";

                                            // set the next parse state
                                            opFuncParseState = tempParseState;
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Other Handling
                                    // try and create the new formular
                                    currentToken += c;

                                    tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    if (tokenCreated == true)
                                    {
                                        // the new tokens were created

                                        // reset the current formular
                                        currentToken = "";

                                        // set the next parse state
                                        opFuncParseState = tempParseState;

                                    }
                                    #endregion
                                }

                                #endregion
                                break;

                            case ParseState.Parse_State_Operator:
                                #region Parse_State_OperandFunction, Parse_State_Operator

                                if (c == '"')
                                {
                                    #region Quote Handling
                                    // we found a quote while looking for an operator...this is a problem
                                    lastErrorMessage = "Error in Rule Syntax: Found a double quote (\") while looking for an operator.";
                                    return;
                                    #endregion
                                }
                                else if (c == ' ')
                                {
                                    #region Space Handling
                                    // we found a space while looking for an operator...maybe it's just white space
                                    if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                                    {
                                        // yep, it's just white space; it can be ignored.
                                        currentToken = "";
                                    }
                                    else
                                    {
                                        // we found a space while looking for an operator....this is a problem because
                                        // no operators allow for spaces.
                                        lastErrorMessage = "Error with expression syntax: Found a space while looking for an operator";
                                        return;
                                    }
                                    #endregion
                                }
                                else if (c == '(')
                                {
                                    #region Parenthesis Handling
                                    // we found an opern parenthesis while searching for an operator....that's a problem
                                    lastErrorMessage = "Error in rule syntax: Found an open parenthesis while searching for an operator";
                                    return;
                                    #endregion
                                }
                                else if (c == ')')
                                {
                                    #region Parenthesis Handling
                                    // add the closed parenthesis and keep looking for the operator
                                    parenthesisMatch--;

                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    if (nc[parenthesisMatch] == true)
                                    {
                                        operandFunctionDepth--;
                                        tokenItems.Add(new TokenItem(")", TokenType.Token_Operand_Function_Stop, true));

                                        if (operandFunctionDepth <= 0)
                                        {
                                            operandFunctionDepth = 0;
                                            parseState = ParseState.Parse_State_Operator;
                                        }

                                        currentToken = "";
                                        nc[parenthesisMatch] = false;
                                    }
                                    else
                                    {
                                        tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, true));
                                    }
                                    #endregion
                                }
                                else if (c == ',')
                                {
                                    #region Command Handling
                                    // we found a comma while searching for an operator....that's alright in an operand function
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the comma
                                    tokenItems.Add(new TokenItem(",", TokenType.Token_Operand_Function_Delimiter, true));

                                    // reset the formular
                                    currentToken = "";

                                    // set the parse state
                                    opFuncParseState = ParseState.Parse_State_Operand;
                                    #endregion
                                }
                                else
                                {
                                    #region Other Handling
                                    // try and create the new formular
                                    currentToken += c;
                                    tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;
                                    if (tokenCreated == true)
                                    {
                                        // the new tokens were created

                                        // reset the current formular
                                        currentToken = "";

                                        // set the next parse state
                                        opFuncParseState = ParseState.Parse_State_Operand;

                                    }
                                    #endregion
                                }

                                #endregion
                                break;

                            case ParseState.Parse_State_Quote:
                                #region Parse_State_OperandFunction, Parse_State_Quote
                                // 포인트 토큰끝
                                if (c == '"')
                                {
                                    #region Quote Handling

                                    quoteMatch--;

                                    // we found the end of the qoute
                                    currentToken += c;
                                    tokenCreated = CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) 
                                        return;
                                    if (tokenCreated == true)
                                    {
                                        // set the next parse state
                                        opFuncParseState = tempParseState;
                                        // clear the current formular
                                        currentToken = "";
                                    }

                                    #endregion
                                }
                                else
                                {
                                    #region Other Handling
                                    currentToken += c;
                                    #endregion
                                }

                                #endregion
                                break;
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_Operator:
                        #region Parse_State_Operator
                        if (c == '"')
                        {
                            #region Quote Handling
                            // we found a quote while looking for an operator...this is a problem
                            lastErrorMessage =  "Error in Rule Syntax: Found a double quote (\") " +
                                                "while looking for an operator.";
                            return;
                            #endregion
                        }
                        else if (c == ' ')
                        {
                            #region Space Handling
                            // we found a space while looking for an operator...maybe it's just white space
                            if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                            {
                                // yep, it's just white space; it can be ignored.
                                currentToken = "";
                            }
                            else
                            {
                                // we found a space while looking for an operator....
                                // this is a problem because no operators allow for spaces.
                                lastErrorMessage =  "Error with expression syntax: Found a space " +
                                                    "while looking for an operator";
                                return;
                            }
                            #endregion
                        }
                        else if (c == '(')
                        {
                            #region Parenthesis Handling
                            // we found an opern parenthesis while searching for an operator....that's a problem
                            lastErrorMessage =  "Error in rule syntax: Found an open parenthesis " + 
                                                "while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ')')
                        {
                            #region Parenthesis Handling
                            parenthesisMatch--;

                            if (nc[parenthesisMatch] == true)
                            {
                                tokenItems.Add(new TokenItem(")", TokenType.Token_Operand_Function_Stop, false));
                            }
                            else
                            {
                                // add the closed parenthesis and keep looking for the operator
                                tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, false));
                            }
                            #endregion
                        }
                        else if (c == ',')
                        {
                            #region Comma Handling
                            // we found a comma while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found a comma while searching for an operator";
                            return;
                            #endregion
                        }
                        else
                        {
                            #region Other Handling
                            // try and create the new formular
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;
                            if (tokenCreated == true)
                            {
                                // the new tokens were created

                                // reset the current formular
                                currentToken = "";

                                // set the next parse state
                                parseState = tempParseState;

                            }
                            #endregion
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_Quote:
                        #region Parse_State_Quote
                        // 포인트 토큰의 끝
                        if (c == '"')
                        {
                            #region Quote Handling
                            // we found the end of the qoute
                            quoteMatch--;
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;
                            if (tokenCreated == true)
                            {
                                // set the next parse state
                                parseState = tempParseState;

                                // clear the current formular
                                currentToken = "";
                            }
                            #endregion
                        }
                        else
                        {
                            currentToken += c;
                        }

                        #endregion
                        break;
                }


            } while (charIndex < ruleSyntax.Length);


            #region Final Token Handling

            // see if we have a current formular that needs to be processed.
            currentToken = currentToken.Trim();

            if (String.IsNullOrEmpty(currentToken) == false)
            {
                // we have a formular that needs to be processed
                if (currentToken == "(")
                    parenthesisMatch++;
                else if (currentToken == ")")
                {
                    parenthesisMatch--;

                    if (nc[parenthesisMatch] == true)
                    {
                        operandFunctionDepth--;
                        nc[parenthesisMatch] = false;
                    }
                }
                else if (currentToken == "\"")
                    quoteMatch--;

                switch (parseState)
                {
                    case ParseState.Parse_State_Operand:
                        CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                        if (isError == true)
                        {
                            lastErrorMessage = "Invalid token : " + currentToken;
                            return;
                        }
                        break;

                    case ParseState.Parse_State_Operator:
                        // there should never be an operator at the end of the rule syntax
                        lastErrorMessage = "Error in Rule Syntax: A rule cannot end with an operator.";
                        break;

                    case ParseState.Parse_State_Quote:
                        // we are looking for a closing quote
                        if (currentToken != "\"")
                        {
                            lastErrorMessage = "Error in RuleSyntax: Double quote mismatch.";
                            return;
                        }
                        else
                        {
                            // add the formular as an operand
                            tokenItems.Add(new TokenItem(currentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_String, false));
                        }
                        break;

                    case ParseState.Parse_State_OperandFunction:
                        break;

                }
            }

            #endregion

            #region RuleSyntax Validation Checks

            // 괄호쌍이 맞지 않은 경우.
            if (parenthesisMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is a parenthesis mismatch.";
                return;
            }

            // 함수의 괄호쌍이 맞지 않는 경우.
            if (operandFunctionDepth > 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is an operand function mismatch error...Operand function depth is not zero.";
                return;
            }

            // 쌍따움표 쌍이 맞지않는 경우.
            if (quoteMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is a quote mismatch.";
                return;
            }

            // 구문 오류로  중간에 끝난경우. 
            if (charIndex < ruleSyntax.Length)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem parsing the rule...some of the tokens were not found.";
                return;
            }

            #endregion

            #region Make the RPN Queue

            // create the RPN Stack
            if (this.AnyErrors == false) 
                MakeRPNQueue();

            // check that we have tokens in out RPN Queue
            if (rpn_queue == null)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem creating the RPN queue.";
                return;
            }

            if (rpn_queue.Count == 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem creating the RPN queue.";
                return;
            }

            #endregion

            // stop the timer
            parseTime.Stop();
            tokenParseTime = parseTime.Elapsed.TotalMilliseconds;

        }

        private void MakeRPNQueue()
        {

            /*
            While there are tokens to be read: 
                Read a formular. 
                If the formular is a number, then add it to the output queue. 
                If the formular is a function formular, then push it onto the stack. 
                If the formular is a function argument separator (e.g., a comma): 
                    Until the topmost element of the stack is a left parenthesis, pop the element onto the output queue. If no left parentheses are encountered, either the separator was misplaced or parentheses were mismatched. 
                If the formular is an operator, o1, then: 
                    while there is an operator, o2, at the top of the stack, and either 
                        o1 is associative or left-associative and its precedence is less than (lower precedence) or equal to that of o2, or 
                        o1 is right-associative and its precedence is less than (lower precedence) that of o2,

                        pop o2 off the stack, onto the output queue; 
                    push o1 onto the operator stack. 
                If the formular is a left parenthesis, then push it onto the stack. 
                If the formular is a right parenthesis: 
                    Until the formular at the top of the stack is a left parenthesis, pop operators off the stack onto the output queue. 
                    Pop the left parenthesis from the stack, but not onto the output queue. 
                    If the formular at the top of the stack is a function formular, pop it and onto the output queue. 
                    If the stack runs out without finding a left parenthesis, then there are mismatched parentheses. 
                When there are no more tokens to read: 
                    While there are still operator tokens in the stack: 
                        If the operator formular on the top of the stack is a parenthesis, then there are mismatched parenthesis. 
                        Pop the operator onto the output queue. 
            Exit. 
            */


            // set the last error message
            lastErrorMessage = "";

            // make sure we have parsed tokens
            if (this.tokenItems.Count == 0)
            {
                lastErrorMessage = "No tokens to add to the RPN stack";
                return;
            }

            // create the output queue...This is the final queue that is exposed through the public property
            rpn_queue = new Utility.WTSQueue<TokenItem>(tokenItems.Count);

            // create the operator stack
            Utility.WTSStack<TokenItem> operators = new Utility.WTSStack<TokenItem>();


            // create the operator stack for operators
            Utility.WTSStack<TokenItem> param_operators = new Utility.WTSStack<TokenItem>();

            // create a temporary queue for expressions that are in operand functions
            Utility.WTSQueue<TokenItem> param_queue = new Utility.WTSQueue<TokenItem>();

            int matchingFunction = 0;

            // create a "fake" delimiter formular item
            TokenItem delimiter = new TokenItem(",", TokenType.Token_Operand_Function_Delimiter, false);

            // loop through all the formular items
            foreach ( TokenItem item in tokenItems )
            {
                switch ( item.TokenType )
                {
                    // 괄호의 끝
                    case TokenType.Token_Close_Parenthesis:
                        #region Token_Close_Parenthesis
                        // 함수 내부에 있는 경우.
                        if ( item.InOperandFunction == true )
                        {
                            #region Parameter Operator Stack
                            if (param_operators.Count > 0)
                            {
                                // start peeking at the top operator
                                do
                                {
                                    // 괄호의 시작 '('
                                    if (param_operators.Peek().TokenType == TokenType.Token_Open_Parenthesis)
                                    {
                                        // 파라미터 연산자에서 제거
                                        param_operators.Pop();
                                        break;
                                    }
                                    else
                                    {
                                        // 파마리터 큐에 삽입
                                        param_queue.Add(param_operators.Pop());
                                    }

                                    if (param_operators.Count == 0) 
                                        break;
                                }
                                while (true);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            // 연산자 큐에 연산자가 존재하는 경우,
                            if (operators.Count > 0)
                            {
                                // start peeking at the top operator
                                do
                                {
                                    // 괄호의 시작 '('
                                    if (operators.Peek().TokenType == TokenType.Token_Open_Parenthesis)
                                    {
                                        // 연산자 스택에서 제거한다.
                                        operators.Pop();
                                        break;
                                    }
                                    else
                                    {
                                        // RPN 큐에 삽입한다.
                                        rpn_queue.Add(operators.Pop());
                                    }

                                    if (operators.Count == 0) break;
                                }
                                while (true);
                            }
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Open_Parenthesis:
                        #region Token_Open_Parenthesis

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operator Stack
                            // 파라미터 연산자 스택에 삽입한다.
                            param_operators.Push(item);
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            // 연산자 스택에 삽입한다.
                            operators.Push(item);
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Operand:
                        #region Token_Operand
                        // 함수 안에 있는 피연산자인 경우 
                        if (item.InOperandFunction == true)
                        {
                            #region Operand Queue
                            // 파라미터 큐에 삽입한다.
                            param_queue.Add(item);  
                            #endregion
                        }
                        else
                        {
                            #region Operand Queue
                            // RPN 큐에 삽입한다.
                            rpn_queue.Add(item);
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Operand_Function_Delimiter:
                        #region Token_Operand_Function_Delimiter

                        // 파라미터 스택이 비었거나 파라미터 구분자를 만날때까지
                        // 파라미터 스택에서 연산자를 꺼낸다.
                        do
                        {
                            // 파라미터 연산자 스택이 비어있는 경우 중지
                            if (param_operators.Count == 0) 
                                break;

                            // 파라미터 연산자 스택에서 
                            TokenItem opItem = param_operators.Pop();

                            // 함수 파라미터 구분자인 경우에는 중지한다.
                            if ( opItem.TokenType == TokenType.Token_Operand_Function_Delimiter) 
                                break;

                            // 파라미터 큐에 넣는다.
                            param_queue.Add(opItem);
                        }
                        while (true);

                        // start removing items from the paraemters queue and add them to the RPN queue
                        // unill it's empty or we access an operand function start
                        do
                        {
                            // 파라마터 큐가 비어있는 경우 중지
                            if (param_queue.Count == 0) 
                                break;

                            // 파라미터 큐에서 토큰을 얻는다.
                            TokenItem qItem = param_queue.Dequeue();
                            // RPN 큐에 넣는다.
                            rpn_queue.Add(qItem);

                            // 함수 시작인 경우에는 중지한다.
                            if (qItem.TokenType == TokenType.Token_Operand_Function_Start) 
                                break;
                        }
                        while (true);

                        // 파라미터 스택에 구분자(,)를 삽입한다.
                        param_operators.Push(item);

                        // RPN 큐에 구분자(,)를 삽입한다.
                        rpn_queue.Add(item);

                        #endregion
                        break;

                    case TokenType.Token_Operand_Function_Start:
                        #region Token_Operand_Function_Start
                        // 함수 내부에서 함수 시작인 경우.
                        if (item.InOperandFunction == true)
                        {

                            #region Parameter Operand Queue
                            param_queue.Add(item);

                            // whenever we add a new operand function to the parameter queue,
                            // add a "fake" delimiter to the parameter operator queue
                            param_operators.Push(delimiter);

                            #endregion
                        }
                        else
                        {
                            #region Operand Queue
                            rpn_queue.Add(item); // if formular is an operand function, then write it to output. 

                            #endregion
                        }

                        matchingFunction++;

                        #endregion
                        break;

                    case TokenType.Token_Operand_Function_Stop:
                        #region Token_Operand_Function_Stop
                        // 함수 내부에 있는 함수의 마지막 
                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Queue

                            // 파라미터 스택이 비워졌거나 파라미터 구분자를 만날때까지
                            // 파라미터 스택에서 연산자를 꺼낸다.
                            do
                            {
                                // 파라미터의 연사자가 비워진경우 종료
                                if (param_operators.Count == 0) 
                                    break;

                                // 파라미터 연산자 스택에서 토큰을 얻는다.
                                TokenItem opItem = param_operators.Pop();

                                // 파라미터 구분자인 경우 종료
                                if (opItem.TokenType == TokenType.Token_Operand_Function_Delimiter) 
                                    break;

                                // 파라미터 큐에 넣는다.
                                param_queue.Add(opItem);
                            }
                            while (true);

                            // 파라미터 큐에 괄호 끝')'을 넣는다.
                            param_queue.Add(item);

                            // 파라미터 큐가 비워졌거난 함수의 끝 만날때 까지
                            // 파라미터 큐에 있는 토큰을 RPN 큐에 넣는다.
                            do
                            {
                                // 파라미터 큐가 비워진 경우 종료
                                if (param_queue.Count == 0) 
                                    break;

                                // 파라미터 큐에서 토큰을 얻는다.
                                TokenItem qItem = param_queue.Dequeue();

                                rpn_queue.Add(qItem);

                                // 함수의 끝을 만나면 종료
                                if (qItem.TokenType == TokenType.Token_Operand_Function_Stop) 
                                    break;
                            }
                            while (true);

                            matchingFunction--;

                            #endregion
                        }
                        else
                        {
                            #region Operand Queue
                            rpn_queue.Add(item); // if formular is an operand function, then write it to output. 
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Operator:
                        #region Token_Operator

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operator Stack
                            // 파라미터의 연산자가 있는 경우.
                            if (param_operators.Count > 0)
                            {
                                // peek at the top item of the operator stack
                                do
                                {
                                    // 현재 연산자 우선순위가 파라미터 연산자보다 낮거나 같은 경우
                                    // 파라미터 연산자에 있는 토큰을 파라미터 큐에 넣는다.
                                    if (item.OrderOfOperationPrecedence >= param_operators.Peek().OrderOfOperationPrecedence)
                                    {
                                        param_queue.Add(param_operators.Pop());
                                    }
                                    else
                                        break;

                                    // 파라미터 연산자가 비워진 경우 종료
                                    if (param_operators.Count == 0) 
                                        break;

                                } while (true);

                            }

                            // 파라미터 연사자 스택에 연사자를 넣는다.
                            param_operators.Push(item);
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            // 연산자 스택에 연산자가 존재하는 경우.
                            if (operators.Count > 0)
                            {
                                // 연산자 스택에 있는 연산자 토큰을 얻어
                                do
                                {
                                    // 현재 연산자 우선순위가 연산자 스택에 있는 연산자보다 낮거나 같은 경우
                                    // 연산자 스택에서 꺼내어 RPN 스택에 넣는다.
                                    if (item.OrderOfOperationPrecedence >= operators.Peek().OrderOfOperationPrecedence)
                                    {
                                        rpn_queue.Add(operators.Pop());
                                    }
                                    else
                                        break;

                                    // 연산자 스택이 비워진 경우 종료.
                                    if (operators.Count == 0) 
                                        break;

                                } while (true);
                            }

                            // 연산자 스택에 넣는다.
                            operators.Push(item);
                            #endregion
                        }

                        #endregion
                        break;
                }
            }

            // 연산자 스택에 남아있는 연산자를 RPN 스택에 넣는다.
            int opCount = operators.Count;
            for (int i = 0; i < opCount; i++) 
                rpn_queue.Add(operators.Pop());

            #endregion

        }
    }
}
