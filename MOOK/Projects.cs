using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOOK // projects is a middle class in transactions, projects, buissness. the busisness will contain a list of transactions and projects, as projects need identifiers, so must have multiple that ust be sorted through
    // the buisness calss must be very similar to what has been done here for projects, but when a transaction is processed, the file reader must check if there are any projects that have the same id as the transaction being processed, if not it must create one
{
    public class Projects
    {
        
        public List<transactions> _transactions;
        private int _Project_ID;
        public bool VAT_available;

        public int Project_ID
        {
            get
            {
                return _Project_ID;
            }
            set
            {
                if (value != null)
                { _Project_ID = value; }
                else { throw new Exception("Invalid Project ID"); }
            }
        }

        public Projects(int project_ID, bool VAT_Available) //  project constructor stores ID to reduce repetition and weather the VAT refund is available for project when created to remove an unessisary check laer in the program
        {
            Project_ID = project_ID;
            VAT_available = VAT_Available;
            _transactions = new List<transactions>();
        }

        public void addTransaction(transactions transactions)
        { 
            _transactions.Add(transactions);

        }
            
        public double Get_Total_Sales()
        {
            double total = 0;
            foreach (transactions trans in _transactions) 
            {
                if (trans.Transaction_Type == 'S')
                {
                    total += trans.Transaction_Value;
                }
            }
            
            return Math.Round(total, 2);
        }
        public double Get_Total_Purchases()
        {
            double total = 0;
            foreach (transactions trans in _transactions)
            {
                if (trans.Transaction_Type == 'P' || trans.Transaction_Type == 'R' || trans.Transaction_Type == 'L')
                {
                    total += trans.Transaction_Value;
                }
            }

            return Math.Round(total, 2);
        }
        public double Get_Total_Profit()
        {
            double total = 0;
            foreach (transactions trans in _transactions)
            {
                if (trans.Transaction_Type == 'S')
                {
                    total += trans.Transaction_Value;
                }
                else
                {
                    total -= trans.Transaction_Value;
                }
            }

            return Math.Round(total, 2);
        }
        public double VATRefund (double profits) 
        {
            if (VAT_available == true) 
            {
                double Total = (100 + 20) / 100;
                Total = profits / Total;
                if (Total >= 0) 
                {
                    return Math.Round(Total, 2);
                }
                else
                {
                    return 0.00;
                }
                
            }
            else
            {
                return 0.00;
            }
            
        }

    }
}
