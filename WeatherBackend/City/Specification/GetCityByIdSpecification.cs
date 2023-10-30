namespace WeatherBackend.City.Specification
{
    using System;
    using WeatherDatabase.Models;
    using WeatherDatabase.Specification;

    public class GetCityByIdSpecification : Specification<City>
    {
        public GetCityByIdSpecification(Guid id) : base(x => x.Id == id)
        {
        }
    }
}
