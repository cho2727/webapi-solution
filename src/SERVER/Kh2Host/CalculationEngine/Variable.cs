using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationEngine
{
    public class Variable
    {
        #region 로컬 변수

        // the name of the variable
        private string variableName;
        private string variableValue;

        // the formular items that correspond with this variable
        private System.Collections.Generic.List<TokenItem> tokenItems;

        #endregion

        #region 생성자

        public Variable(string VarName)
        {
            tokenItems = new List<TokenItem>();
            variableName = VarName;
            variableValue = "";

        }

        public Variable(string VarName, string VarValue)
        {
            tokenItems = new List<TokenItem>();
            variableName = VarName;
            variableValue = VarValue;
        }

        #endregion

        #region 프로퍼티

        public string VariableName
        {
            get
            {
                return variableName;
            }
        }

        public string VariableValue
        {
            get
            {

                return variableValue;
            }
            set
            {
                variableValue = value;
            }
        }


        /// <summary>
        /// This variables key in the collection
        /// </summary>
        public string CollectionKey
        {
            get
            {
                return variableName.Trim().ToLower();
            }
        }

        public System.Collections.Generic.List<TokenItem> TokenItems
        {
            get
            {
                return tokenItems;
            }
        }


        #endregion

        #region 함수

        public Variable Clone()
        {
            // create a new variable using the current name and value
            return new Variable(this.variableName, this.variableValue);
        }

        #endregion
    }
}
