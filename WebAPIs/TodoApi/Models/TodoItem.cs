using Shared.Abstracts;
using Shared.Attributes;

namespace TodoApi.Models
{
    [CollectionName("TodoItem")]
    public class TodoItem : Entity
    {
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
