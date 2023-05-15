using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
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
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        demoHomeWork4Entities1 db = new demoHomeWork4Entities1();

        public AddWindow()
        {
            InitializeComponent();
            if (SystemContext.isEditing)
                LoadComponent();
        }

        private void LoadComponent()
        {
            var service = SystemContext.SchoolService;
            NameTextBox.Text = service.Name;
            DurationTextBox.Text = service.Duration;
            CostTextBox.Text = service.Cost.ToString();
            DiscountTextBox.Text = service.Discount.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var service = SystemContext.SchoolService;
            if (service == null)
                return;
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Введите название");
                return;
            }
            if (string.IsNullOrEmpty(DurationTextBox.Text))
            {
                MessageBox.Show("Введите длительность");
                return;
            }
                
            if (Convert.ToInt32(DurationTextBox.Text) > 240 || Convert.ToInt32(DurationTextBox.Text) <= 0)
            {
                MessageBox.Show("Длительность услуги должна быть не меньше 0 и не больше 4 часов!");
                return;
            }

            service.Name = NameTextBox.Text;
            service.Cost = Convert.ToInt32(CostTextBox.Text);
            service.Duration = DurationTextBox.Text;
            if (DiscountTextBox.Text == "")
                service.Discount = 0;
            else
                service.Discount = Convert.ToInt32(DiscountTextBox.Text);
            db.SchoolService.AddOrUpdate(service);
            try
            {
                db.SaveChanges();
                MessageBox.Show("Сервис успешно добавлен");
            }
            catch 
            {
                MessageBox.Show("Ошибка");
            }
            AdminWindow adminWindow = new AdminWindow();
            this.Close();
            adminWindow.ShowDialog();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            this.Close();
            adminWindow.ShowDialog();
        }
    }
}
