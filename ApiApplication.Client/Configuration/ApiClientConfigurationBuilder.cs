namespace ApiApplication.Client.Configuration {
    public class ApiClientConfigurationBuilder {
        private string address;
        private string apiKey;

        public ApiClientConfigurationBuilder Address(string value) {
            address = value;
            return this;
        }

        public ApiClientConfigurationBuilder ApiKey(string value) {
            apiKey = value;
            return this;
        }

        public IApiClientConfiguration Build() {
            return new ApiClientConfiguration(address, apiKey);
        }
    }
}
