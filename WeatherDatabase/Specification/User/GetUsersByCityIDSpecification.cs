namespace WeatherDatabase.Specification.User
{
    using WeatherDatabase.Models;

    public class GetUsersByCityIDSpecification : Specification<User>
    {
        public GetUsersByCityIDSpecification(Guid cityID) : base(x => x.Cities.Select(c => c.Id).Contains(cityID))
        {
            AddInclude(x => x.Cities);
        }
    }
}
