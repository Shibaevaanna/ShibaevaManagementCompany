using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity; // Добавьте эту директиву для Include()
using ShibaevaManagementCompany; // Добавьте эту директиву для контекста и моделей

namespace ShibaevaManagementCompany.Pages
{
    public partial class ApartmentsPage : Page
    {
        private int? filteredBuildingId = null;

        public ApartmentsPage()
        {
            InitializeComponent();
            Loaded += ApartmentsPage_Loaded;
        }

        public void FilterByBuilding(int buildingId)
        {
            filteredBuildingId = buildingId;
        }

        private void ApartmentsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBuildingsFilter();
            LoadApartments();
            UpdateFilterInfo();
        }

        private void LoadBuildingsFilter()
        {
            try
            {
                using (var db = new ShibaevaManagementCompanyEntities()) // Используйте правильное имя
                {
                    var buildings = db.Buildings
                        .OrderBy(b => b.Address)
                        .ToList();

                    foreach (var building in buildings)
                    {
                        cbBuildingFilter.Items.Add(building);
                    }

                    if (filteredBuildingId.HasValue)
                    {
                        foreach (Buildings item in cbBuildingFilter.Items) // Buildings, а не Building
                        {
                            if (item.BuildingID == filteredBuildingId.Value)
                            {
                                cbBuildingFilter.SelectedItem = item;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке фильтра домов: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadApartments()
        {
            try
            {
                using (var db = new ShibaevaManagementCompanyEntities()) // Используйте правильное имя
                {
                    IQueryable<Apartments> query = db.Apartments.Include(a => a.Buildings); // Исправлено Include

                    if (filteredBuildingId.HasValue)
                    {
                        query = query.Where(a => a.BuildingID == filteredBuildingId.Value);
                    }

                    var apartments = query
                        .OrderBy(a => a.Buildings.Address)
                        .ThenBy(a => a.ApartmentNumber)
                        .ToList();

                    dgApartments.ItemsSource = apartments;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке квартир: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void UpdateFilterInfo()
        {
            if (filteredBuildingId.HasValue)
            {
                using (var db = new ShibaevaManagementCompanyEntities()) // Используйте правильное имя
                {
                    var building = db.Buildings.Find(filteredBuildingId.Value);
                    if (building != null)
                    {
                        txtFilterInfo.Text = $"Отображены квартиры дома: {building.Address}";
                    }
                }
            }
            else
            {
                txtFilterInfo.Text = "Отображены все квартиры";
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgApartments.ItemsSource is System.Collections.IList items)
            {
                var filtered = items.Cast<Apartments>() // Apartments, а не Apartment
                    .Where(a => a.OwnerName.ToLower().Contains(txtSearch.Text.ToLower()) ||
                               a.PhoneNumber.Contains(txtSearch.Text) ||
                               a.ApartmentNumber.ToString().Contains(txtSearch.Text) ||
                               a.Buildings.Address.ToLower().Contains(txtSearch.Text.ToLower()))
                    .ToList();

                dgApartments.ItemsSource = filtered;

                if (string.IsNullOrEmpty(txtSearch.Text))
                {
                    LoadApartments();
                }
            }
        }

        private void CbBuildingFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbBuildingFilter.SelectedItem is Buildings selectedBuilding) // Buildings, а не Building
            {
                filteredBuildingId = selectedBuilding.BuildingID;
            }
            else
            {
                filteredBuildingId = null;
            }

            LoadApartments();
            UpdateFilterInfo();
        }

        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            filteredBuildingId = null;
            cbBuildingFilter.SelectedIndex = 0;
            LoadApartments();
            UpdateFilterInfo();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadApartments();
            txtSearch.Clear();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BuildingsPage());
        }
    }
}