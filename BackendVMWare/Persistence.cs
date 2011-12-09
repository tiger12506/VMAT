using System;
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
        protected const string CONFIGPATH = "C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/HostTest.xls";
        protected const string VMCACHEPATH = "C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/VirtualMachinesTest.xls";

        /// <summary>
        /// Write the given key-value pair to the host configuration file.
        /// </summary>
        /// <param name="option">The key option string</param>
        /// <param name="value">The value for the associated option</param>
        public static void WriteData(string option, string value)
        {
            DataSet data = new DataSet();
            string command = String.Format("UPDATE [Host] SET Value='{1}' WHERE Option='{0}' IF @@ROWCOUNT=0 INSERT INTO [Host] (Option, Value) VALUES ({0}, {1})", option, value);
            ConnectDataSource(CONFIGPATH, command, data);
        }

        /// <summary>
        /// Write the given IP address for the given machine name to the
        /// static data source.
        /// </summary>
        /// <param name="name">The virtual machine's name</param>
        /// <param name="ip">The desired IP address</param>
        public static void WriteVMIP(string name, string ip)
        {
            DataSet data = new DataSet();
            string command = "UPDATE [VirtualMachines] SET IP = '" + ip + "' WHERE Name = '" + name + "'";
            ConnectDataSource(VMCACHEPATH, command, data);
        }

        /// <summary>
        /// Return the value associated with the given option in the host
        /// configuration file.
        /// </summary>
        /// <param name="option">The selected option key</param>
        /// <returns>The value associated with the given option</returns>
        public static string GetValue(string option)
        {
            DataSet data = new DataSet();
            string command = "SELECT Value FROM [Host$] WHERE Option = '" + option + "'";
            ConnectDataSource(CONFIGPATH, command, data);

            string result = data.Tables[0].Rows[0][0].ToString();

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
            DataSet data = new DataSet();
            string command = "SELECT IP FROM [VirtualMachines$] WHERE Name = '" + name + "'";
            ConnectDataSource(VMCACHEPATH, command, data);

            string result = data.Tables[0].Rows[0][0].ToString();

            return result;
        }

        /// <summary>
        /// Return all data stored within the static virtual machine data source.
        /// </summary>
        /// <returns>The entire data table of virtual machine information</returns>
        public static DataSet GetVirtualMachineData()
        {
            DataSet data = new DataSet();
            string command = "SELECT * FROM [VirtualMachines$];";
            ConnectDataSource(VMCACHEPATH, command, data);

            return data;
        }

        private static void ConnectDataSource(string resourceFile, string command, DataSet data)
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
            objAdapter.SelectCommand = objCmdSelect;

            // Fill the DataSet with the information from the worksheet.
            //objAdapter.Fill(data, "XLData");
            objAdapter.Fill(data);

            // Clean up objects.
            objConn.Close();
        }
    }
}
