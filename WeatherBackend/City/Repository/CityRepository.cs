namespace WeatherBackend.City.Repository
{
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Options;
    using Models;
    using System.Threading.Tasks;
    using WeatherBackend.Settings;

    public class CityRepository : ICityRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public CityRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }
        public async Task<Guid> Create(CreateCityDTO dto)
        {
            var sqlConnection = new SqlConnection(_databaseSettings.ConnectionString);

            await sqlConnection.OpenAsync();
            var id = Guid.NewGuid();
            var insertCommand = new SqlCommand($"insert into [dbo].[Cities] values ('{id}', '{dto.Name}')", sqlConnection);

            await insertCommand.ExecuteNonQueryAsync();

            await sqlConnection.CloseAsync();

            return id;
        }

        public async Task<IEnumerable<City>> List()
        {
            var result = new List<City>();

            var sqlConnection = new SqlConnection(_databaseSettings.ConnectionString);

            await sqlConnection.OpenAsync();

            var selectCommand = new SqlCommand("select * from [dbo].[Cities]", sqlConnection);

            //ExecuteNonQuery - для запросов без результата
            //ExecuteReader - для запросов с результатом select
            //ExecuteScalar - для встроенных функций SQL (Min, Max, Count)

            var reader = selectCommand.ExecuteReader();
            if (reader.HasRows)
            {
                // выводим названия столбцов
                string columnName1 = reader.GetName(0);
                string columnName2 = reader.GetName(1);

                Console.WriteLine($"{columnName1}\t{columnName2}");

                while (reader.Read()) // построчно считываем данные
                {
                    Guid id = reader.GetGuid(0);
                    string name = reader.GetString(1);
                    result.Add(new City
                    {
                        ID = id,
                        Name = name
                    });
                }
            }
            reader.Close();

            await sqlConnection.CloseAsync();

            return result;
        }
    }
}
