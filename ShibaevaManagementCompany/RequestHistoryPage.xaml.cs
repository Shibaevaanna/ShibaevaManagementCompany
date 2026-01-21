using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShibaevaManagementCompany
{
    public partial class RequestHistoryPage : Page
    {
        public class HistoryRecord
        {
            public int RequestId { get; set; }
            public string Address { get; set; }
            public string Employee { get; set; }
            public string Status { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int Duration { get; set; }
            public int? Rating { get; set; }
            public string Comment { get; set; }
        }

        private List<HistoryRecord> _allHistory;
        private List<HistoryRecord> _filteredHistory;

        public RequestHistoryPage()
        {
            InitializeComponent();
            InitializeDates();
            LoadHistoryData();
        }

        private void InitializeDates()
        {
            // Устанавливаем даты по умолчанию (последние 30 дней)
            EndDatePicker.SelectedDate = DateTime.Now;
            StartDatePicker.SelectedDate = DateTime.Now.AddDays(-30);
        }

        private void LoadHistoryData()
        {
            // Имитация данных из базы данных
            _allHistory = new List<HistoryRecord>
            {
                new HistoryRecord { RequestId = 1001, Address = "ул. Ленина, д. 10, кв. 5", Employee = "Смирнов Петр Иванович", Status = "Выполнена", StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(-8), Duration = 2, Rating = 5, Comment = "Работа выполнена качественно и в срок" },
                new HistoryRecord { RequestId = 1002, Address = "ул. Пушкина, д. 25, кв. 12", Employee = "Кузнецов Алексей Владимирович", Status = "Выполнена", StartDate = DateTime.Now.AddDays(-15), EndDate = DateTime.Now.AddDays(-13), Duration = 2, Rating = 4, Comment = "Небольшая задержка из-за отсутствия деталей" },
                new HistoryRecord { RequestId = 1003, Address = "пр. Мира, д. 15, кв. 7", Employee = "Николаев Сергей Михайлович", Status = "Выполнена", StartDate = DateTime.Now.AddDays(-20), EndDate = DateTime.Now.AddDays(-17), Duration = 3, Rating = 5, Comment = "Отличная работа" },
                new HistoryRecord { RequestId = 1004, Address = "ул. Гагарина, д. 8, кв. 3", Employee = "Васильев Игорь Константинович", Status = "Выполнена", StartDate = DateTime.Now.AddDays(-5), EndDate = DateTime.Now.AddDays(-3), Duration = 2, Rating = 3, Comment = "Пришлось переделывать часть работы" },
                new HistoryRecord { RequestId = 1005, Address = "ул. Советская, д. 30, кв. 9", Employee = "Петров Андрей Сергеевич", Status = "Отменена", StartDate = DateTime.Now.AddDays(-7), EndDate = null, Duration = 0, Rating = null, Comment = "Клиент отменил заявку" },
                new HistoryRecord { RequestId = 1006, Address = "ул. Ленина, д. 10, кв. 8", Employee = "Иванов Михаил Петрович", Status = "Выполнена", StartDate = DateTime.Now.AddDays(-25), EndDate = DateTime.Now.AddDays(-23), Duration = 2, Rating = 5, Comment = "Быстрое и качественное выполнение" },
                new HistoryRecord { RequestId = 1007, Address = "ул. Пушкина, д. 25, кв. 15", Employee = "Сидоров Владимир Владимирович", Status = "Выполнена", StartDate = DateTime.Now.AddDays(-12), EndDate = DateTime.Now.AddDays(-10), Duration = 2, Rating = 4, Comment = "Хорошая работа" },
                new HistoryRecord { RequestId = 1008, Address = "ул. Ленина, д. 10, кв. 3", Employee = "Смирнов Петр Иванович", Status = "Выполнена", StartDate = DateTime.Now.AddDays(-8), EndDate = DateTime.Now.AddDays(-6), Duration = 2, Rating = 5, Comment = "Профессионально выполнено" },
                new HistoryRecord { RequestId = 1009, Address = "пр. Мира, д. 15, кв. 12", Employee = "Кузнецов Алексей Владимирович", Status = "Выполнена", StartDate = DateTime.Now.AddDays(-18), EndDate = DateTime.Now.AddDays(-16), Duration = 2, Rating = 4, Comment = "Работа выполнена согласно требованиям" },
                new HistoryRecord { RequestId = 1010, Address = "ул. Гагарина, д. 8, кв. 5", Employee = "Николаев Сергей Михайлович", Status = "Выполнена", StartDate = DateTime.Now.AddDays(-22), EndDate = DateTime.Now.AddDays(-20), Duration = 2, Rating = 5, Comment = "Отличное обслуживание" }
            };

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            _filteredHistory = new List<HistoryRecord>(_allHistory);

            // Фильтр по сотруднику
            var employeeFilter = ((ComboBoxItem)EmployeeFilterComboBox.SelectedItem)?.Content.ToString();
            if (employeeFilter != null && employeeFilter != "Все сотрудники")
            {
                _filteredHistory = _filteredHistory.Where(h => h.Employee == employeeFilter).ToList();
            }

            // Фильтр по адресу
            var addressFilter = ((ComboBoxItem)AddressFilterComboBox.SelectedItem)?.Content.ToString();
            if (addressFilter != null && addressFilter != "Все адреса")
            {
                _filteredHistory = _filteredHistory.Where(h => h.Address.Contains(addressFilter)).ToList();
            }

            // Фильтр по дате
            if (StartDatePicker.SelectedDate.HasValue)
            {
                _filteredHistory = _filteredHistory.Where(h => h.StartDate >= StartDatePicker.SelectedDate.Value).ToList();
            }

            if (EndDatePicker.SelectedDate.HasValue)
            {
                _filteredHistory = _filteredHistory.Where(h => h.StartDate <= EndDatePicker.SelectedDate.Value).ToList();
            }

            // Обновляем DataGrid
            HistoryDataGrid.ItemsSource = _filteredHistory;

            // Обновляем статистику
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            if (_filteredHistory == null || _filteredHistory.Count == 0)
            {
                StatsTextBlock.Text = "Нет данных для отображения";
                return;
            }

            var completedCount = _filteredHistory.Count(h => h.Status == "Выполнена");
            var canceledCount = _filteredHistory.Count(h => h.Status == "Отменена");
            var avgRating = _filteredHistory.Where(h => h.Rating.HasValue).Average(h => h.Rating.Value);
            var avgDuration = _filteredHistory.Where(h => h.Duration > 0).Average(h => h.Duration);

            StatsTextBlock.Text = $"Статистика: {_filteredHistory.Count} заявок, " +
                                  $"Выполнено: {completedCount}, " +
                                  $"Отменено: {canceledCount}, " +
                                  $"Средняя оценка: {avgRating:F1}, " +
                                  $"Средняя длительность: {avgDuration:F1} дней";
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void DatePicker_Changed(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, что начальная дата не позже конечной
            if (StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                if (StartDatePicker.SelectedDate.Value > EndDatePicker.SelectedDate.Value)
                {
                    MessageBox.Show("Дата начала не может быть позже даты окончания", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EndDatePicker.SelectedDate = StartDatePicker.SelectedDate.Value;
                }
            }

            ApplyFilters();
        }
    }
}
