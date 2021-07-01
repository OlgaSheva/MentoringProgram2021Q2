using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Catalog.Interfaces;
using Catalog.ViewModels;
using Microsoft.Extensions.Options;

namespace Catalog.Services
{
    public class FileCachePictureService : ICachePictureService
    {
        private readonly Regex _regex = new Regex(@"img(?'id'\d+)_(?'datetime'.+)");
        private readonly CashSettings _configuration;
        private readonly string _path;
        private readonly int _maxCashedImagesCount;
        private readonly int _cacheExpirationTimeInSec;
        private readonly DirectoryInfo _directory;

        public FileCachePictureService(IOptions<CashSettings> configuration)
        {
            _configuration = configuration == null
                ? throw new ArgumentNullException(nameof(configuration))
                : configuration.Value;
            _path = _configuration.Path;
            _maxCashedImagesCount = _configuration.MaxCashedImagesCount;
            _cacheExpirationTimeInSec = _configuration.CacheExpirationTimeInSec;
            _directory = Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}{_path}");
        }

        public async Task Cache(int id, Stream stream)
        {
            var files = _directory.GetFiles();
            if (files.Length < _maxCashedImagesCount)
            {
                var filePath = $"{_directory}\\img{id}_{DateTime.UtcNow:yyyyMMddTHHmmss}";
                await using FileStream fileStream = File.OpenWrite(filePath);
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(fileStream);
            }
        }

        public async Task<MemoryStream> GetCachedData(int id)
        {
            var fileName = GetExistFileName(id);
            if (fileName == null)
            {
                return null;
            }

            var filePath = $"{_directory}\\{fileName}";
            var cashedDateTime = DateTime.ParseExact(_regex.Match(fileName).Groups["datetime"].Value, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);
            if ((DateTime.UtcNow - cashedDateTime).Seconds > _cacheExpirationTimeInSec)
            {
                File.Delete(filePath);
                return null;
            }

            await using FileStream fileStream = File.OpenRead(filePath);
            var result = new MemoryStream();
            await fileStream.CopyToAsync(result);
            return result;
        }

        private string GetExistFileName(int id)
        {
            foreach (var file in _directory.GetFiles())
            {
                if (file.Name.StartsWith($"img{id}"))
                {
                    return file.Name;
                }
            }

            return null;
        }
    }
}
