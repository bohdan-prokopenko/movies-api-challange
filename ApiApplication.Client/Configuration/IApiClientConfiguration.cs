namespace ApiApplication.Client.Configuration {
    public interface IApiClientConfiguration {
        string Address {
            get;
        }
        string ApiKey {
            get;
        }
    }
}