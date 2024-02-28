using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace PP_Omega
{
    /// <summary>
    /// Логика взаимодействия для FullApplicPage.xaml
    /// </summary>
    public partial class FullApplicPage : Page
    {
        static AppOmegaEntities DB = new AppOmegaEntities();
        private int ApID = 0;

        public FullApplicPage()
        {
            InitializeComponent();
            if (Loginer.IsAdmin)
            {
                btn_Add.Visibility = Visibility.Visible;
                dtg_Applic.ItemsSource = DB.Applications.ToList();
            }
            else
            {
                btn_Del.Visibility = Visibility.Hidden;
                btn_Add.Visibility = Visibility.Hidden;
                btn_print.Visibility = Visibility.Hidden;
                dtg_Applic.ItemsSource = DB.Applications.Where(a => a.Staff.ID_Department == Loginer.DepartID).ToList();
            }
        }

        private void dtg_Applic_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            int ID_Applic = (int)dtg_Applic.SelectedValue;
            ApID = ID_Applic;
            AddApplicWin add = new AddApplicWin(DB, ID_Applic);
            add.Show();
            dtg_Applic.ItemsSource = DB.Applications.ToList();
        }

        private void dtg_Applic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtg_Applic.SelectedValue != null)
            {
                var temp = DB.Applications.ToList().Where(a => a.ID_Applic == (int)dtg_Applic.SelectedValue).Single();
                BRD_INFO.DataContext = temp;
            }
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            AddApplicWin add = new AddApplicWin(DB, ApID);
            add.Show();
            dtg_Applic.ItemsSource = DB.Applications.ToList();
        }

        private void btn_Del_Click(object sender, RoutedEventArgs e)
        {
            if (dtg_Applic.SelectedValue != null)
            {
                int ID_Applic = (int)dtg_Applic.SelectedValue;
                var Del = DB.Applications.Where(a => a.ID_Applic == ID_Applic).Single();
                MessageBoxResult res = MessageBox.Show("Вы действительно хотите удалить данные об этом водителе?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    DB.Applications.Remove(Del);
                    DB.SaveChanges();
                    dtg_Applic.ItemsSource = DB.Applications.ToList();

                }
            }
            else
            {
                MessageBox.Show("Поле для удаления не выбрано", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void SearchTxb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string Search = SearchTxb.Text;
               // DB = new AppOmegaEntities();
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

                var match = DB.Applications.Where(a => (a.Theme + " " + a.StatusAp.NameStatus + " " +
                    a.NumApplic + " " + a.TypeAp.NameType + " " + a.Corpus.NameCorpus + " "
                    + a.Department.NameDepart + " ").Trim().ToLower().Contains(Search.ToLower()) |
                    IsSearch | (isValidDateTimeFormat && a.DateApplic == searchDateTime)).ToList();

                dtg_Applic.ItemsSource = match;
            }
        }

        private void Img_Search_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dtg_Applic.ItemsSource = DB.Applications.ToList();
        }

        
        private void btn_print_Click(object sender, RoutedEventArgs e)
        {
            
            // Создаем документ PDF 
            Document doc = new Document();

            try
            {
                // Создаем объект записи пдф-документа в файл 
                PdfWriter.GetInstance(doc, new FileStream("C:\\2023-06-28.pdf", FileMode.Create));

                // Открываем документ 
                doc.Open();

                // Определение шрифта необходимо для сохранения кириллического текста 
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);

                // Создаем таблицу 
                // Создаем таблицу 
                PdfPTable table = new PdfPTable(dtg_Applic.Columns.Count);

                // Добавляем заголовки таблицы 
                for (int j = 0; j < dtg_Applic.Columns.Count; j++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(dtg_Applic.Columns[j].Header.ToString(), font));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.BackgroundColor = new BaseColor(240, 240, 240); //цвет фона ячейки 
                    table.AddCell(cell);
                }

                // Определяем индекс выбранной строки
                int selectedRowIndex = -1;

                if (dtg_Applic.SelectedItems.Count > 0)
                {
                    selectedRowIndex = dtg_Applic.Items.IndexOf(dtg_Applic.SelectedItem);
                }

                // Добавляем данные из выбранной строки в DataGrid 
                if (selectedRowIndex != -1)
                {
                    for (int k = 0; k < dtg_Applic.Columns.Count; k++)
                    {
                        object value = GetDataGridCell(selectedRowIndex, k);

                        if (value != null)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(value.ToString(), font));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                        }
                    }
                }

                doc.Add(table);

                if ((selectedRowIndex + 1) % 7 == 0 || selectedRowIndex == dtg_Applic.Items.Count - 1)
                {
                    doc.NewPage();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                // Закрываем документ 
                doc.Close();
            }

            MessageBox.Show("Pdf-документ сохранен");
            
        }
       
        private object GetDataGridCell(int rowIndex, int columnIndex)
        {
            DataGridRow row = dtg_Applic.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;

            if (row != null)
            {
                TextBlock cellContent = dtg_Applic.Columns[columnIndex].GetCellContent(row) as TextBlock;

                if (cellContent != null)
                {
                    return cellContent.Text;
                }
            }

            return null;
        }
        


    }
} 
