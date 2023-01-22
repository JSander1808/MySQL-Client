﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MySQL_Client
{
    class MySQLHandle
    {
        private static string ip = null!;
        private static uint port = 0;
        private static string userId = null!;
        private static string password = null!;
        private static string database = null!;
        public MySQLHandle() { }

        public static void setConnectionString(string sendServer, uint sendPort, string sendUserID, string sendPassword, string sendDatabase) {
            ip= sendServer;
            port= sendPort;
            userId= sendUserID;
            password= sendPassword;
            database= sendDatabase;
        }

        public static void setDatabase(string SendDatabase) {
            database= SendDatabase;
        }

        private static string buildConnectionString() {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server= ip;
            builder.Port= port;
            builder.UserID= userId;
            builder.Password= password;
            if(database != null) {
                builder.Database= database;
            };
            return builder.ToString();
        }

        public static MySqlDataReader GetData(string command) {
            try {
                MySqlConnection connection = new MySqlConnection(buildConnectionString());
                connection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(command, connection);
                return mySqlCommand.ExecuteReader();
            }catch(Exception) {
                return null!;
            }
        }

        public static bool isConnected() {
            try {
                MySqlConnection connection = new MySqlConnection(buildConnectionString());
                connection.Open();
                return true;
            }catch(Exception) {
                return false;
            }
        }

        public static DataTable SelectData(string sendCommand) {
            MySqlConnection connection = new MySqlConnection(buildConnectionString());
            connection.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(sendCommand, connection);
            DataTable dt = new DataTable("Call Reciept");
            da.Fill(dt);
            return dt;
        }
    }
}