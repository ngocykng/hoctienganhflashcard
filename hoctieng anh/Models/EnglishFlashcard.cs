using System;

namespace EnglishFlashcard3D.Models
{
    public class EnglishFlashcard
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Mặt trước (Front side)
        public string Word { get; set; } = string.Empty;
        public string Phonetic { get; set; } = string.Empty;
        public string PartOfSpeech { get; set; } = string.Empty;

        // Mặt sau (Back side)
        public string MeaningVi { get; set; } = string.Empty;
        public string ExampleEn { get; set; } = string.Empty;
        public string ExampleVi { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;

        // Trạng thái học tập
        public int ReviewCount { get; set; } = 0;
        public bool IsMastered { get; set; } = false;
        public DateTime? LastReviewedAt { get; set; }
    }
}
