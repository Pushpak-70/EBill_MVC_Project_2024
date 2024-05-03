using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBill_MVC_Project_2024.Models;
using System.Data.SqlClient;

namespace EBill_MVC_Project_2024.Repository
{
    internal interface IData
    {
        void SaveBillDetails(BillDetail details);
        void SaveBillItems(List<Items> items, SqlConnection con,int id);

        List<BillDetail> GetAllDetails();

        BillDetail GetDetail(int id);
    }
}
