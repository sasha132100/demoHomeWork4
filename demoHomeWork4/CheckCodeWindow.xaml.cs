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
    /// Логика взаимодействия для CheckCodeWindow.xaml
    /// </summary>
    public partial class CheckCodeWindow : Window
    {
        public CheckCodeWindow()
        {
            InitializeComponent();
        }

        private void goToAdminWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkTextBox.Text == "0000")
            {
                AdminWindow adminWindow = new AdminWindow();
                this.Close();
                adminWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Код неверный");
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.ShowDialog();
            }
        }
    }
}
