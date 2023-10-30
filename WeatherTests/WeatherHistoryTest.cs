using Bogus;
using WeatherBackend.History.Models;

namespace WeatherTests
{
    public class WeatherHistoryTest
    {
        [Theory]
        [InlineData(0, 2)]
        [InlineData(25, 26)]
        public void WeatherHistoryGenerateTypeWalkTest(int minTemperature, int maxTemperature)
        {
            #region Arrange
            var weatherHistoryModel = InitializeWeatherHistory(minTemperature, maxTemperature);
            #endregion

            #region Act
            var result = weatherHistoryModel.Generate();
            #endregion

            #region Assert
            Assert.Equal(EWeatherForecastType.Walk, result.Type);
            #endregion
        }

        [Fact]
        public void WeatherHistoryGenerateTypeSwimTest()
        {
            #region Arrange
            var weatherHistoryModel = InitializeWeatherHistory(25, 27);
            #endregion

            #region Act
            var result = weatherHistoryModel.Generate();
            #endregion

            #region Assert
            Assert.Equal(EWeatherForecastType.Swim, result.Type);
            #endregion
        }

        [Fact]
        public void WeatherHistoryGenerateTypeHomeTest()
        {
            #region Arrange
            var weatherHistoryModel = InitializeWeatherHistory(0, 1);
            #endregion

            #region Act
            var result = weatherHistoryModel.Generate();
            #endregion

            #region Assert
            Assert.Equal(EWeatherForecastType.Home, result.Type);
            #endregion
        }

        private static Faker<WeatherHistory> InitializeWeatherHistory(int minTemperature, int maxTemperature)
        {
            return new Faker<WeatherHistory>()
                .RuleFor(x => x.Date, f => f.Date.PastDateOnly())
                .RuleFor(x => x.MinTemperature, minTemperature)
                .RuleFor(x => x.MaxTemperature, maxTemperature);
        }
    }
}