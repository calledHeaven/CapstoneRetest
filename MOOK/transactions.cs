using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOOK
{
    public class transactions
    {
        
        private char _Transaction_Type;
        private float _Transaction_Value;
        public string transaction { get { return Transaction_Type.ToString() + "     " + Transaction_Value.ToString(); } } //tostring method in order to assist in outputing when called in the menu
    
        

        public Char Transaction_Type
        {
            get
            {
                return _Transaction_Type;
            }
            set
            {
              
                if (value != null && value != 'L' || value != 'S' || value != 'P' || value != 'R')
                { _Transaction_Type = value; }
                else { throw new Exception("Invalid Transaction type"); }
                
            }
        }

        public float Transaction_Value
        {
            get
            {
                return _Transaction_Value;
            }
            set
            {
                if (value != null && value >= 0)
                { _Transaction_Value = value; }
                else { throw new Exception("Invalid Transaction type"); }
            }
        }

        public transactions(char transaction_Type, float transaction_Value)  // creation of a transaction, not requiring projectID, as transactions are stored in lists in projects, that already store Project ids, reducing dependancies and repetition
        {
            Transaction_Type = transaction_Type;
            Transaction_Value = transaction_Value;
        }
    }

}
