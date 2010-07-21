using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ServerInfo.DomainModel
{
    public static class Utilities
    {
        public const string IpRegEx = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
        public const string WmiProgramGroup = "All Users";
        public const string StartMenuPrograms = @"Start Menu\Programs\";
        private const string SQL = @"Microsoft SQL Server\MSSQL.1\MSSQL\DATA";
        public static string[] PossibleSqlDirectories { get { return new string[] { @"\Program Files (x86)\" + SQL, @"\Program Files\" + SQL }; } }

        public class WMIQueries
        {
            public const string Windows = "SELECT csname, Caption, Version FROM Win32_OperatingSystem";
            public const string Applications = "SELECT Name FROM Win32_LogicalProgramGroup WHERE UserName = \"All Users\"";
            public const string Drives = "SELECT Caption FROM Win32_LogicalDisk";
        }

        public static void AddError(this TempDataDictionary tdd, string error)
        { tdd.AddNotification(error, "error"); }
        public static void AddWarning(this TempDataDictionary tdd, string warning)
        { tdd.AddNotification(warning, "warning"); }
        public static void AddSuccess(this TempDataDictionary tdd, string message)
        { tdd.AddNotification(message, "success"); }

        private static void AddNotification(this TempDataDictionary tdd, string notification, string type)
        {
            tdd[type] = tdd[type] == null ? notification : tdd[type] + "; " + notification;
        }
    }
}
