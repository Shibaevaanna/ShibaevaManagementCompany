using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShibaevaManagementCompany.Pages
{
    public partial class BuildingsPage : Page
    {
        public BuildingsPage()
        {
            InitializeComponent();
            Loaded += BuildingsPage_Loaded;
        }

        private void BuildingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBuildings();
        }

        private void LoadBuildings()
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var buildings = db.Buildings
                        .OrderBy(b => b.Address)
                        .ToList();

                    dgBuildings.ItemsSource = buildings;

                    txtTotalBuildings.Text = $"Всего домов: {buildings.Count}";
                    txtTotalApartments.Text = $"Всего квартир: {buildings.Sum(b => b.ApartmentsCount ?? 0)}";
                    txtTotalArea.Text = $"Общая площадь: {buildings.Sum(b => b.TotalArea ?? 0):N2} м²";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgBuildings.ItemsSource is System.Collections.IList items)
            {
                var filtered = items.Cast<Building>()
                    .Where(b => b.Address.ToLower().Contains(txtSearch.Text.ToLower()))
                    .ToList();

                dgBuildings.ItemsSource = filtered;

                if (string.IsNullOrEmpty(txtSearch.Text))
                {
                    LoadBuildings();
                }
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadBuildings();
            txtSearch.Clear();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage());
        }
    }
}