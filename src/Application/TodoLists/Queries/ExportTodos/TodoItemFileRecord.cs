using Contacts.Application.Common.Mappings;
using Contacts.Domain.Entities;

namespace Contacts.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
