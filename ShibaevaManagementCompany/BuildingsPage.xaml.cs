using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ShibaevaManagementCompany; // Добавьте эту директиву для доступа к контексту и моделям

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
                using (var db = new ShibaevaManagementCompanyEntities()) // Используйте правильное имя контекста
                {
                    var buildings = db.Buildings
                        .OrderBy(b => b.Address)
                        .ToList();

                    dgBuildings.ItemsSource = buildings;

                    // Обновление статистики
                    UpdateStatistics(buildings);
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

        private void UpdateStatistics(System.Collections.Generic.List<Buildings> buildings) // Buildings вместо Building
        {
            try
            {
                using (var db = new ShibaevaManagementCompanyEntities())
                {
                    // Если в модели Buildings нет ApartmentsCount и TotalArea, вычисляем
                    int totalApartments = 0;
                    decimal totalArea = 0;

                    foreach (var building in buildings)
                    {
                        // Получаем квартиры для каждого дома
                        var apartments = db.Apartments
                            .Where(a => a.BuildingID == building.BuildingID)
                            .ToList();

                        totalApartments += apartments.Count;

                        // Если есть поле Area в Apartments
                        // totalArea += apartments.Sum(a => a.Area ?? 0);
                    }

                    txtTotalBuildings.Text = $"Всего домов: {buildings.Count}";
                    txtTotalApartments.Text = $"Всего квартир: {totalApartments}";
                    txtTotalArea.Text = $"Общая площадь: {totalArea:N2} м²";
                }
            }
            catch (Exception ex)
            {
                // Если не удалось вычислить статистику, показываем базовую информацию
                txtTotalBuildings.Text = $"Всего домов: {buildings.Count}";
                txtTotalApartments.Text = "Всего квартир: (не удалось загрузить)";
                txtTotalArea.Text = "Общая площадь: (не удалось загрузить)";
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgBuildings.ItemsSource is System.Collections.IList items)
            {
                var filtered = items.Cast<Buildings>() // Buildings вместо Building
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

        // Дополнительные методы (если нужны)

        private void BtnViewApartments_Click(object sender, RoutedEventArgs e)
        {
            if (dgBuildings.SelectedItem is Buildings selectedBuilding) // Buildings вместо Building
            {
                var apartmentsPage = new ApartmentsPage();
                apartmentsPage.FilterByBuilding(selectedBuilding.BuildingID);
                NavigationService?.Navigate(apartmentsPage);
            }
            else
            {
                MessageBox.Show("Выберите дом для просмотра квартир",
                    "Информация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void BtnAddBuilding_Click(object sender, RoutedEventArgs e)
        {
            // Код для добавления нового дома
            // var addWindow = new AddBuildingWindow();
            // if (addWindow.ShowDialog() == true)
            // {
            //     LoadBuildings();
            // }
        }

        private void BtnEditBuilding_Click(object sender, RoutedEventArgs e)
        {
            if (dgBuildings.SelectedItem is Buildings selectedBuilding) // Buildings вместо Building
            {
                // Код для редактирования дома
                // var editWindow = new EditBuildingWindow(selectedBuilding);
                // if (editWindow.ShowDialog() == true)
                // {
                //     LoadBuildings();
                // }
            }
            else
            {
                MessageBox.Show("Выберите дом для редактирования",
                    "Информация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}