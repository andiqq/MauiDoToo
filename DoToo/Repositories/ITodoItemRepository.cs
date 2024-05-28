namespace DoToo.Repositories;

using DoToo.Models;

public interface ITodoItemRepository
{
    event EventHandler<TodoItem> OnItemAdded; 
    event EventHandler<TodoItem> OnItemUpdated;
    event EventHandler<TodoItem> OnItemDeleted;

    Task<List<TodoItem>> GetItemsAsync(); 
    Task AddItemAsync(TodoItem item); 
    Task UpdateItemAsync(TodoItem item); 
    Task AddOrUpdateAsync(TodoItem item);
    Task DeleteAsync(TodoItem item);
}
