using System.Text.Json.Serialization;
using System.Net.Http.Json;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

using var http = new HttpClient
{
    BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
    Timeout = TimeSpan.FromSeconds(15)
};

int userId = ReadInt("Введите ID пользователя: ");
await ShowTodosAsync();

async Task ShowTodosAsync()
{
    using var response = await http.GetAsync($"/todos?userId={userId}");
    if (!await CheckAsync(response)) return;

    List<Todo>? todos = await response.Content.ReadFromJsonAsync<List<Todo>>();
    if (todos is null || todos.Count == 0)
    {
        Console.WriteLine("У пользователя нет задач.");
        return;
    }

    Console.WriteLine($"Задачи пользователя {userId}:");
    foreach (var t in todos) PrintTodo(t);
}
async Task FindTodoAsync()
{
    int id = ReadInt("Введите ID задачи: ");
    using var response = await http.GetAsync($"/todos/{id}");
    if (!await CheckAsync(response)) return;

    Todo? todo = await response.Content.ReadFromJsonAsync<Todo>();
    if (todo is null) { Console.WriteLine("Пустой ответ сервера."); return; }
    PrintTodo(todo);
}
async Task CreateTodoAsync()
{
    Console.Write("Введите название задачи: ");
    string title = Console.ReadLine()?.Trim() ?? "";
    if (title.Length == 0)
    {
        Console.WriteLine("Название не может быть пустым.");
        return;
    }

    var newTodo = new Todo { UserId = userId, Title = title, Completed = false };

    using var response = await http.PostAsJsonAsync("/todos", newTodo);
    if (!await CheckAsync(response)) return;

    Todo? created = await response.Content.ReadFromJsonAsync<Todo>();
    Console.WriteLine("Задача создана (сервер имитирует создание):");
    if (created is not null) PrintTodo(created);
}
async Task UpdateStatusAsync()
{
    int id = ReadInt("Введите ID задачи: ");
    bool completed = ReadBool("Задача выполнена? (y/n): ");

    // В System.Net.Http.Json нет PatchAsJsonAsync, поэтому собираем запрос вручную
    using var request = new HttpRequestMessage(HttpMethod.Patch, $"/todos/{id}")
    {
        Content = JsonContent.Create(new { completed })
    };

    using var response = await http.SendAsync(request);
    if (!await CheckAsync(response)) return;

    Todo? updated = await response.Content.ReadFromJsonAsync<Todo>();
    Console.WriteLine("Статус изменён:");
    if (updated is not null) PrintTodo(updated);
}
async Task DeleteTodoAsync()
{
    int id = ReadInt("Введите ID задачи: ");
    using var response = await http.DeleteAsync($"/todos/{id}");
    if (!await CheckAsync(response)) return;

    Console.WriteLine($"Задача {id} удалена (сервер имитирует удаление).");
}
async Task<bool> CheckAsync(HttpResponseMessage response)
{
    if (response.IsSuccessStatusCode) return true;

    int code = (int)response.StatusCode;
    string message = code switch
    {
        404 => "Ресурс не найден.",
        400 => "Некорректный запрос.",
        >= 500 => "Ошибка на стороне сервера.",
        _ => "Запрос не выполнен."
    };
    Console.WriteLine($"Ошибка {code} ({response.ReasonPhrase}): {message}");
    await Task.CompletedTask;
    return false;
}
bool ReadBool(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string s = (Console.ReadLine() ?? "").Trim().ToLower();
        if (s is "y" or "д" or "да" or "1" or "true") return true;
        if (s is "n" or "н" or "нет" or "0" or "false") return false;
        Console.WriteLine("Введите y или n.");
    }
}
void PrintTodo(Todo t)
{
    string status = t.Completed ? "выполнена" : "не выполнена";
    Console.WriteLine($"[{t.Id}] {t.Title} — {status}");
}
while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Показать задачи пользователя");
    Console.WriteLine("2. Найти задачу по ID");
    Console.WriteLine("3. Создать задачу");
    Console.WriteLine("4. Изменить статус задачи");
    Console.WriteLine("5. Удалить задачу");
    Console.WriteLine("0. Выход");
    Console.WriteLine();
    Console.Write("Выберите действие: ");

    string? choice = Console.ReadLine()?.Trim();
    if (choice == "0") break;

    switch (choice)
    {
        case "1": await ShowTodosAsync(); 
            break;
        default: Console.WriteLine("Неверный пункт меню."); 
            break;
        case "2": await FindTodoAsync(); 
            break;
        case "3": await CreateTodoAsync(); 
            break;
        case "4": await UpdateStatusAsync();
            break;
        case "5":
            await DeleteTodoAsync();
            break;
    }
}
int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int value) && value > 0) return value;
        Console.WriteLine("Введите положительное целое число.");
    }
}
Console.WriteLine("Todo App");
public class Todo
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("completed")]
    public bool Completed { get; set; }
}