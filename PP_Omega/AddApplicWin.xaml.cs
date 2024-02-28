using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PP_Omega
{
    /// <summary>
    /// Логика взаимодействия для AddApplicWin.xaml
    /// </summary>
    public partial class AddApplicWin : Window
    {
        
        AppOmegaEntities DB = new AppOmegaEntities();   
        private int ID_Applic = 0;
     

        public AddApplicWin(AppOmegaEntities DB, int ApID)
        {
            InitializeComponent();
            ID_Applic = ApID;
            if (ID_Applic > 0)
            {
                Editing(ID_Applic);
                Cmb_DepartMent.ItemsSource = DB.Department.ToList();
                Cmb_StatusAP.ItemsSource = DB.StatusAp.ToList();
                Cmb_TypeAp.ItemsSource = DB.TypeAp.ToList();
                Cmb_District.ItemsSource = DB.District.ToList();

            }
            else
            {
                Cmb_DepartMent.ItemsSource = DB.Department.ToList();
                Cmb_StatusAP.ItemsSource = DB.StatusAp.ToList();
                Cmb_TypeAp.ItemsSource = DB.TypeAp.ToList();
                Cmb_District.ItemsSource = DB.District.ToList();
                tbl_Tel.DataContext = DB.Applications.Where(a => a.ID_Staff == a.ID_Staff).ToList().FirstOrDefault();
                tbl_Staff.DataContext = DB.Applications.Where(a => a.ID_Staff == a.ID_Staff).ToList().FirstOrDefault();
                tbx_Date.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            }

        }

        private void Generate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string connectionString = "Data Source=DOGGY\\SQLEXPRESS;Initial Catalog=AppOmega;Integrated Security=True";
            string sqlCommandText = "SELECT MAX(NumApplic) FROM Applications";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlCommandText, connection);
                connection.Open();
                int maxNumApplic = (int?)command.ExecuteScalar() ?? 0;
                int uniqueNumApplic = maxNumApplic + 1;
                tbx_NumApplic.Text = uniqueNumApplic.ToString();
            }
        }

        private void Cmb_District_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Equals(Cmb_District.SelectedValue))
            {
                Cmb_CorNum.ItemsSource = null;
                Cmb_CorNum.ItemsSource = DB.Corpus.ToList();
            }
            else
            {
                Cmb_CorNum.ItemsSource = null;
                Cmb_CorNum.ItemsSource = DB.Corpus.Where(a => a.District.ID_District.ToString() == Cmb_District.SelectedValue.ToString()).ToList();
            }
        }
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (ID_Applic == 0)
            {
                NewApplic();
            }
            //SafeSave();
        }

        /*private void SafeSave() 
        {
            try
            {
                DB.SaveChanges();
                MessageBox.Show("Изменения сохранены", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка сохранения", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } */

        private void NewApplic()
        {
            if (tbx_NumApplic.Text != "" & Cmb_DepartMent.Text != "" & Cmb_StatusAP.Text != "" & Cmb_TypeAp.Text != "" & Cmb_District.Text != "" & Cmb_CorNum.Text != "" & tbx_Theme.Text != "")
            {
                Applications applications = new Applications();
                applications.NumApplic = Convert.ToInt32(tbx_NumApplic.Text);
                applications.DateApplic = Convert.ToDateTime(tbx_Date.Text);
                applications.ID_Department = DB.Department.Where(a => a.NameDepart == Cmb_DepartMent.Text).First().ID_Department;
                applications.ID_Staff = Loginer.StaffID;
                applications.ID_Status = DB.StatusAp.Where(a => a.NameStatus == Cmb_StatusAP.Text).First().ID_Status;
                applications.ID_Type = DB.TypeAp.Where(a => a.NameType == Cmb_TypeAp.Text).First().ID_Type;
                applications.ID_Corpus = DB.Corpus.Where(a => a.NameCorpus == Cmb_CorNum.Text).First().ID_Corpus;
                applications.Entrance = tbx_Entrance.Text;
                applications.Room = tbx_Room.Text;
                applications.Theme = tbx_Theme.Text;
                applications.Remark = tbx_Remark.Text;

                DB.Applications.Add(applications);
                DB.SaveChanges();
                MessageBox.Show("Заявка сохранена", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
            else
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Editing(int ID_Applic)
        {
            //var Dep = DB.Applications.Where(a => a.ID_Department == ID_Applic).FirstOrDefault();
            Applications Applic = DB.Applications.Where(a => a.ID_Applic == ID_Applic).Single();
            tbx_NumApplic.DataContext = Applic;
            tbx_Date.DataContext = Applic;
            tbx_Entrance.DataContext = Applic;
            tbx_Room.DataContext = Applic;
            tbx_Theme.DataContext = Applic;
            tbx_Remark.DataContext = Applic;
            tbl_Staff.DataContext = Applic;
            tbl_Tel.DataContext = Applic;

            Cmb_StatusAP.DataContext = Applic;
            Cmb_TypeAp.DataContext = Applic;
            Cmb_CorNum.DataContext = Applic;
            Cmb_District.DataContext = Applic;

            Department Depart = DB.Department.Where(a => a.ID_Department == Applic.ID_Department).Single();
            Cmb_DepartMent.DataContext = Depart;

            //Staff staff = DB.Staff.Where(a => a.ID_Staff == ID_Applic).FirstOrDefault();
            //StatusAp status = DB.StatusAp.Where(a => a.ID_Status == ID_Applic).FirstOrDefault();
            //TypeAp type = DB.TypeAp.Where(a => a.ID_Type == ID_Applic).Single();
            //Corpus corpus = DB.Corpus.Where(a => a.ID_Corpus == ID_Applic).Single();
            //District district = DB.District.Where(a => a.ID_District == corpus.ID_Corpus).Single();
        } 

         private void tbx_NumApplic_TextChanged(object sender, TextChangedEventArgs e)
        {
        /*
            string connectionString = "Data Source=DOGGY\\SQLEXPRESS;Initial Catalog=AppOmega;Integrated Security=True";
            string sqlCommandText = "SELECT MAX(NumApplic) FROM Applications";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlCommandText, connection);
                connection.Open();
                int maxNumApplic = (int?)command.ExecuteScalar() ?? 0;
                int uniqueNumApplic = maxNumApplic + 1;
                tbx_NumApplic.Text = uniqueNumApplic.ToString();
        } */
            } 
    
    }
}
