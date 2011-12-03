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
        protected static const string CONFIGPATH = "/Host.xls";
        protected static const string VMCACHEPATH = "/VirtualMachines.xls";

        public static void WriteData(string option, string value)
        {
            DataSet data = new DataSet();
            string command = "UPDATE Value WHERE Option = '" + option + "'";
            ConnectDataSource(CONFIGPATH, command, data);
        }

        public static void WriteVMIP(string name, string ip)
        {
            DataSet data = new DataSet();
            string command = "UPDATE  WHERE Option = '" + option + "'";
            ConnectDataSource(CONFIGPATH, command, data);
        }

        public static string GetValue(string option)
        {
            DataSet data = new DataSet();
            string command = "SELECT Value FROM TestName WHERE Option = '" + option + "'";
            ConnectDataSource(CONFIGPATH, command, data);

            return "null";
        }

        private static void ConnectDataSource(string filePath, string command, DataSet data)
        {
            String sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + filePath + ";" +
                "Extended Properties=Excel 12.0;";

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
