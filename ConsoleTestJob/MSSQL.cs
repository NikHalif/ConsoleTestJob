using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleTestJob
{
    public class MSSQL
    {
        protected SqlConnection connection;

        static protected DateTime MinDate = new DateTime(1754, 1, 1);
        static protected DateTime MaxDate = new DateTime(9999, 12, 31);

        public void Connection() { connection.Open(); }

        public void Disconnect() { connection.Close(); }


        protected int SendCommand(SqlCommand command)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                return command.ExecuteNonQuery();
            }
            else throw new Exception($"Запрос не выполнен!\nНет подключения к базе данных.\nСтатус: {connection.State}");
        }

        protected DataTable GetTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("dealNumber", typeof(string)));
            tbl.Columns.Add(new DataColumn("dealDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("buyerInn", typeof(string)));
            tbl.Columns.Add(new DataColumn("buyerName", typeof(string)));
            tbl.Columns.Add(new DataColumn("sellerInn", typeof(string)));
            tbl.Columns.Add(new DataColumn("sellerName", typeof(string)));
            tbl.Columns.Add(new DataColumn("woodVolumeBuyer", typeof(double)));
            tbl.Columns.Add(new DataColumn("woodVolumeSeller", typeof(double)));
            return tbl;
        }

        public void Bulk(List<Deal> deals)
        {
            DataTable tbl = GetTable();

            foreach (var deal in deals)
            {
                if (CountDeal(deal) <= 0)
                {
                    DataRow dr = tbl.NewRow();
                    dr["dealNumber"] = deal.dealNumber;
                    if (deal.dealDate == null || deal.dealDate < MinDate || deal.dealDate < MaxDate) dr["dealDate"] = DBNull.Value;
                    else dr["dealDate"] = deal.dealDate;
                    if (String.IsNullOrEmpty(deal.buyerInn)) dr["buyerInn"] = DBNull.Value;
                    else dr["buyerInn"] = deal.buyerInn;
                    if (String.IsNullOrEmpty(deal.buyerName)) dr["buyerName"] = DBNull.Value;
                    else dr["buyerName"] = deal.buyerName;
                    if (String.IsNullOrEmpty(deal.sellerInn)) dr["sellerInn"] = DBNull.Value;
                    else dr["sellerInn"] = deal.sellerInn;
                    if (String.IsNullOrEmpty(deal.sellerName)) dr["sellerName"] = DBNull.Value;
                    else dr["sellerName"] = deal.sellerName;
                    dr["woodVolumeBuyer"] = deal.woodVolumeBuyer;
                    dr["woodVolumeSeller"] = deal.woodVolumeSeller;
                    tbl.Rows.Add(dr);
                }
            }

            using (SqlBulkCopy objbulk = new SqlBulkCopy(connection))
            {

                objbulk.DestinationTableName = "Deals";

                objbulk.ColumnMappings.Add("dealNumber", "dealNumber");
                objbulk.ColumnMappings.Add("dealDate", "dealDate");
                objbulk.ColumnMappings.Add("buyerInn", "buyerInn");
                objbulk.ColumnMappings.Add("buyerName", "buyerName");
                objbulk.ColumnMappings.Add("sellerInn", "sellerInn");
                objbulk.ColumnMappings.Add("sellerName", "sellerName");
                objbulk.ColumnMappings.Add("woodVolumeBuyer", "woodVolumeBuyer");
                objbulk.ColumnMappings.Add("woodVolumeSeller", "woodVolumeSeller");

                objbulk.WriteToServer(tbl);
            }
        }

        public int Insert(Deal deal)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandText = "INSERT INTO Deals (buyerInn, buyerName, dealDate, dealNumber, sellerInn, sellerName, woodVolumeBuyer, woodVolumeSeller) " +
                          " VALUES ( @buyerInn, @buyerName, @dealDate, @dealNumber, @sellerInn, @sellerName, @woodVolumeBuyer, @woodVolumeSeller )";
                command.Connection = connection;
                if (String.IsNullOrEmpty(deal.buyerInn)) command.Parameters.AddWithValue("buyerInn", DBNull.Value);
                else command.Parameters.AddWithValue("buyerInn", deal.buyerInn);
                if (String.IsNullOrEmpty(deal.buyerName)) command.Parameters.AddWithValue("buyerName", DBNull.Value);
                else command.Parameters.AddWithValue("buyerName", deal.buyerName);
                if (deal.dealDate == null || deal.dealDate < MinDate || deal.dealDate < MaxDate) command.Parameters.AddWithValue("dealDate", DBNull.Value);
                else command.Parameters.AddWithValue("dealDate", deal.dealDate);
                command.Parameters.AddWithValue("dealNumber", deal.dealNumber);
                if (String.IsNullOrEmpty(deal.sellerInn)) command.Parameters.AddWithValue("sellerInn", DBNull.Value);
                else command.Parameters.AddWithValue("sellerInn", deal.sellerInn);
                if (String.IsNullOrEmpty(deal.sellerName)) command.Parameters.AddWithValue("sellerName", DBNull.Value);
                else command.Parameters.AddWithValue("sellerName", deal.sellerName);
                command.Parameters.AddWithValue("woodVolumeBuyer", deal.woodVolumeBuyer);
                command.Parameters.AddWithValue("woodVolumeSeller", deal.woodVolumeSeller);

                return SendCommand(command);
            }
        }

        public int CountDeal(Deal deal)
        {
            var queryString = @"SELECT COUNT(*) FROM Deals WHERE dealNumber=@dealNumber AND dealDate=@dealDate AND buyerInn=@buyerInn AND buyerName=@buyerName AND sellerInn=@sellerInn AND sellerName=@sellerName AND woodVolumeBuyer=@woodVolumeBuyer AND woodVolumeSeller=@woodVolumeSeller";
            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                if (String.IsNullOrEmpty(deal.buyerInn)) command.Parameters.AddWithValue("buyerInn", DBNull.Value);
                else command.Parameters.AddWithValue("buyerInn", deal.buyerInn);
                if (String.IsNullOrEmpty(deal.buyerName)) command.Parameters.AddWithValue("buyerName", DBNull.Value);
                else command.Parameters.AddWithValue("buyerName", deal.buyerName);
                if (deal.dealDate == null || deal.dealDate < MinDate || deal.dealDate < MaxDate) command.Parameters.AddWithValue("dealDate", DBNull.Value);
                else command.Parameters.AddWithValue("dealDate", deal.dealDate);
                command.Parameters.AddWithValue("dealNumber", deal.dealNumber);
                if (String.IsNullOrEmpty(deal.sellerInn)) command.Parameters.AddWithValue("sellerInn", DBNull.Value);
                else command.Parameters.AddWithValue("sellerInn", deal.sellerInn);
                if (String.IsNullOrEmpty(deal.sellerName)) command.Parameters.AddWithValue("sellerName", DBNull.Value);
                else command.Parameters.AddWithValue("sellerName", deal.sellerName);
                command.Parameters.AddWithValue("woodVolumeBuyer", deal.woodVolumeBuyer);
                command.Parameters.AddWithValue("woodVolumeSeller", deal.woodVolumeSeller);

                return (Int32)command.ExecuteScalar();
            }
        }

        public int SelectInt()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(*) FROM Deals";
                return (Int32)command.ExecuteScalar();
            }
        }


        public MSSQL(string conString)
        {
            connection = new SqlConnection(conString);
        }

        protected ref SqlCommand AddParamsDeal(ref SqlCommand command, Deal deal)
        {
            if (String.IsNullOrEmpty(deal.buyerInn)) command.Parameters.AddWithValue("buyerInn", DBNull.Value);
            else command.Parameters.AddWithValue("buyerInn", deal.buyerInn);
            if (String.IsNullOrEmpty(deal.buyerName)) command.Parameters.AddWithValue("buyerName", DBNull.Value);
            else command.Parameters.AddWithValue("buyerName", deal.buyerName);
            if (deal.dealDate == null || deal.dealDate < MinDate || deal.dealDate < MaxDate) command.Parameters.AddWithValue("dealDate", DBNull.Value);
            else command.Parameters.AddWithValue("dealDate", deal.dealDate);
            command.Parameters.AddWithValue("dealNumber", deal.dealNumber);
            if (String.IsNullOrEmpty(deal.sellerInn)) command.Parameters.AddWithValue("sellerInn", DBNull.Value);
            else command.Parameters.AddWithValue("sellerInn", deal.sellerInn);
            if (String.IsNullOrEmpty(deal.sellerName)) command.Parameters.AddWithValue("sellerName", DBNull.Value);
            else command.Parameters.AddWithValue("sellerName", deal.sellerName);
            command.Parameters.AddWithValue("woodVolumeBuyer", deal.woodVolumeBuyer);
            command.Parameters.AddWithValue("woodVolumeSeller", deal.woodVolumeSeller);

            return ref command;
        }
    }
}
