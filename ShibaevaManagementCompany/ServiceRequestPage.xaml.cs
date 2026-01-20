using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShibaevaManagementCompany.Pages
{
    public partial class ServiceRequestsPage : Page
    {
        private int? filteredApartmentId = null;

        public ServiceRequestsPage()
        {
            InitializeComponent();
            Loaded += ServiceRequestsPage_Loaded;
        }

        public void FilterByApartment(int apartmentId)
        {
            filteredApartmentId = apartmentId;
        }

        private void ServiceRequestsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRequests();
            UpdateFilterInfo();
        }

        private void LoadRequests()
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    IQueryable<ServiceRequest> query = db.ServiceRequests
                        .Include("Apartments")
                        .Include("Apartments.Buildings")
                        .Include("Employees");

                    if (filteredApartmentId.HasValue)
                    {
                        query = query.Where(r => r.ApartmentID == filteredApartmentId.Value);
                    }

                    var requests = query
                        .OrderByDescending(r => r.RequestDate)
                        .ToList();

                    dgRequests.ItemsSource = requests;
                    UpdateButtonStates();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заявок: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void UpdateFilterInfo()
        {
            if (filteredApartmentId.HasValue)
            {
                using (var db = new DatabaseContext())
                {
                    var apartment = db.Apartments
                        .Include("Buildings")
                        .FirstOrDefault(a => a.ApartmentID == filteredApartmentId.Value);

                    if (apartment != null)
                    {
                        txtFilterInfo.Text = $"Заявки квартиры: {apartment.Buildings.Address}, кв. {apartment.ApartmentNumber}";
                    }
                }
            }
            else
            {
                txtFilterInfo.Text = "Все заявки";
            }
        }

        private void ApplyFilters()
        {
            if (dgRequests.ItemsSource is System.Collections.IList items)
            {
                var filtered = items.Cast<ServiceRequest>()
                    .Where(r => {
                        // Фильтр по статусу
                        string selectedStatus = (cbStatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
                        if (selectedStatus != "Все" && !string.IsNullOrEmpty(selectedStatus))
                        {
                            if (r.Status != selectedStatus)
                                return false;
                        }

                        // Фильтр по адресу
                        string addressFilter = txtAddressFilter.Text.Trim();
                        if (!string.IsNullOrEmpty(addressFilter))
                        {
                            if (!r.Apartments.Buildings.Address.ToLower().Contains(addressFilter.ToLower()))
                                return false;
                        }

                        return true;
                    })
                    .ToList();

                dgRequests.ItemsSource = filtered;
            }
        }

        private void CbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void TxtAddressFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void BtnToday_Click(object sender, RoutedEventArgs e)
        {
            FilterByDate(DateTime.Today);
        }

        private void BtnWeek_Click(object sender, RoutedEventArgs e)
        {
            FilterByDate(DateTime.Today.AddDays(-7));
        }

        private void BtnMonth_Click(object sender, RoutedEventArgs e)
        {
            FilterByDate(DateTime.Today.AddMonths(-1));
        }

        private void BtnOnlyNew_Click(object sender, RoutedEventArgs e)
        {
            cbStatusFilter.SelectedItem = cbStatusFilter.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == "Новая");
        }

        private void FilterByDate(DateTime fromDate)
        {
            if (dgRequests.ItemsSource is System.Collections.IList items)
            {
                var filtered = items.Cast<ServiceRequest>()
                    .Where(r => r.RequestDate >= fromDate)
                    .ToList();

                dgRequests.ItemsSource = filtered;
            }
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = dgRequests.SelectedItem != null;
            btnEdit.IsEnabled = hasSelection;
            btnDelete.IsEnabled = hasSelection;
        }

        private void DgRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonStates();
        }

        private void DgRequests_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditSelectedRequest();
        }

        private void EditSelectedRequest()
        {
            if (dgRequests.SelectedItem is ServiceRequest selectedRequest)
            {
                var editPage = new RequestEditPage(selectedRequest);
                editPage.RequestSaved += EditPage_RequestSaved;
                NavigationService?.Navigate(editPage);
            }
        }

        private void EditPage_RequestSaved(object sender, EventArgs e)
        {
            LoadRequests();
        }

        private void BtnAddRequest_Click(object sender, RoutedEventArgs e)
        {
            var editPage = new RequestEditPage();
            editPage.RequestSaved += EditPage_RequestSaved;
            NavigationService?.Navigate(editPage);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedRequest();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequests.SelectedItem is ServiceRequest selectedRequest)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить заявку №{selectedRequest.RequestID}?\nЭто действие нельзя отменить.",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var db = new DatabaseContext())
                        {
                            var requestToDelete = db.ServiceRequests.Find(selectedRequest.RequestID);
                            if (requestToDelete != null)
                            {
                                db.ServiceRequests.Remove(requestToDelete);
                                db.SaveChanges();

                                MessageBox.Show("Заявка успешно удалена.",
                                    "Успех",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                                LoadRequests();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении заявки: {ex.Message}",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BtnViewHistory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RequestHistoryPage());
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
            txtAddressFilter.Clear();
            cbStatusFilter.SelectedIndex = 0;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage());
        }
    }
}
