﻿namespace Catalog.ViewModels
{
    public class CashSettings
    {
        public string Path { get; set; }

        public int MaxCashedImagesCount { get; set; }

        public int CacheExpirationTimeInSec { get; set; }
    }
}
