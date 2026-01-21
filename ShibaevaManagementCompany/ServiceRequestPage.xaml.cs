using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShibaevaManagementCompany
{
    public partial class ServiceRequestsPage : Page
    {
        public class ServiceRequest
        {
            public int RequestId { get; set; }
            public string Address { get; set; }
            public string ApplicantName { get; set; }
            public string Phone { get; set; }
            public string Description { get; set; }
            public string Assignee { get; set; }
            public string Status { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        private List<ServiceRequest> _allRequests;
        private List<ServiceRequest> _filteredRequests;

        public ServiceRequestsPage()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            // Имитация данных из базы
            _allRequests = new List<ServiceRequest>
            {
                new ServiceRequest { RequestId = 1001, Address = "ул. Ленина, д. 10, кв. 5", ApplicantName = "Иванов Александр Алексеевич", Phone = "+7(900)123-45-67", Description = "Протекает кран на кухне", Assignee = "Смирнов П.И.", Status = "В работе", CreatedDate = DateTime.Now.AddDays(-2) },
                new ServiceRequest { RequestId = 1002, Address = "ул. Пушкина, д. 25, кв. 12", ApplicantName = "Петрова Ирина Ивановна", Phone = "+7(900)234-56-78", Description = "Не работает розетка в зале", Assignee = "Кузнецов А.В.", Status = "Новая", CreatedDate = DateTime.Now.AddDays(-1) },
                new ServiceRequest { RequestId = 1003, Address = "пр. Мира, д. 15, кв. 7", ApplicantName = "Сидоров Владимир Владимирович", Phone = "+7(900)345-67-89", Description = "Требуется покраска подъезда", Assignee = "Николаев С.М.", Status = "Выполнена", CreatedDate = DateTime.Now.AddDays(-5) },
                new ServiceRequest { RequestId = 1004, Address = "ул. Гагарина, д. 8, кв. 3", ApplicantName = "Кузнецова Ольга Петровна", Phone = "+7(900)456-78-90", Description = "Засорилась канализация", Assignee = "Васильев И.К.", Status = "В работе", CreatedDate = DateTime.Now.AddDays(-3) },
                new ServiceRequest { RequestId = 1005, Address = "ул. Советская, д. 30, кв. 9", ApplicantName = "Николаев Сергей Михайлович", Phone = "+7(900)567-89-01", Description = "Не закрывается входная дверь", Assignee = "Петров А.С.", Status = "Новая", CreatedDate = DateTime.Now },
                new ServiceRequest { RequestId = 1006, Address = "ул. Ленина, д. 10, кв. 8", ApplicantName = "Смирнова Елена Викторовна", Phone = "+7(900)678-90-12", Description = "Треснуло окно в спальне", Assignee = "Иванов М.П.", Status = "Выполнена", CreatedDate = DateTime.Now.AddDays(-7) },
                new ServiceRequest { RequestId = 1007, Address = "ул. Пушкина, д. 25, кв. 15", ApplicantName = "Васильев Игорь Константинович", Phone = "+7(900)789-01-23", Description = "Протечка крыши", Assignee = "Сидоров В.В.", Status = "В работе", CreatedDate = DateTime.Now.AddDays(-4) }
            };

            _filteredRequests = new List<ServiceRequest>(_allRequests);
            RequestsDataGrid.ItemsSource = _filteredRequests;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditWindow = new RequestAddEditWindow(null);
            addEditWindow.Owner = Window.GetWindow(this);
            if (addEditWindow.ShowDialog() == true)
            {
                LoadRequests(); // Перезагружаем список после добавления
                ShowMessage("Заявка успешно добавлена!", "Успех", MessageBoxImage.Information);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem == null)
            {
                ShowMessage("Пожалуйста, выберите заявку для редактирования", "Внимание", MessageBoxImage.Warning);
                return;
            }

            var selectedRequest = (ServiceRequest)RequestsDataGrid.SelectedItem;
            var addEditWindow = new RequestAddEditWindow(selectedRequest);
            addEditWindow.Owner = Window.GetWindow(this);
            if (addEditWindow.ShowDialog() == true)
            {
                LoadRequests(); // Перезагружаем список после редактирования
                ShowMessage("Заявка успешно обновлена!", "Успех", MessageBoxImage.Information);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem == null)
            {
                ShowMessage("Пожалуйста, выберите заявку для удаления", "Внимание", MessageBoxImage.Warning);
                return;
            }

            var selectedRequest = (ServiceRequest)RequestsDataGrid.SelectedItem;
            var result = MessageBox.Show($"Вы уверены, что хотите удалить заявку #{selectedRequest.RequestId}?\nЭто действие нельзя отменить.",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // Имитация удаления из базы данных
                _allRequests.RemoveAll(r => r.RequestId == selectedRequest.RequestId);
                LoadRequests();
                ShowMessage($"Заявка #{selectedRequest.RequestId} успешно удалена", "Успех", MessageBoxImage.Information);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
            SearchTextBox.Text = "Поиск по адресу или ФИО...";
            SearchTextBox.Foreground = Brushes.Gray;
            FilterComboBox.SelectedIndex = 0;
            ShowMessage("Список заявок обновлен", "Информация", MessageBoxImage.Information);
        }

        private void RequestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditButton.IsEnabled = RequestsDataGrid.SelectedItem != null;
            DeleteButton.IsEnabled = RequestsDataGrid.SelectedItem != null;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text == "Поиск по адресу или ФИО..." || string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                _filteredRequests = new List<ServiceRequest>(_allRequests);
            }
            else
            {
                var searchText = SearchTextBox.Text.ToLower();
                _filteredRequests = _allRequests
                    .Where(r => r.Address.ToLower().Contains(searchText) ||
                                r.ApplicantName.ToLower().Contains(searchText) ||
                                r.Description.ToLower().Contains(searchText))
                    .ToList();
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var filter = ((ComboBoxItem)FilterComboBox.SelectedItem).Content.ToString();

            switch (filter)
            {
                case "Новые":
                    RequestsDataGrid.ItemsSource = _filteredRequests.Where(r => r.Status == "Новая").ToList();
                    break;
                case "В работе":
                    RequestsDataGrid.ItemsSource = _filteredRequests.Where(r => r.Status == "В работе").ToList();
                    break;
                case "Выполненные":
                    RequestsDataGrid.ItemsSource = _filteredRequests.Where(r => r.Status == "Выполнена").ToList();
                    break;
                default:
                    RequestsDataGrid.ItemsSource = _filteredRequests;
                    break;
            }
        }

        private void ShowMessage(string message, string title, MessageBoxImage icon)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, icon);
        }
    }
}