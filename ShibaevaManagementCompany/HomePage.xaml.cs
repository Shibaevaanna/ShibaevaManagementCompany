using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ShibaevaManagementCompany.Pages
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            Loaded += HomePage_Loaded;
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStatistics();
            LoadRecentRequests();
        }

        private void LoadStatistics()
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    // Подсчет домов
                    int totalBuildings = db.Buildings.Count();
                    txtTotalBuildings.Text = totalBuildings.ToString();

                    // Подсчет квартир
                    int totalApartments = db.Apartments.Count();
                    txtTotalApartments.Text = totalApartments.ToString();

                    // Подсчет заявок
                    int totalRequests = db.ServiceRequests.Count();
                    txtTotalRequests.Text = totalRequests.ToString();

                    // Подсчет сотрудников
                    int totalEmployees = db.Employees.Count();
                    txtTotalEmployees.Text = totalEmployees.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статистики: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadRecentRequests()
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var recentRequests = db.ServiceRequests
                        .Include("Apartments")
                        .Include("Apartments.Buildings")
                        .OrderByDescending(r => r.RequestDate)
                        .Take(10)
                        .ToList();

                    dgRecentRequests.ItemsSource = recentRequests;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке последних заявок: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BtnQuickBuildings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BuildingsPage());
        }

        private void BtnQuickApartments_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ApartmentsPage());
        }

        private void BtnQuickRequests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ServiceRequestsPage());
        }

        private void BtnQuickHistory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RequestHistoryPage());
        }

        private void BtnAllRequests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ServiceRequestsPage());
        }
    }
}
