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
    response.EnsureSuccessStatusCode();

    List<Todo>? todos = await response.Content.ReadFromJsonAsync<List<Todo>>();
    if (todos is null || todos.Count == 0)
    {
        Console.WriteLine("У пользователя нет задач.");
        return;
    }

    Console.WriteLine($"Задачи пользователя {userId}:");
    foreach (var t in todos) PrintTodo(t);
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
        case "1": await ShowTodosAsync(); break;
        default: Console.WriteLine("Неверный пункт меню."); break;
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