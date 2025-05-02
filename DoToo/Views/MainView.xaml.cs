namespace DoToo.Views;

using ViewModels;

public partial class MainView
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        viewModel.Navigation = Navigation;
        BindingContext = viewModel;
        ItemsListView.ItemSelected += (_, _) => ItemsListView.SelectedItem = null;
    }
}