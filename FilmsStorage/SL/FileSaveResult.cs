﻿using System;

namespace FilmsStorage.SL
{
    public class FileSaveResult
    {
        public bool IsSaved { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Exception Error { get; set; }
    }
}