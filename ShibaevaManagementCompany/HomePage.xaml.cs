using ShibaevaManagementCompany.Pages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ShibaevaManagementCompany
{
    public partial class HomePage : Page
    {
        public class RecentRequest
        {
            public int RequestId { get; set; }
            public string Address { get; set; }
            public string Applicant { get; set; }
            public string Status { get; set; }
            public string Date { get; set; }
        }

        public HomePage()
        {
            InitializeComponent();
            LoadRecentRequests();
        }

        private void LoadRecentRequests()
        {
            var recentRequests = new List<RecentRequest>
            {
                new RecentRequest { RequestId = 1001, Address = "ул. Ленина, д. 10, кв. 5", Applicant = "Иванов А.А.", Status = "В работе", Date = DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy") },
                new RecentRequest { RequestId = 1002, Address = "ул. Пушкина, д. 25, кв. 12", Applicant = "Петрова И.И.", Status = "Новая", Date = DateTime.Now.ToString("dd.MM.yyyy") },
                new RecentRequest { RequestId = 1003, Address = "пр. Мира, д. 15, кв. 7", Applicant = "Сидоров В.В.", Status = "Выполнена", Date = DateTime.Now.AddDays(-3).ToString("dd.MM.yyyy") },
                new RecentRequest { RequestId = 1004, Address = "ул. Гагарина, д. 8, кв. 3", Applicant = "Кузнецова О.П.", Status = "В работе", Date = DateTime.Now.AddDays(-2).ToString("dd.MM.yyyy") },
                new RecentRequest { RequestId = 1005, Address = "ул. Советская, д. 30, кв. 9", Applicant = "Николаев С.М.", Status = "Новая", Date = DateTime.Now.ToString("dd.MM.yyyy") }
            };

            RecentRequestsGrid.ItemsSource = recentRequests;
        }

        private void NewRequest_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Navigate(new ServiceRequestsPage());
            }
        }
    }
}