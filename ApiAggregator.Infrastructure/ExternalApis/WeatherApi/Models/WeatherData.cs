using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAggregator.Infrastructure.ExternalApis.WeatherApi.Models
{
    public class WeatherData
    {
        public string Name { get; set; }
        public MainWeather Main { get; set; }
        public IEnumerable<Weather> Weather { get; set; }
    }

    public class MainWeather
    {
        public double Temp { get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
    }

    public class Weather
    {
        public string Main { get; set; }
        public string Description { get; set; }
    }
}
