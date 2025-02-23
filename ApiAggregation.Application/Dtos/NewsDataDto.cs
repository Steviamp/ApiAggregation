﻿namespace ApiAggregation.Application.Dtos
{
    public class NewsDataDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Source { get; set; }
        public DateTime PublishedAt { get; set; }
        public string? Url { get; set; }
    }
}
