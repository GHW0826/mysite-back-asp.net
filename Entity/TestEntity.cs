using System.Collections.Concurrent;

namespace mysite_back_asp.net.Entity
{
    public class TestEntity : Entity
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
