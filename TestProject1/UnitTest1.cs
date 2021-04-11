using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System;
using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using HouseOfSoulSounds.Helpers;
using System.Data.SqlClient;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace HouseOfSoulSounds

{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetConnStringMethod()
        {
            //arrange
            DbConnection connection = new SqlConnection();
            Config.ConnectionString =
                "Data Source=(local)\\SQLEXPRESS; " +
                "Database=HouseOfSoulSoundsdb; " +
                "Persist Security Info=false; " +
                "User ID='DESKTOP-5BLGRDC\\Admin'; Password=''; " +
                "MultipleActiveResultSets=True; " +
                "Trusted_Connection=True;";
            ConnectionState expected = ConnectionState.Open;
            //act
            ConnectionState actual = GetState(connection, Config.ConnectionString);
            //assert
            Assert.AreEqual(expected, actual);
        }
        private ConnectionState GetState(DbConnection connection, string connStr)
        {
            ConnectionState actual = ConnectionState.Closed;
            try
            {
                connection.ConnectionString = connStr;
                connection.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.GetType()}: \n\t{e.Message}");
                if (connection != null)
                    Debug.WriteLine($"Connection state: {connection.State}");
            }
            finally
            {
                if (connection != null)
                {
                    actual = connection.State;
                    connection.Close();
                }
            }
            return actual;
        }
    }
}

