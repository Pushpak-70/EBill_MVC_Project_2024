﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EBill_MVC_Project_2024.Models;
using EBill_MVC_Project_2024.Repository;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq.Expressions;
using System.CodeDom;

namespace EBill_MVC_Project_2024.Repository
{
    public class Data : IData
    {
        public string ConntionString { get; set; }
        public Data()
        {
            ConntionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
        }
        public void SaveBillDetails(BillDetail details)
        {
            SqlConnection con = new SqlConnection(ConntionString);
            try
            {
                details.TotalAmount = details.Items.Sum(i => i.Price * i.Quantity);
                con.Open();
                SqlCommand cmd= new SqlCommand("spt_saveEBillDetails",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerName",details.CustomerName);
                cmd.Parameters.AddWithValue("@MobileNumber",details.MobileNumber);
                cmd.Parameters.AddWithValue("@Address",details.Address);
                cmd.Parameters.AddWithValue("@TotalAmount",details.TotalAmount);

                SqlParameter outputPara = new SqlParameter();
                outputPara.DbType = DbType.Int32;
                outputPara.Direction = ParameterDirection.Output;
                outputPara.ParameterName = "@Id";
                cmd.Parameters.Add(outputPara);
                cmd.ExecuteNonQuery();
                int id=int.Parse(outputPara.Value.ToString());
                if(details.Items.Count > 0)
                {
                    SaveBillItems(details.Items, con,id);
                }

            }
            catch(Exception)
            {
                throw;
            } 
            finally
            {
                con.Close();
            }
        }
      
        public void SaveBillItems(List<Items> items, SqlConnection con, int id)
        {
            try
            {
                string qry = "insert into tbl_BillItems(ProductName, Price, Quantity, BillId) values ";
                foreach (var item in items)
                {
                    qry += $"('{item.ProductName}', {item.Price}, {item.Quantity}, {id}),";
                }
                qry = qry.Remove(qry.Length - 1);
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }




        public List<BillDetail> GetAllDetails()
        {
            List<BillDetail> list = new List<BillDetail>();
            BillDetail detail;
    
            SqlConnection con = new SqlConnection(ConntionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spt_getAllEBillDetails", con);
                cmd.CommandType=CommandType.StoredProcedure;
                SqlDataReader reader= cmd.ExecuteReader();
                while(reader.Read()) 
                {
                    detail = new BillDetail();
                    detail.Id = int.Parse(reader["Id"].ToString());
                    detail.CustomerName = reader["CustomerName"].ToString();
                    detail.MobileNumber = reader["MobileNumber"].ToString();
                    detail.Address = reader["Address"].ToString();
                    detail.TotalAmount = int.Parse(reader["TotalAmount"].ToString()) ;
                    list.Add(detail);
                }
            }
            catch(Exception ) 
            {
                throw;            
            }

            finally
            {
                con.Close();
            }
            return list;
        }

        public BillDetail GetDetail(int Id)
        {
            SqlConnection con = new SqlConnection(ConntionString);
            BillDetail detail = new BillDetail();
            Items item;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spt_getEBillDetails", con);
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    detail.Id = int.Parse(reader["BillId"].ToString());
                    detail.CustomerName = reader["CustomerName"].ToString();
                    detail.MobileNumber = reader["MobileNumber"].ToString();
                    detail.Address = reader["Address"].ToString();
                    detail.TotalAmount = int.Parse(reader["TotalAmount"].ToString());
                    item = new Items();
                    item.Id = int.Parse(reader["ItemId"].ToString());
                    item.ProductName = reader["ProductName"].ToString();
                    item.Price = int.Parse(reader["Price"].ToString());
                    item.Quantity = int.Parse(reader["Quantity"].ToString());
                    detail.Items.Add(item);
                }
            }
            catch(Exception)
            {

            }
            finally
            {
                con.Close();
            }
            return detail; 
        }
    }
}