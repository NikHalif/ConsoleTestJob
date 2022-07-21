using System;
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

        /// <summary>
        /// Добавить значение в базу при его отсутсвии
        /// </summary>
        /// <param name="deal"> Объект сделки </param>
        /// <returns></returns>
        public int IsAddDeal(Deal deal)
        {
            var queryString = @"SELECT COUNT(*) FROM Deals WHERE buyerInn=@buyerInn AND buyerName=@buyerName AND dealDate=@dealDate AND dealNumber=@dealNumber AND sellerInn=@sellerInn AND sellerName=@sellerName AND woodVolumeBuyer=@woodVolumeBuyer AND woodVolumeSeller=@woodVolumeSeller";
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

                var count = (Int32)command.ExecuteScalar();
                if (count > 0) return -1;
            }
            return Insert(deal);
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
