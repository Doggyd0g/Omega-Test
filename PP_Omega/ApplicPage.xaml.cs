using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace PP_Omega
{
    /// <summary>
    /// Логика взаимодействия для ApplicPage.xaml
    /// </summary>
    public partial class ApplicPage : Page
    {
        static AppOmegaEntities DB = new AppOmegaEntities();
        private int ApID = 0;

        public ApplicPage()
        {
            InitializeComponent();
            if (Loginer.IsAdmin)
            {
                btn_Add.Visibility = Visibility.Visible;
                items_list.ItemsSource = DB.Applications.ToList();
            }
            else
            {
                items_list.ItemsSource = DB.Applications.Where(a => a.Staff.ID_Department == Loginer.DepartID).ToList();
            }
        }
        
        private void btn_Click_Add(object sender, RoutedEventArgs e)
        {
            AddApplicWin add = new AddApplicWin(DB, ApID);
            add.Show();
            items_list.ItemsSource = DB.Applications.ToList();
        }
        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string Search = SearchTxb.Text;
                //DB = new AppOmegaEntities();
                bool IsSearch = false;

                if (SearchTxb.Text == String.Empty)
                {
                    IsSearch = true;
                }
                else
                {
                    IsSearch = false;
                }

                DateTime searchDateTime;
                bool isValidDateTimeFormat = DateTime.TryParseExact(Search, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out searchDateTime);

                var match = DB.Applications.Where(a =>(a.Theme + " " + a.StatusAp.NameStatus + " " +
                    a.NumApplic + " " + a.TypeAp.NameType + " " + a.Corpus.NameCorpus + " " 
                    + a.Department.NameDepart + " ").Trim().ToLower().Contains(Search.ToLower()) |
                    IsSearch | (isValidDateTimeFormat && a.DateApplic == searchDateTime)).ToList();


                items_list.ItemsSource = match;
            }
        }
      

        private void BtnInfo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var temp = DB.Applications.ToList().Where(a => a.ID_Applic.ToString() == (sender as Image).Tag.ToString()).FirstOrDefault();
            BRD_INFO.DataContext = temp;
        }

        private void BtnDel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Loginer.IsAdmin)
            {
                var temp = DB.Applications.ToList().Where(a => a.ID_Applic.ToString() == (sender as Image).Tag.ToString()).Single();
                items_list.DataContext = temp;

                MessageBoxResult res = MessageBox.Show("Вы действительно хотите удалить данную заявку?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    DB.Applications.Remove(temp);
                    DB.SaveChanges();
                    items_list.ItemsSource = DB.Applications.ToList();  
                }
            }
            else
            {
                MessageBox.Show("У вас нет доступа для удаления заявок", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Img_Search_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            items_list.ItemsSource = DB.Applications.ToList();  
        }
    }
}
