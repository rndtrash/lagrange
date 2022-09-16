using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Algo.Views;

public partial class FromFunctionWindow : Window
{
    public FromFunctionWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}