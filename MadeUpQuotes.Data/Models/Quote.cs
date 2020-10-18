using System;
using MadeUpQuotes.Data.Models.Enums;

namespace MadeUpQuotes.Data.Models
{
    public class Quote
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Text { get; set; }

        public Visibility Visibility { get; set; }
    }
}