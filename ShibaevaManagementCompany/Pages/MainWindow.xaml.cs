using ShibaevaManagementCompany.Pages;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShibaevaManagementCompany
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigateToHome();
        }

        private void NavigateToHome()
        {
            MainFrame.Navigate(new HomePage());
            UpdateButtonStates(btnHome);
        }

        private void NavigateToBuildings()
        {
            MainFrame.Navigate(new BuildingsPage());
            UpdateButtonStates(btnBuildings);
        }

        private void NavigateToApartments()
        {
            MainFrame.Navigate(new ApartmentsPage());
            UpdateButtonStates(btnApartments);
        }

        private void NavigateToServiceRequests()
        {
            MainFrame.Navigate(new ServiceRequestsPage());
            UpdateButtonStates(btnServiceRequests);
        }

        private void NavigateToRequestHistory()
        {
            MainFrame.Navigate(new RequestHistoryPage());
            UpdateButtonStates(btnRequestHistory);
        }

        private void UpdateButtonStates(Button activeButton)
        {
            
            btnHome.Background = Brushes.Transparent;
            btnBuildings.Background = Brushes.Transparent;
            btnApartments.Background = Brushes.Transparent;
            btnServiceRequests.Background = Brushes.Transparent;
            btnRequestHistory.Background = Brushes.Transparent;

            
            if (activeButton != null)
            {
                activeButton.Background = new SolidColorBrush(Color.FromRgb(103, 186, 128)); // #67BA80
                activeButton.Foreground = Brushes.White;
            }
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            NavigateToHome();
        }

        private void BtnBuildings_Click(object sender, RoutedEventArgs e)
        {
            NavigateToBuildings();
        }

        private void BtnApartments_Click(object sender, RoutedEventArgs e)
        {
            NavigateToApartments();
        }

        private void BtnServiceRequests_Click(object sender, RoutedEventArgs e)
        {
            NavigateToServiceRequests();
        }

        private void BtnRequestHistory_Click(object sender, RoutedEventArgs e)
        {
            NavigateToRequestHistory();
        }
    }
}