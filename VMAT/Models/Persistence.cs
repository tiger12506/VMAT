using System;
using System.Data;
using System.Collections.Generic;
using System.Data.OleDb;

namespace VMAT.Models
{
    /// <summary>
    /// Interface with the data files associated with the application.
    /// </summary>
    public class Persistence
    {
        /// <summary>
        /// The local filepath to the VMware host configuration file.
        /// </summary>
        protected static string configPath = AppConfiguration.GetDataFilesDirectory() + "Host.xls";

        /// <summary>
        /// The local filepath to the VMware virtual machine cache file.
        /// </summary>
        protected static string vmCachePath = AppConfiguration.GetDataFilesDirectory() + "VirtualMachines.xls";


        /// <summary>
        /// Write the file paths for the host configuration and virtual machine
        /// cache files. This is used primarily for testing pusposes.
        /// </summary>
        /// <param name="configPath">The filepath of the host configuration file</param>
        /// <param name="vmcachePath">The filepath of the virtual machine cache file</param>
        public static void ChangeFileLocations(string cfgPath, string vmcachePath)
        {
            configPath = cfgPath;
            vmCachePath = vmcachePath;
        }

        /// <summary>
        /// Write the given key-value pair to the host configuration file.
        /// </summary>
        /// <param name="option">The key option string</param>
        /// <param name="value">The value for the associated option</param>
        public static void WriteData(string option, string value)
        {
            string command = "UPDATE [Host$] SET [Value] = '" + value + "' WHERE [Option] = '" + option + "'";
            ExecuteUpdateQuery(configPath, command);
        }

        /// <summary>
        /// Write the given IP address for the given machine name to the
        /// static data source.
        /// </summary>
        /// <param name="name">The virtual machine's name</param>
        /// <param name="ip">The desired IP address</param>
        public static void WriteVMIP(string name, string ip)
        {
            string command = "UPDATE [VirtualMachines$] SET [IP] = '" + ip + "' WHERE [Name] = '" + name + "'";
            ExecuteUpdateQuery(vmCachePath, command);
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
            ExecuteSelectQuery(configPath, command, data);

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
            ExecuteSelectQuery(vmCachePath, command, data);

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
            ExecuteSelectQuery(vmCachePath, command, data);

            return data;
        }

        /// <summary>
        /// Connect to the given data file and perform a select operation on it.
        /// </summary>
        /// <param name="resourceFile">The selected data file name.</param>
        /// <param name="command">The query to execute.</param>
        /// <param name="data">The data set to place the selected cells in.</param>
        private static void ExecuteSelectQuery(string resourceFile, string command, DataTable data)
        {
            String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source=" + resourceFile + ";" +
                "Extended Properties=Excel 8.0;";

            try
            {
                OleDbConnection objConn = new OleDbConnection(sConnectionString);
                objConn.Open();

                OleDbCommand objCmd = new OleDbCommand(command, objConn);
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();

                objAdapter.SelectCommand = objCmd;
                objAdapter.Fill(data);

                objConn.Close();
            }
            catch (OleDbException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Connect to the given data file and perform an update operation on it.
        /// </summary>
        /// <param name="resourceFile">The selected data file name.</param>
        /// <param name="command">The query to execute.</param>
        private static void ExecuteUpdateQuery(string resourceFile, string command)
        {
            String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source=" + resourceFile + ";" +
                "Extended Properties=Excel 8.0;";

            try
            {
                OleDbConnection objConn = new OleDbConnection(sConnectionString);
                objConn.Open();

                OleDbCommand objCmd = new OleDbCommand(command, objConn);
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();

                objAdapter.UpdateCommand = objCmd;
                objAdapter.UpdateCommand.ExecuteNonQuery();

                objConn.Close();
            }
            catch (OleDbException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Query all Virtual Machine information from VMware. Merge this data with the
        /// data currently stored in the data file. Rewrite this merged data table
        /// back into the data file.
        /// </summary>
        public static void RefreshVMDataFile()
        {
            String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source=" + vmCachePath + ";" +
                "Extended Properties=Excel 8.0;";

            VirtualMachineManager manager = new VirtualMachineManager();
            DataTable data = new DataTable();

            foreach (string imageName in manager.GetRunningVMs())
            {
                var vm = new RunningVirtualMachine(imageName);
                DataRow row = data.NewRow(); ;

                // Add more items as needed
                row["Name"] = vm.GetMachineName();
                row["IP"] = vm.IP;
                row["Hostname"] = vm.Hostname;
            }

            try
            {
                OleDbConnection conn = new OleDbConnection(sConnectionString);
                conn.Open();

                OleDbDataAdapter adapter = new OleDbDataAdapter();

                // Pull the existing data in the file and merge it with the newly queried
                // items, preserving any newly queried data.
                DataTable existing = new DataTable();
                adapter.Fill(existing);
                data.Merge(existing, true);

                adapter.Update(data);

                conn.Close();
            }
            catch (OleDbException e)
            {
                throw e;
            }
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
                    Console.WriteLine(e.Message + ": Invalid IP address " + 
                        ipTail + " in static data source / cache.");
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
