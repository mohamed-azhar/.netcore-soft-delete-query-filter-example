using System;

namespace SoftDeleteQueryFilterExample.Models
{
    public class Book : CommonAttributes
    {
        public Book(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
