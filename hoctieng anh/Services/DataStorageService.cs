using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using EnglishFlashcard3D.Models;

namespace EnglishFlashcard3D.Services
{
    public class DataStorageService
    {
        private readonly string _folderPath;
        private readonly string _filePath;

        public DataStorageService(string fileName = "decks.json")
        {
            _folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "EnglishFlashcard3D");

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            _filePath = Path.Combine(_folderPath, fileName);
        }

        public async Task<List<TopicDeck>> LoadDecksAsync()
        {
            if (!File.Exists(_filePath))
            {
                var defaultDecks = GetDefaultDecks();
                await SaveDecksAsync(defaultDecks);
                return defaultDecks;
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<TopicDeck>>(json) ?? new List<TopicDeck>();
        }

        public async Task SaveDecksAsync(List<TopicDeck> decks)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(decks, options);
            await File.WriteAllTextAsync(_filePath, json);
        }

        private List<TopicDeck> GetDefaultDecks()
        {
            return new List<TopicDeck>
            {
                new TopicDeck
                {
                    Name = "Oxford 3000 - Essential Words",
                    Description = "Những từ vựng quan trọng nhất tiếng Anh",
                    Level = "A1",
                    Cards = new List<EnglishFlashcard>
                    {
                        new EnglishFlashcard
                        {
                            Word = "Accomplish",
                            Phonetic = "/əˈkʌm.plɪʃ/",
                            PartOfSpeech = "Verb",
                            MeaningVi = "Hoàn thành, đạt được (mục tiêu)",
                            ExampleEn = "If we work together, we can accomplish anything.",
                            ExampleVi = "Nếu chúng ta làm việc cùng nhau, chúng ta có thể đạt được bất cứ điều gì."
                        },
                        new EnglishFlashcard
                        {
                            Word = "Perseverance",
                            Phonetic = "/ˌpɜː.sɪˈvɪə.rəns/",
                            PartOfSpeech = "Noun",
                            MeaningVi = "Sự kiên trì, lòng nhẫn nại vượt qua khó khăn",
                            ExampleEn = "Through hard work and perseverance, he reached his dream.",
                            ExampleVi = "Bằng sự chăm chỉ và lòng kiên trì, anh ấy đã chạm tới ước mơ của mình."
                        },
                        new EnglishFlashcard
                        {
                            Word = "Eloquent",
                            Phonetic = "/ˈel.ə.kwənt/",
                            PartOfSpeech = "Adjective",
                            MeaningVi = "Hùng biện, lưu loát, truyền cảm",
                            ExampleEn = "An eloquent speech moved everyone in the audience.",
                            ExampleVi = "Bài phát biểu hùng biện đã làm lay động tất cả khán giả."
                        },
                        new EnglishFlashcard
                        {
                            Word = "Resilience",
                            Phonetic = "/rɪˈzɪl.jəns/",
                            PartOfSpeech = "Noun",
                            MeaningVi = "Khả năng phục hồi, kiên cường",
                            ExampleEn = "She showed great resilience in the face of adversity.",
                            ExampleVi = "Cô ấy đã thể hiện sự kiên cường lớn khi đối mặt với khó khăn."
                        }
                    }
                }
            };
        }
    }
}
