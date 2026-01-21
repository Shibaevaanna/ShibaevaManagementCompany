using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ShibaevaManagementCompany
{
    public partial class RequestAddEditWindow : Window
    {
        private ServiceRequestsPage.ServiceRequest _request;
        private bool _isEditMode;

        public RequestAddEditWindow(ServiceRequestsPage.ServiceRequest request)
        {
            InitializeComponent();
            _request = request;
            _isEditMode = request != null;

            if (_isEditMode)
            {
                WindowTitle.Text = "Редактирование заявки";
                LoadRequestData();
            }

            // Устанавливаем текущую дату по умолчанию для даты выполнения
            CompletionDatePicker.SelectedDate = DateTime.Now.AddDays(3);
        }

        private void LoadRequestData()
        {
            if (_request == null) return;

            // Заполняем поля данными из выбранной заявки
            foreach (ComboBoxItem item in AddressComboBox.Items)
            {
                if (item.Content.ToString() == _request.Address)
                {
                    AddressComboBox.SelectedItem = item;
                    break;
                }
            }

            ApplicantTextBox.Text = _request.ApplicantName;
            PhoneTextBox.Text = _request.Phone;
            DescriptionTextBox.Text = _request.Description;

            foreach (ComboBoxItem item in AssigneeComboBox.Items)
            {
                if (item.Content.ToString() == _request.Assignee)
                {
                    AssigneeComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == _request.Status)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (!ValidateInput())
                return;

            try
            {
                // Имитация сохранения в базу данных
                if (_isEditMode)
                {
                    // Обновление существующей заявки
                    MessageBox.Show($"Заявка #{_request.RequestId} обновлена успешно!\n\n" +
                                    $"Адрес: {AddressComboBox.Text}\n" +
                                    $"Заявитель: {ApplicantTextBox.Text}\n" +
                                    $"Статус: {StatusComboBox.Text}",
                                    "Успешное сохранение",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Создание новой заявки
                    MessageBox.Show($"Новая заявка создана успешно!\n\n" +
                                    $"Адрес: {AddressComboBox.Text}\n" +
                                    $"Заявитель: {ApplicantTextBox.Text}\n" +
                                    $"Исполнитель: {AssigneeComboBox.Text}\n" +
                                    $"Статус: {StatusComboBox.Text}\n" +
                                    $"Приоритет: {PriorityComboBox.Text}",
                                    "Успешное сохранение",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            // Проверка адреса
            if (AddressComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите адрес", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                AddressComboBox.Focus();
                return false;
            }

            // Проверка ФИО
            if (string.IsNullOrWhiteSpace(ApplicantTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите ФИО заявителя", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                ApplicantTextBox.Focus();
                return false;
            }

            // Проверка телефона
            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text) || PhoneTextBox.Text == "+7(XXX)XXX-XX-XX")
            {
                MessageBox.Show("Пожалуйста, введите контактный телефон", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                PhoneTextBox.Focus();
                return false;
            }

            // Проверка формата телефона
            var phoneRegex = new Regex(@"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$");
            if (!phoneRegex.IsMatch(PhoneTextBox.Text))
            {
                MessageBox.Show("Некорректный формат телефона. Используйте формат: +7(XXX)XXX-XX-XX",
                    "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                PhoneTextBox.Focus();
                return false;
            }

            // Проверка описания проблемы
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите описание проблемы", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                DescriptionTextBox.Focus();
                return false;
            }

            // Проверка статуса
            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите статус заявки", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                StatusComboBox.Focus();
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Все несохраненные изменения будут потеряны. Продолжить?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DialogResult = false;
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Автоматически выбираем первый элемент в комбобоксах, если они пусты
            if (AddressComboBox.SelectedItem == null && AddressComboBox.Items.Count > 0)
                AddressComboBox.SelectedIndex = 0;

            if (AssigneeComboBox.SelectedItem == null && AssigneeComboBox.Items.Count > 0)
                AssigneeComboBox.SelectedIndex = 0;

            if (StatusComboBox.SelectedItem == null && StatusComboBox.Items.Count > 0)
                StatusComboBox.SelectedIndex = 0;
        }
    }
}
