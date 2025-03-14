namespace DoToo.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

public class ViewModel : ObservableObject
{
    public INavigation Navigation { get; set; } = null!;
}
