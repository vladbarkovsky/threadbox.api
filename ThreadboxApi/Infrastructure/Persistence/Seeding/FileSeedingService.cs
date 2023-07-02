using Microsoft.AspNetCore.StaticFiles;
using System.Text.RegularExpressions;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Infrastructure.Persistence.Seeding
{
    /// <summary>
    /// Generates file entities for seeding
    /// </summary>
    public class FileSeedingService : ITransientService
    {
        private readonly GuidCombGenerator _guidCombGenerator;

        private List<string> FilePaths { get; }
        private int Iterator { get; set; }
        private FileExtensionContentTypeProvider FileExtensionContentTypeProvider { get; }

        public FileSeedingService(GuidCombGenerator guidCombGenerator)
        {
            FilePaths = Directory
                .GetFiles(Constants.CataasImagesDirectory)
                // Pad each match of numeric values in the file path with a 0 character to a length of 4 characters.
                // This is necessary to preserve the order of files containing a serial number in the name.
                // NOTE: tested only on Windows 10 NTFS
                // Taken from https://stackoverflow.com/a/46143251
                .OrderBy(filePath => Regex.Replace(filePath, @"\d+", match => match.Value.PadLeft(4, '0')))
                .ToList();

            FileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
        }

        /// <summary>
        /// Generates defined count of file entities.
        /// Maximum total count of entities per service injection is equal to count of files
        /// in directory used for files seeding
        /// </summary>
        /// <typeparam name="TEntity">Type of file entities you want to generate</typeparam>
        /// <param name="count">Count of file entities you want to generate</param>
        /// <returns>List of file entities</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<List<TEntity>> GetFiles<TEntity>(int count)
            where TEntity : FileEntity<TEntity>
        {
            if (Iterator + count > FilePaths.Count)
            {
                throw new ArgumentException("Not enough files.", nameof(count));
            }

            var filePaths = FilePaths.GetRange(Iterator, count);
            Iterator += count;
            var fileReadingTasks = filePaths.Select(x => File.ReadAllBytesAsync(x));
            var files = await Task.WhenAll(fileReadingTasks);
            var entities = new List<Domain.Entities.Owned.File>();

            for (int i = 0; i < filePaths.Count; i++)
            {
                if (!FileExtensionContentTypeProvider.TryGetContentType(filePaths[i], out var contentType))
                {
                    throw new Exception($"Can't get Content-Type HTTP header for {filePaths[i]}.");
                }

                entities.Add(new Domain.Entities.Owned.File
                {
                    Name = Path.GetFileName(filePaths[i]),
                    Extension = Path.GetExtension(filePaths[i]),
                    ContentType = contentType,
                    Data = files[i],
                });
            }

            return entities
                .Select(x =>
                {
                    var entity = Activator.CreateInstance<TEntity>();
                    entity.Id = _guidCombGenerator.Generate();
                    entity.File = x;
                    return entity;
                })
                .ToList();
        }
    }
}