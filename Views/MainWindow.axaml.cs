using Algo.ViewModels;
using Avalonia.Controls;
using Avalonia.ReactiveUI;

namespace Algo.Views
{
    public partial class MainWindow : ReactiveWindow<Window>
    {
        public MainWindow()
        {
            InitializeComponent();

            PointsComboBox.Items = Utils.PointPlacementMethodsNames;
            PointsComboBox.SelectedIndex = 0;
        }

        private void PointsChangedEvent(object? sender, DataGridCellEditEndedEventArgs e) =>
            (DataContext as MainWindowViewModel)?.InitialPointsChanged();

        private void PointPlacementMethodChangedEvent(object? sender, SelectionChangedEventArgs e) =>
            (DataContext as MainWindowViewModel)?.PointPlacementMethodChanged(sender);
    }
}