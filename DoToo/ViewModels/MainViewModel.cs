namespace DoToo.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Repositories;
using Views;
using System.Collections.ObjectModel;

public partial class MainViewModel : ViewModel
{
    private readonly ITodoItemRepository _repository;
    private readonly IServiceProvider _services;

    [ObservableProperty] private ObservableCollection<TodoItemViewModel>? _items;
    [ObservableProperty] private bool _showAll;

    [RelayCommand]
    public async Task ToggleFilterAsync()
    {
        ShowAll = !ShowAll;
        await LoadDataAsync();
    }

    public MainViewModel(ITodoItemRepository repository, IServiceProvider services)
    {
        this._repository = repository;
        repository.OnItemAdded += (_, item) => Items!.Add(CreateTodoItemViewModel(item));
        repository.OnItemUpdated += (_, _) => Task.Run(async () => await LoadDataAsync());
        repository.OnItemDeleted += (_, item) => Task.Run(async () => await LoadDataAsync());

        this._services = services;
        Task.Run(async () => await LoadDataAsync());
    }

    private async Task LoadDataAsync()
    {
        var items = await _repository.GetItemsAsync(); 
        if (!ShowAll)
        {
            items = items.Where(x => x.Completed == false).ToList();
        }

        var itemViewModels = items.Select(i => CreateTodoItemViewModel(i));
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
        if (sender is TodoItemViewModel item)
        {
            if (!ShowAll && item.Item.Completed)
            {
                Items!.Remove(item);
            }

            Task.Run(async () => await _repository.UpdateItemAsync(item.Item));
        }
    }

    [RelayCommand]
    public async Task AddItemAsync() => await Navigation.PushAsync(_services.GetRequiredService<ItemView>());

    [RelayCommand]
    public async Task DeleteItemAsync() => await Navigation.PushAsync(_services.GetRequiredService<ItemView>());

    [ObservableProperty]
    TodoItemViewModel? _selectedItem;

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
        var itemView = _services.GetRequiredService<ItemView>();
        var vm = itemView.BindingContext as ItemViewModel;
        vm!.Item = item!.Item;
        itemView.Title = "Edit todo item";

        await Navigation.PushAsync(itemView);
    }
}
