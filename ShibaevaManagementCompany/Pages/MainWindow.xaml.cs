using ShibaevaManagementCompany.Pages;
using System.Windows;
using System.Windows.Controls;

namespace ShibaevaManagementCompany
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadHomePage();
        }

        private void LoadHomePage()
        {
            MainFrame.Navigate(new HomePage());
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomePage());
        }

        private void RequestsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ServiceRequestsPage());
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RequestHistoryPage());
        }

        private void PaymentsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Страница платежей в разработке", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DebtsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Страница долгов в разработке", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Страница сотрудников в разработке", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BuildingsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Страница домов в разработке", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ApartmentsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Страница квартир в разработке", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}