using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;

namespace Consultancy.Services;

public interface IGoogleDriveService
{
    Task<List<GoogleFile>> ListFilesInFolderAsync(string folderId);
    Task<bool> TestConnectionAsync();
}

public class GoogleDriveService : IGoogleDriveService
{
    private DriveService? _driveService;
    private readonly IConfiguration _configuration;
    
    public GoogleDriveService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    private DriveService? GetDriveService()
    {
        if (_driveService != null)
            return _driveService;
            
        var serviceAccountEmail = _configuration["GoogleDrive:ServiceAccountEmail"];
        var privateKeyPath = _configuration["GoogleDrive:PrivateKeyPath"];
        
        if (string.IsNullOrEmpty(serviceAccountEmail) || string.IsNullOrEmpty(privateKeyPath))
        {
            return null; // Return null instead of throwing
        }
        
        if (!File.Exists(privateKeyPath))
        {
            return null; // Return null instead of throwing
        }
        
        var credential = GoogleCredential.FromJson(File.ReadAllText(privateKeyPath))
            .CreateScoped(new[] { DriveService.Scope.DriveReadonly });
        
        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Consultancy.NETH"
        });
        
        return _driveService;
    }
    
    public async Task<List<GoogleFile>> ListFilesInFolderAsync(string folderId)
    {
        var driveService = GetDriveService();
        if (driveService == null)
            return new List<GoogleFile>(); // Return empty list if not configured
            
        var files = new List<GoogleFile>();
        string? pageToken = null;
        
        do
        {
            var request = driveService.Files.List();
            request.Q = $"'{folderId}' in parents and trashed=false";
            request.Fields = "nextPageToken, files(id, name, mimeType, size, modifiedTime)";
            request.PageSize = 100;
            request.PageToken = pageToken;
            
            var result = await request.ExecuteAsync();
            
            if (result.Files != null)
            {
                foreach (var file in result.Files)
                {
                    files.Add(new GoogleFile
                    {
                        Id = file.Id,
                        Name = file.Name,
                        MimeType = file.MimeType,
                        Size = file.Size,
                        ModifiedTime = file.ModifiedTimeDateTimeOffset?.DateTime
                    });
                }
            }
            
            pageToken = result.NextPageToken;
        } while (pageToken != null);
        
        return files;
    }
    
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            var driveService = GetDriveService();
            if (driveService == null)
                return false;
                
            var request = driveService.Files.List();
            request.PageSize = 1;
            await request.ExecuteAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class GoogleFile
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long? Size { get; set; }
    public DateTime? ModifiedTime { get; set; }
}
