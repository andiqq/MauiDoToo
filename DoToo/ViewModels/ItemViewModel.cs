namespace DoToo.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Repositories;

public partial class ItemViewModel : ViewModel
{
    private readonly ITodoItemRepository repository;

    [ObservableProperty] private TodoItem item;

    public ItemViewModel(ITodoItemRepository repository)
    {
        this.repository = repository;
        Item = new TodoItem() { Due = DateTime.Now.AddDays(1) };
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        await repository.AddOrUpdateAsync(Item);
        await Navigation.PopAsync();
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        await repository.DeleteAsync(Item);
        await Navigation.PopAsync();
    }

}
