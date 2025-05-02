namespace DoToo.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Repositories;
using Views;
using System.Collections.ObjectModel;

public partial class MainViewModel : ViewModel
{
    private readonly ITodoItemRepository repository;
    private readonly IServiceProvider services;

    [ObservableProperty] private ObservableCollection<TodoItemViewModel>? items;
    [ObservableProperty] private bool showAll;

    [RelayCommand]
    private async Task ToggleFilterAsync()
    {
        ShowAll = !ShowAll;
        await LoadDataAsync();
    }
    
    public MainViewModel(ITodoItemRepository repository, IServiceProvider services)
    {
        this.repository = repository;
        repository.OnItemAdded += (_, item) => Items!.Add(CreateTodoItemViewModel(item));
        repository.OnItemUpdated += (_, _) => Task.Run(async () => await LoadDataAsync());
        repository.OnItemDeleted += (_, _) => Task.Run(async () => await LoadDataAsync());

        this.services = services;
        Task.Run(async () => await LoadDataAsync());
    }

    private async Task LoadDataAsync()
    {
        var items = await repository.GetItemsAsync(); 
        if (!ShowAll)
        {
            items = items.Where(x => x.Completed == false).ToList();
        }

        var itemViewModels = items.Select(CreateTodoItemViewModel);
        Items = new ObservableCollection<TodoItemViewModel>(itemViewModels);
    }

    private TodoItemViewModel CreateTodoItemViewModel(TodoItem item)
    {
        var itemViewModel = new TodoItemViewModel(item);
        itemViewModel.ItemStatusChanged += ItemStatusChanged!;
        return itemViewModel;
    }

    private void ItemStatusChanged(object sender, EventArgs e)
    {
        if (sender is not TodoItemViewModel item) return;
        if (!ShowAll && item.Item.Completed)
        {
            Items!.Remove(item);
        }

        Task.Run(async () => await repository.UpdateItemAsync(item.Item));
    }

    [RelayCommand]
    private async Task AddItemAsync() => await Navigation.PushAsync(services.GetRequiredService<ItemView>());

    [RelayCommand]
    private async Task DeleteItemAsync() => await Navigation.PushAsync(services.GetRequiredService<ItemView>());

    [ObservableProperty] private TodoItemViewModel? selectedItem;

    partial void OnSelectedItemChanging(TodoItemViewModel? value)
    {
        if (value == null)
        {
            return;
        }

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await NavigateToItemAsync(value);
        });
    }

    private async Task NavigateToItemAsync(TodoItemViewModel? item)
    {
        var itemView = services.GetRequiredService<ItemView>();
        var vm = itemView.BindingContext as ItemViewModel;
        vm!.Item = item!.Item;
        itemView.Title = "Edit todo item";

        await Navigation.PushAsync(itemView);
    }
}
