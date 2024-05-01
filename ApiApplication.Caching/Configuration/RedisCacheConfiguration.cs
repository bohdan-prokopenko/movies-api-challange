namespace ApiApplication.Caching.Configuration {
    internal class RedisCacheConfiguration : ICacheConfiguration {
        public RedisCacheConfiguration(string configuration, string instance) {
            Configuration = configuration;
            Instance = instance;
        }

        public string Configuration {
            get; private set;
        }

        public string Instance {
            get; private set;
        }
    }
}
