namespace WeatherCommon.Models.Request
{
    public record WeatherChangeAlertRequest(Guid CityID, string Text);
}
