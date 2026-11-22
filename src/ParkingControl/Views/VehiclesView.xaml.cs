using System.Windows;
using System.Windows.Controls;
using ParkingControl.Models;
using ParkingControl.Services;

namespace ParkingControl.Views;

public partial class VehiclesView : UserControl
{
    private readonly ParkingService _service = new();

    public VehiclesView()
    {
        InitializeComponent();
        LoadClients();
        Load();
    }

    private void LoadClients()
    {
        try
        {
            ClientCombo.ItemsSource = _service.SearchClients(null);
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
            Grid.ItemsSource = _service.SearchVehicles(SearchBox.Text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка");
        }
    }

    private void Search_Click(object sender, RoutedEventArgs e) => Load();

    private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Grid.SelectedItem is Vehicle v)
        {
            ClientCombo.SelectedValue = v.ClientId;
            PlateBox.Text = v.PlateNumber;
            ModelBox.Text = v.Model;
        }
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ClientCombo.SelectedValue is not int clientId)
                throw new ArgumentException("Выберите клиента.");
            var vehicle = new Vehicle
            {
                ClientId = clientId,
                PlateNumber = PlateBox.Text,
                Model = ModelBox.Text
            };
            _service.SaveVehicle(vehicle);
            Load();
            MessageBox.Show("Автомобиль добавлен.", "Успех");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка");
        }
    }

    private void Update_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Grid.SelectedItem is not Vehicle v)
                throw new InvalidOperationException("Выберите автомобиль.");
            if (ClientCombo.SelectedValue is not int clientId)
                throw new ArgumentException("Выберите клиента.");
            v.ClientId = clientId;
            v.PlateNumber = PlateBox.Text;
            v.Model = ModelBox.Text;
            _service.SaveVehicle(v);
            Load();
            MessageBox.Show("Автомобиль обновлён.", "Успех");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка");
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Grid.SelectedItem is not Vehicle v)
                throw new InvalidOperationException("Выберите автомобиль.");
            var confirm = MessageBox.Show(
                $"Удалить автомобиль {v.PlateNumber}?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;
            _service.DeleteVehicle(v.VehicleId);
            Load();
            MessageBox.Show("Автомобиль удалён.", "Успех");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка");
        }
    }
}