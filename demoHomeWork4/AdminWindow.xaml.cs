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

namespace demoHomeWork4
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private List<ServiceObject> serviceObjectsList;
        private List<SchoolService> serviceList = new List<SchoolService>();
        private string searchFilter = "";
        private int discountMark = 6;
        int sortMark = 0;

        public AdminWindow()
        {
            InitializeComponent();
            LoadSorts();
            LoadData();
            LoadServices();
        }

        private void LoadSorts()
        {
            comboBoxSort.Items.Add("Все");
            comboBoxSort.Items.Add("Цена по убыванию");
            comboBoxSort.Items.Add("Цена по возрастанию");
        }

        private void LoadServices()
        {
            serviceObjectsList = new List<ServiceObject>();
            using (var db = new demoHomeWork4Entities1())
            {
                foreach (var service in serviceList)
                {
                    double price = 0;
                    if (service.Discount == null)
                        price = service.Cost;
                    else
                        price = service.Cost * (double)(1 - (double)service.Discount / 100);
                    int time = Convert.ToInt32(service.Duration);
                    string discountText = "", panelColor = "#fff";
                    Visibility visibility = Visibility.Visible;
                    if (Convert.ToInt32(service.Discount) != 0)
                    {
                        discountText = $"*скидка {service.Discount}%";
                        panelColor = "#E7FABF";
                    }
                    else
                        visibility = Visibility.Collapsed;
                    var serviceObject = new ServiceObject()
                    {
                        PanelColor = panelColor,
                        Name = service.Name,
                        Service = service,
                        PriceText = $" {price} рублей за {time} минут",
                        DiscountText = discountText,
                        OldPriceVisibility = visibility,
                        MainImage = "\\Resources\\" + service.Photo
                    };
                    serviceObjectsList.Add(serviceObject);
                }
            }
            servicesPanel.ItemsSource = serviceObjectsList;
        }

        private void LoadData()
        {
            using (var db = new demoHomeWork4Entities1())
            {
                serviceList.Clear();
                serviceList = db.SchoolService.ToList();
                if (searchFilter != "")
                    serviceList = serviceList.Where(s => s.Name.Contains(searchFilter)).ToList();

                switch (discountMark)
                {
                    case 0:
                        serviceList = serviceList.Where(s => s.Discount < 5).ToList();
                        break;
                    case 1:
                        serviceList = serviceList.Where(s => s.Discount >= 5 && s.Discount < 15).ToList();
                        break;
                    case 2:
                        serviceList = serviceList.Where(s => s.Discount >= 15 && s.Discount < 30).ToList();
                        break;
                    case 3:
                        serviceList = serviceList.Where(s => s.Discount >= 30 && s.Discount < 70).ToList();
                        break;
                    case 4:
                        serviceList = serviceList.Where(s => s.Discount >= 70 && s.Discount <= 100).ToList();
                        break;
                }
                switch (sortMark)
                {
                    case 1:
                        serviceList = serviceList.OrderByDescending(s => s.Cost * (double)(1 - (double)s.Discount / 100)).ToList();
                        break;
                    case 2:
                        serviceList = serviceList.OrderBy(s => s.Cost * (double)(1 - (double)s.Discount / 100)).ToList();
                        break;
                }
                txtCount.Text = $"Выведено услуг: {serviceList.Count()} из {db.SchoolService.Count()}.";
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchFilter = (sender as TextBox).Text;
            LoadData();
            LoadServices();
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var service = ((((sender as Button).Parent as StackPanel).Parent as Grid).DataContext as ServiceObject).Service;
                using (var db = new demoHomeWork4Entities1())
                {
                    if (db.ClientService.FirstOrDefault(cs => cs.ServiceID == service.ID) != null)
                    {
                        MessageBox.Show("Невозможно удалить услугу. В базе данных есть записи ссылающие на услугу");
                        return;
                    }
                    db.Entry(service).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                LoadData();
                LoadServices();
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var service = ((((sender as Button).Parent as StackPanel).Parent as Grid).DataContext as ServiceObject).Service;
            SystemContext.isEditing = true;
            SystemContext.SchoolService = service;
            AddWindow addWindow = new AddWindow();
            this.Close();
            addWindow.ShowDialog();

        }

        private void myComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            discountMark = (sender as ComboBox).SelectedIndex;
            LoadData();
            LoadServices();
        }

        private void comboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sortMark = (sender as ComboBox).SelectedIndex;
            LoadData();
            LoadServices();
        }

        private void AddNewServiceButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isEditing = false;
            AddWindow addWindow = new AddWindow();
            this.Close();
            addWindow.ShowDialog();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.ShowDialog();
        }
    }
}
