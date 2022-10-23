// This file is part of the Project RLS.
//
// Copyright (c) 2019 Vladislav Sosedov.

using System;
using System.Data.SqlClient;

namespace DBFunctions
{
    static class DBFunctions
    {
        static SqlConnection connection;
        static DBFunctions() { Connect(); }
        static void Connect()
        {
            connection = new SqlConnection(@"Data Source=DESKTOP-JRIFFKK\SQL_EXPRESS;Initial Catalog=ProjectSQL;Integrated Security=True");
        }
        public static void Insert(string Name, string[] Columns, string[] Values)
        {
            connection.Open();

            string Query = "INSERT INTO " + Name + " (";

            for(int i = 0; i < Columns.Length - 1; i++)
            {
                Query += Columns[i] + ", ";
            }
            Query += Columns[Columns.Length - 1];
            Query += ") VALUES (";

            for (int i = 0; i < Values.Length - 1; i++)
            {
                Query += Values[i] + ", ";
            }
            Query += Values[Values.Length - 1];
            Query += ")";

            SqlCommand cmd;
            cmd = new SqlCommand(Query, connection);
            cmd.ExecuteNonQuery();

            connection.Close();
        }
        public static void Delete(string Name, string Condition)
        {
            connection.Open();

            string Query = "DELETE FROM " + Name + " WHERE " + Condition;
            SqlCommand cmd;
            cmd = new SqlCommand(Query, connection);
            cmd.ExecuteNonQuery();

            connection.Close();
        }
        public static string[,] Select(string Name, string Condition)
        {
            connection.Open();

            string Query = "SELECT * FROM " + Name + " WHERE " + Condition;
            int Counter = 0;

            SqlCommand cmd;
            cmd = new SqlCommand(Query, connection);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Counter++;
            }
            String[,] Output = new String[Counter, rd.VisibleFieldCount];

            connection.Close();
            connection.Open();

            cmd = new SqlCommand(Query, connection);
            rd = cmd.ExecuteReader();
            int i = 0;
            while (rd.Read())
            {
                for (int j = 0; j < rd.VisibleFieldCount; j++)
                {
                    Output[i, j] = rd[j].ToString();
                }
                i++;
            }

            connection.Close();

            return Output;
        }
        public static string[,] ExecuteCustomQuery(string Query)
        {
            connection.Open();

            int Counter = 0;
            SqlCommand cmd;
            cmd = new SqlCommand(Query, connection);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Counter++;
            }
            String[,] Output = new String[Counter, rd.VisibleFieldCount];

            connection.Close();
            connection.Open();

            cmd = new SqlCommand(Query, connection);
            rd = cmd.ExecuteReader();
            int i = 0;
            while (rd.Read())
            {
                for (int j = 0; j < rd.VisibleFieldCount; j++)
                {
                    Output[i, j] = rd[j].ToString();
                }
                i++;
            }

            connection.Close();

            return Output;
        }
    }
}