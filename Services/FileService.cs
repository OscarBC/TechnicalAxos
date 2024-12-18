namespace TechnicalAxos_OscarBarrera.Services
{

    public interface IFileService
    {
        Task<Stream> OpenReadAsync(FileResult fileResult);
    }

    public class FileService : IFileService
    {
        public async Task<Stream> OpenReadAsync(FileResult fileResult)
        {
            return await fileResult.OpenReadAsync();
        }
    }

}