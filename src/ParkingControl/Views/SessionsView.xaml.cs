using System.Windows;
using System.Windows.Controls;
using ParkingControl.Models;
using ParkingControl.Services;

namespace ParkingControl.Views;

public partial class SessionsView : UserControl
{
    private readonly ParkingService _service = new();

    public SessionsView()
    {
        InitializeComponent();
        LoadLookups();
        Load();
    }

    private void LoadLookups()
    {
        try
        {
            VehicleCombo.ItemsSource = _service.SearchVehicles(null);
            SpaceCombo.ItemsSource = _service.GetFreeSpaces();
            TariffCombo.ItemsSource = _service.GetTariffs();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка");
        }
    }

    private void Load()
    {
        try
        {
            var list = _service.SearchSessions(SearchBox.Text, OnlyActiveCheck.IsChecked == true);
            Grid.ItemsSource = list;
            EmptyMessage.Visibility = list.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка");
        }
    }

    private void Search_Click(object sender, RoutedEventArgs e) => Load();

    private void Filter_Changed(object sender, RoutedEventArgs e) => Load();

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (VehicleCombo.SelectedValue is not int vehicleId)
                throw new ArgumentException("Выберите автомобиль.");
            if (SpaceCombo.SelectedValue is not int spaceId)
                throw new ArgumentException("Выберите свободное место.");
            if (TariffCombo.SelectedValue is not int tariffId)
                throw new ArgumentException("Выберите тариф.");
            _service.OpenSession(vehicleId, spaceId, tariffId);
            LoadLookups();
            Load();
            MessageBox.Show("Сессия открыта.", "Успех");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка");
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Grid.SelectedItem is not ParkingSession s)
                throw new InvalidOperationException("Выберите сессию.");
            if (!s.IsActive)
                throw new InvalidOperationException("Сессия уже закрыта.");
            var confirm = MessageBox.Show(
                $"Закрыть сессию для {s.PlateNumber} и рассчитать стоимость?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;
            var cost = _service.CloseSession(s.SessionId);
            LoadLookups();
            Load();
            MessageBox.Show($"Сессия закрыта. Стоимость: {cost:C}", "Успех");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка");
        }
    }
}