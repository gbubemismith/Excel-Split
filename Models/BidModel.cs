using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CsvUpload.Models
{
    public class BidModel
    {
        public string Zone { get; set; }
        public string Branch { get; set; }
        public string Lc_number { get; set; }
        public string Acct_no { get; set; }
        public string Customers_name { get; set; }
        public decimal Amount_won { get; set; }
        public string Other_cur_desc { get; set; }
        public decimal Rate { get; set; }
        public string Product_type { get; set; }
        public decimal NGN { get; set; }
        public string Item_of_import { get; set; }
        public string Form_m { get; set; }
        public DateTime Transaction_dt { get; set; }
        public string Tenor { get; set; }
        public string Fwd_type { get; set; }
        public DateTime Maturity_dt { get; set; }
        public string Customer_address { get; set; }


        internal static BidModel ParseFromCSV(string line)
        {
            var columns = line.Split(',');

            return new BidModel
            {
                Zone = columns[0].ToString(),
                Branch = columns[1].ToString(),
                Lc_number = columns[2].ToString(),
                Acct_no = columns[3].ToString(),
                Customers_name = columns[4].ToString(),
                //Amount_won = decimal.Parse(columns[5])
            };
        }


    }
}