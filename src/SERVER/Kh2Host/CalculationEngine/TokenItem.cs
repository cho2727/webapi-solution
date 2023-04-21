using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationEngine
{
    public enum TokenType
    {
        Token_Operand,                      // indicates the formular is an operand 
        Token_Operand_Function_Start,       // indicates the formular is the start of an operand function
        Token_Operand_Point_Start,          // indicates the formular is the start of realtime point
        Token_Open_Parenthesis,             // indicates the formular is an open parenthesis
        Token_Close_Parenthesis,            // indicates the formular is a closed parenthesis
        Token_Operand_Function_Stop,        // indicates the formular is the end of an operand function
        Token_Operand_Function_Delimiter,   // indicates the formular is an operand function delimiter
        Token_Operator,                     // indicates the formular is an operator
        Token_Assignemt_Start,              // indicates the formular is the start of an assignment 
        Token_Assignment_Stop               // indicates the formular is the stop of an assignment.
    };

    public enum TokenDataType
    {
        Token_DataType_None,            // the formular does not have a data type (for example, an operator)
        Token_DataType_Variable,         // indicates that the formular is a variable - unknown data type
        Token_DataType_Int,              // indicates that the formular is an integer
        Token_DataType_Date,             // indicates that the formular is date
        Token_DataType_Double,           // indicates the formular is a double
        Token_DataType_String,           // indicates the formular is a string
        Token_DataType_Boolean,          // indicates the formular is a boolean
        Token_DataType_NULL              // indicates the formular is a null value
    };

    public class TokenItem
    {
        #region 로컬변수

        private string tokenName;
        private string tokenNameBase;
        private TokenType       tokenType;
        private TokenDataType   tokenDataType;

        // 함수안에 있는 Token인지 여부를 설정한다.
        private bool            inOperandFunction = false;
        // 토큰의 데이터 타입이 Token_DataType_Variable 인 경우 
        // 값을 지정되었는지 여부를 설정한다.
        private bool willBeAssigned = false;

        // This formular item will reference the TokenItems collectopn
        internal TokenItems parent = null;
        
        #endregion

        #region 생성자

        public TokenItem(string TokenName, TokenType TokenType, bool InOperandFunction, string TokenNameBase = "")
        {
            tokenName           = TokenName;
            tokenType           = TokenType;
            tokenDataType       = TokenDataType.Token_DataType_None;
            inOperandFunction   = InOperandFunction;
            this.tokenNameBase = TokenNameBase;
        }

        public TokenItem(string TokenName, TokenType TokenType, TokenDataType TokenDataType, bool InOperandFunction, string TokenNameBase = "")
        {
            tokenName           = TokenName;
            tokenType           = TokenType;
            tokenDataType       = TokenDataType;
            inOperandFunction   = InOperandFunction;
            this.tokenNameBase = TokenNameBase;
        }

        #endregion

        #region 프로퍼티

        public string TokenName
        {
            get
            {
                return tokenName;
            }
        }

        public string TokenNameBase
        {
            get
            {
                return tokenNameBase;
            }
        }

        public TokenType TokenType
        {
            get
            {
                return tokenType;
            }
        }

        public TokenDataType TokenDataType
        {
            get
            {
                return tokenDataType;
            }
        }

        public bool InOperandFunction
        {
            get
            {
                return inOperandFunction;
            }
            set
            {
                inOperandFunction = value;
            }
        }

        /// <summary>
        /// Indicates if the variable will be assigned to in the rule
        /// </summary>
        public bool WillBeAssigned
        {
            get
            {
                return willBeAssigned;
            }
            set
            {
                willBeAssigned = value;
            }
        }

        /// <summary>
        /// A tokenitem object parent is a tokenitems collection
        /// </summary>
        public TokenItems Parent
        {
            get
            {
                return parent;
            }
        }
        
        #endregion

        #region 데이터타입 프로퍼티

        public int TokenName_Int
        {
            get
            {
                int result = 0;

                if (Int32.TryParse(tokenName, out result) == true)
                    return result;
                else
                    return 0;

            }
        }

        public bool TokenName_Boolean
        {
            get
            {
                if (String.IsNullOrEmpty(tokenName) == true) return false;
                return (tokenName.Trim().ToLower() == "true");
            }
        }

        public double TokenName_Double
        {
            get
            {
                double result = 0;

                if (Double.TryParse(tokenName, out result) == true)
                    return result;
                else
                    return 0;

            }
        }

        public DateTime TokenName_DateTime
        {
            get
            {
                DateTime result = DateTime.MinValue;

                if (DateTime.TryParse(tokenName, out result) == true)
                    return result;
                else
                    return DateTime.MinValue;

            }
        }


        public int OrderOfOperationPrecedence
        {
            get
            {
                int _order = 1000;

                switch (this.tokenName.Trim().ToLower())
                {

                    case "^":
                        _order = 1;
                        break;

                    case "*":
                        _order = 2;
                        break;

                    case "/":
                        _order = 2;
                        break;

                    case "%":
                        _order = 2;
                        break;

                    case "+":
                        _order = 3;
                        break;

                    case "-":
                        _order = 3;
                        break;

                    case "and":
                        _order = 4;
                        break;

                    case "nand":
                        _order = 4;
                        break;

                    case "or":
                        _order = 5;
                        break;

                    case "nor":
                        _order = 5;
                        break;

                    case "xor":
                        _order = 6;
                        break;
                }

                return _order;

            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return tokenName;
        }

        #endregion
    }
}
