using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Catalog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Catalog.Services
{
    public interface ICashPictureService
    {
        Task Cash(int id, Stream stream);

        Task<bool> TryCashedDataAsync(int id, HttpResponse response);
    }

    public class FileCashPictureService : ICashPictureService
    {
        private readonly Regex _regex = new Regex(@"img(?'id'\d+)_(?'datetime'.+)");
        private readonly CashSettings _configuration;
        private readonly string _path;
        private readonly int _maxCashedImagesCount;
        private readonly int _cacheExpirationTimeInSec;
        private readonly DirectoryInfo _directory;

        public FileCashPictureService(IOptions<CashSettings> configuration)
        {
            _configuration = configuration == null
                ? throw new ArgumentNullException(nameof(configuration))
                : configuration.Value;
            _path = _configuration.Path;
            _maxCashedImagesCount = _configuration.MaxCashedImagesCount;
            _cacheExpirationTimeInSec = _configuration.CacheExpirationTimeInSec;
            _directory = Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}{_path}");
        }

        public async Task Cash(int id, Stream stream)
        {
            var files = _directory.GetFiles();
            if (files.Length < _maxCashedImagesCount)
            {
                var filePath = $"{_directory}\\img{id}_{DateTime.UtcNow:yyyyMMddTHHmmss}";
                using (FileStream fileStream = File.OpenWrite(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
        }

        public async Task<bool> TryCashedDataAsync(int id, HttpResponse response)
        {
            var fileName = GetExistFileName(id);
            var filePath = $"{_directory}\\{fileName}";
            if (fileName != null)
            {
                var cashedDateTime = DateTime.ParseExact(_regex.Match(fileName).Groups["datetime"].Value, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);
                if ((DateTime.UtcNow - cashedDateTime).Seconds > _cacheExpirationTimeInSec)
                {
                    File.Delete(filePath);
                    return false;
                }

                response.ContentType = "image/bmp";
                using FileStream fileStream = File.OpenRead(filePath);
                Stream responseBodyStream = response.Body;
                await fileStream.CopyToAsync(responseBodyStream);
                return true;
            }

            return false;
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
