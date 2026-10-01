using System;
using System.Collections.Generic;

namespace EnglishFlashcard3D.Models
{
    public class TopicDeck
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Level { get; set; } = "B1";
        public List<EnglishFlashcard> Cards { get; set; } = new List<EnglishFlashcard>();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
