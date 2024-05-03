using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EBill_MVC_Project_2024.Models;

namespace EBill_MVC_Project_2024.Models
{
    public class BillDetail
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public int TotalAmount { get; set; }

        public List<Items> Items { get; set; }

        public BillDetail()
        {
            Items = new List<Items>();
        
        }
    }
}