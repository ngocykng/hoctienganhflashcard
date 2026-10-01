using System;
using System.Collections.Generic;

namespace EnglishFlashcard3D.Models
{
    public class UserStudyProgress
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string DeckId { get; set; } = string.Empty;
        public int CardsReviewed { get; set; } = 0;
        public int CardsMastered { get; set; } = 0;
        public DateTime LastStudyDate { get; set; } = DateTime.Now;
        public Dictionary<string, int> CardReviewHistory { get; set; } = new Dictionary<string, int>();
    }
}
