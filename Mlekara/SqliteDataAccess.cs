using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Mlekara.Models;

namespace Mlekara
{
    public class SqliteDataAccess
    {
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static PortSettingsModel LoadPortSettings()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PortSettingsModel>("select * from PortSettings", new DynamicParameters());
                if (output.Count() == 0)
                    return null;
                else
                    return output.First();
            }
        }

        public static void SavePortSettings(PortSettingsModel portSettings)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert or replace into PortSettings (Id, Port, BaudRate, DataBits, StopBits, ParityBits) values (@Id, @Port, @BaudRate, @DataBits, @StopBits, @ParityBits)", portSettings);
            }
        }

        #region Company

        public static string LoadCompanyName()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CompanyModel>("select * from Company", new DynamicParameters());
                if (output.Count() == 0)
                    return "Firma";
                else
                    return output.First().Name;
            }
        }

        public static void SaveCompanyName(string name)
        {
            CompanyModel company = new CompanyModel(1, name);
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert or replace into Company (Id, Name) values (@Id, @Name)", company);
            }
        }

        #endregion

        #region Devices

        public static List<DeviceModel> LoadDevices()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<DeviceModel>("select * from Device", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveDevice(DeviceModel device)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert or replace into Device (Id, Name, Active) values (@Id, @Name, @Active)", device);
            }
        }

        #endregion

        #region Probes

        public static List<ProbeModel> LoadProbes()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<ProbeModel>("select * from Probe", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveProbe(ProbeModel probe)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert or replace into Probe (Id, DeviceId, Name, Active, Min, Max, Marker) values (@Id, @DeviceId, @Name, @Active, @Min, @Max, @Marker)", probe);
            }
        }

        #endregion


        public static List<MeasurementModel> LoadMeasurements()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<MeasurementModel>("", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveMeasurement(MeasurementModel measurement)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("", measurement);
            }
        }
    }
}
