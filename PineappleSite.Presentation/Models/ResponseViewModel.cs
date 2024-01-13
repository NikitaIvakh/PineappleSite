﻿namespace PineappleSite.Presentation.Models
{
    public class ResponseViewModel
    {
        public int Id { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; } = true;

        public string ValidationErrors { get; set; }
    }
}