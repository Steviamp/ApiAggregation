using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAggregation.Application.Dtos
{
    public class ErrorDto
    {
        public string? Message { get; set; }
        public string? Source { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
