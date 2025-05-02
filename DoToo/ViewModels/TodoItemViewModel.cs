namespace DoToo.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

public partial class TodoItemViewModel : ViewModel
{
    public TodoItemViewModel(TodoItem item) => Item = item;

    public event EventHandler ItemStatusChanged = null!;

    [ObservableProperty] private TodoItem item;

    public string StatusText => Item.Completed ? "Reactivate" : "Completed";

    [RelayCommand]
    private void ToggleCompleted()
    {
        Item.Completed = !Item.Completed;
        ItemStatusChanged?.Invoke(this, EventArgs.Empty);
    }
}
