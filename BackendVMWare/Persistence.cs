﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace BackendVMWare
{
    public class Persistence
    {
        //TODO: Find a way to make these relative paths
        protected static string CONFIGPATH = "C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/Host.xls";
        protected static string VMCACHEPATH = "C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/VirtualMachines.xls";

        /// <summary>
        /// Write the file paths for the host configuration and virtual machine
        /// cache files.
        /// </summary>
        /// <param name="configPath">The filepath of the host configuration file</param>
        /// <param name="vmcachePath">The filepath of the virtual machine cache file</param>
        public static void ChangeFileLocations(string configPath, string vmcachePath)
        {
            CONFIGPATH = configPath;
            VMCACHEPATH = vmcachePath;
        }

        /// <summary>
        /// Write the given key-value pair to the host configuration file.
        /// </summary>
        /// <param name="option">The key option string</param>
        /// <param name="value">The value for the associated option</param>
        public static void WriteData(string option, string value)
        {
            DataTable data = new DataTable();
            string command = "UPDATE [Host$] SET [Value] = '" + value + "' WHERE [Option] = '" + option + "'";
            ConnectDataSource(CONFIGPATH, command, "update", data);
        }

        /// <summary>
        /// Write the given IP address for the given machine name to the
        /// static data source.
        /// </summary>
        /// <param name="name">The virtual machine's name</param>
        /// <param name="ip">The desired IP address</param>
        public static void WriteVMIP(string name, string ip)
        {
            DataTable data = new DataTable();
            string command = "UPDATE [VirtualMachines$] SET [IP] = '" + ip + "' WHERE [Name] = '" + name + "'";
            ConnectDataSource(VMCACHEPATH, command, "update", data);
        }

        /// <summary>
        /// Return the value associated with the given option in the host
        /// configuration file.
        /// </summary>
        /// <param name="option">The selected option key</param>
        /// <returns>The value associated with the given option</returns>
        public static string GetValue(string option)
        {
            DataTable data = new DataTable("Host");
            string command = "SELECT Value FROM [Host$] WHERE Option = '" + option + "'";
            ConnectDataSource(CONFIGPATH, command, "select", data);

            string result = data.Rows[0][0].ToString();

            return result;
        }

        /// <summary>
        /// Return the IP address of the given virtual machine from the
        /// static data source.
        /// </summary>
        /// <param name="name">The target machine's name</param>
        /// <returns>The IP address of the selected machine</returns>
        public static string GetIP(string name)
        {
            DataTable data = new DataTable("VirtualMachines");
            string command = "SELECT ip FROM [VirtualMachines$] WHERE Name = '" + name + "'";
            ConnectDataSource(VMCACHEPATH, command, "select", data);

            string result = data.Rows[0][0].ToString();

            return result;
        }

        /// <summary>
        /// Return all data stored within the static virtual machine data source.
        /// </summary>
        /// <returns>The entire data table of virtual machine information</returns>
        public static DataTable GetVirtualMachineData()
        {
            DataTable data = new DataTable("VirtualMachines");
            string command = "SELECT * FROM [VirtualMachines$]";
            ConnectDataSource(VMCACHEPATH, command, "select", data);

            return data;
        }

        private static void ConnectDataSource(string resourceFile, string command, string type, DataTable data)
        {
            String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source=" + resourceFile + ";" +
                "Extended Properties=Excel 8.0;";

            // Create connection object by using the preceding connection string.
            OleDbConnection objConn = new OleDbConnection(sConnectionString);

            // Open connection with the database.
            objConn.Open();

            // The code to follow uses a SQL SELECT command to display the data from the worksheet.
            OleDbCommand objCmdSelect = new OleDbCommand(command, objConn);

            // Create new OleDbDataAdapter that is used to build a DataSet
            // based on the preceding SQL SELECT statement.
            OleDbDataAdapter objAdapter = new OleDbDataAdapter();

            // Pass the Select command to the adapter.
            if (type.Equals("select"))
            {
                objAdapter.SelectCommand = objCmdSelect;
                objAdapter.Fill(data);
            }
            else if (type.Equals("update"))
            {
                objAdapter.UpdateCommand = objCmdSelect;
                objAdapter.UpdateCommand.ExecuteNonQuery();
            }

            // Clean up objects.
            objConn.Close();
        }

        /// <summary>
        ///  Find the lowest available IP address.
        /// </summary>
        /// <returns>The last octet of the lowest available IP address</returns>
        public static int GetNextAvailableIP()
        {
            DataTable virtualMachineInfo = GetVirtualMachineData();
            bool[] usedIP = new bool[256];

            foreach (DataRow currentRow in virtualMachineInfo.Rows)
            {
                string name = currentRow["Name"].ToString();
                string ip = currentRow["IP"].ToString();

                if (ip.Equals(""))
                {
                    if (name.Equals(""))
                        break;
                    else
                    {
                        Console.WriteLine("Empty IP address for machine name " +
                            name + " in the static data source / cache.");
                        continue;
                    }
                }

                int ipTail = -1;

                try
                {
                    ipTail = int.Parse(ip.Substring(ip.LastIndexOf('.') + 1));
                }
                catch(FormatException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                
                try
                {
                    usedIP[ipTail - 1] = true;
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("Invalid IP address " + ipTail + " in static data source / cache.");
                }
            }

            for (int index = 0; index < usedIP.Length; index++)
            {
                if (!usedIP[index])
                    return index + 1;
            }

            return -1;
        }
    }
}
