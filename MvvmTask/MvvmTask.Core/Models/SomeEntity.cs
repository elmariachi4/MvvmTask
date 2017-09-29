using SQLite.Net.Attributes;
using System;

namespace MvvmTask.Core.Models
{
    public class SomeEntity
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime Updated { get; set; }
    }
}
