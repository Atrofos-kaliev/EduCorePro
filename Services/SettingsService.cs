using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EduCorePro.Services;

public class SettingsService
{
    private readonly string _settingsFilePath;

    public SettingsService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appData, "EduCorePro");
        Directory.CreateDirectory(appFolder);
        
        _settingsFilePath = Path.Combine(appFolder, "apikey.dat");
    }

    public void SaveApiKey(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) return;

        var encryptedData = ProtectedData.Protect(
            Encoding.UTF8.GetBytes(apiKey),
            null,
            DataProtectionScope.CurrentUser);
            
        File.WriteAllBytes(_settingsFilePath, encryptedData);
    }

    public string GetApiKey()
    {
        if (!File.Exists(_settingsFilePath)) return string.Empty;
        
        try
        {
            var encryptedData = File.ReadAllBytes(_settingsFilePath);
            var decryptedData = ProtectedData.Unprotect(
                encryptedData,
                null,
                DataProtectionScope.CurrentUser);
                
            return Encoding.UTF8.GetString(decryptedData);
        }
        catch
        {
            return string.Empty;
        }
    }
}