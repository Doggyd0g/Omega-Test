using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PP_Omega
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
    }

    public class Loginer
    {
        public static bool IsAdmin { get; set; } = false;
        public static int StaffID { get; set; } = 0;
        public static int DepartID { get; set; } = 0;

        private static AppOmegaEntities DB = new AppOmegaEntities();

        public static bool IsAuthorizated(string login, string password)
        {
            var loginer = DB.Loginers.Where(a => a.Login == login & a.Password == password).FirstOrDefault();
            if (loginer != null)
            {
                var loginStaff = DB.Staff.Where(b => b.ID_Staff == loginer.ID_Staff).FirstOrDefault();
                if (loginStaff != null)
                {
                    Loginer.DepartID = loginStaff.ID_Department;
                    Loginer.StaffID = loginStaff.ID_Staff;
                    Loginer.IsAdmin = (bool)loginStaff.IsAdmin;
                }
                else
                {
                    Loginer.DepartID = 0;
                    Loginer.StaffID = 0;
                    Loginer.IsAdmin = false;
                }
                return true;
            }
            else
            {
                return false;
            }
        } 

    }

}
