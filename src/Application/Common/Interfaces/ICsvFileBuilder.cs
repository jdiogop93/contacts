using Contacts.Application.TodoLists.Queries.ExportTodos;

namespace Contacts.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
