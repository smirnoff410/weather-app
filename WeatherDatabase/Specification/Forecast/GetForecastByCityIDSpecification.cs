namespace WeatherDatabase.Specification.Forecast
{
    using WeatherDatabase.Models;
    public class GetForecastByCityIDSpecification : Specification<Forecast>
    {
        public GetForecastByCityIDSpecification(Guid cityID) : base(x => x.CityId == cityID && x.Date >= DateTime.UtcNow.Date)
        {
            AddOrderBy(x => x.Date);
        }
    }
}
