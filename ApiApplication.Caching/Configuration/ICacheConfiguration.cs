namespace ApiApplication.Caching.Configuration {
    public interface ICacheConfiguration {
        string Configuration {
            get;
        }
        string Instance {
            get;
        }
    }
}