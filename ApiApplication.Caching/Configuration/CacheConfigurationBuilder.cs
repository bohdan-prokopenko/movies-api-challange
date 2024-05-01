namespace ApiApplication.Caching.Configuration {
    public class CacheConfigurationBuilder {
        private string configuration;
        private string instance;

        public CacheConfigurationBuilder Configuration(string value) {
            configuration = value;
            return this;
        }

        public CacheConfigurationBuilder Instance(string value) {
            instance = value;
            return this;
        }

        public ICacheConfiguration Build() {
            return new RedisCacheConfiguration(configuration, instance);
        }
    }
}
