namespace ApiApplication.Client.Configuration {
    internal class ApiClientConfiguration : IApiClientConfiguration {
        public string Address {
            get; private set;
        }

        public string ApiKey {
            get; private set;
        }

        public ApiClientConfiguration(string address, string apiKey) {
            Address = address;
            ApiKey = apiKey;
        }
    }
}
