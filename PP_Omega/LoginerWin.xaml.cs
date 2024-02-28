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
using System.Windows.Shapes;
using NUnit.Framework; 

namespace PP_Omega
{
    /// <summary>
    /// Логика взаимодействия для LoginerWin.xaml
    /// </summary>
    public partial class LoginerWin : Window
    {
        private bool isClosed;
        public bool check = true;

        public bool IsClosed
        {
            get { return isClosed; }
            set { isClosed = value; }
        }

        static AppOmegaEntities DB = new AppOmegaEntities();
        public LoginerWin()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
               
            if (Loginer.IsAuthorizated(tb_login.Text, pw_password.Password))
            {
                MainWindow main = new MainWindow();

                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(
                 "Пароль или логин введены неверно",
                 "Ошибка",
                 MessageBoxButton.OK,
                 MessageBoxImage.Error);
            }
        }

        

        

        private void password_visible(object sender, MouseButtonEventArgs e)
        {
            var password = pw_password.Password;
            txbPass.Text = password;
            txbPass.Visibility = Visibility.Visible;
            pw_password.Visibility = Visibility.Hidden;
            IconVisible.Visibility = Visibility.Hidden;
            IconHidden.Visibility = Visibility.Visible;
        }

        private void password_hidden(object sender, MouseButtonEventArgs e)
        {
            var password = pw_password.Password;
            txbPass.Text = password;
            txbPass.Visibility = Visibility.Hidden;
            pw_password.Visibility = Visibility.Visible;
            IconVisible.Visibility = Visibility.Visible;
            IconHidden.Visibility = Visibility.Hidden;
            check = false;
        }
    }
}
    

