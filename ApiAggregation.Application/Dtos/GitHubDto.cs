using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAggregation.Application.Dtos
{
    public class GithubDataDto
    {
        public string? RepositoryName { get; set; }
        public string? Description { get; set; }
        public int Stars { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? OwnerName { get; set; }
        public string? RepositoryUrl { get; set; }
    }
}
