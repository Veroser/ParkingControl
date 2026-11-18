using System.Windows;
using ParkingControl.Views;

namespace ParkingControl.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Host.Content = new SessionsView();
    }

    private void Clients_Click(object sender, RoutedEventArgs e) => Host.Content = new ClientsView();
    private void Vehicles_Click(object sender, RoutedEventArgs e) => Host.Content = new VehiclesView();
    private void Sessions_Click(object sender, RoutedEventArgs e) => Host.Content = new SessionsView();
    private void Report_Click(object sender, RoutedEventArgs e) => Host.Content = new ReportView();
}