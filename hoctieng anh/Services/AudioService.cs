using System;
using System.Diagnostics;

namespace EnglishFlashcard3D.Services
{
    public class AudioService : IDisposable
    {
        // Sử dụng Windows PowerShell để phát âm (compatibility với .NET 10)
        public void SpeakAsync(string text)
        {
            try
            {
                // Escape quotes trong text
                var escapedText = text.Replace("\"", "\\\"");
                var psCommand = $"Add-Type -AssemblyName System.Speech; " +
                    $"$speak = New-Object System.Speech.Synthesis.SpeechSynthesizer; " +
                    $"$speak.Speak(\\\"{escapedText}\\\")";

                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -Command \"{psCommand}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                };

                using (var process = Process.Start(psi))
                {
                    // Fire and forget - không cần chờ
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TTS Error: {ex.Message}");
            }
        }

        public void Dispose()
        {
            // Không cần dispose vì không có managed resources
        }
    }
}
