namespace Zync.Api.Models.Input
{
    public class Sampler
    {
        public SampleContent Content { get; set; }
        public SampleConfig Config { get; set; }
    }
    // Define your complex objects (Content and Config) here
    public class SampleContent
    {
        public string? head { get; set; }
        public string body { get; set; }
        // Properties for Content
        // ...
    }

    public class SampleConfig
    {
        public int id { get; set; }
        public string? user { get; set; }
        // Properties for Config
        // ...
    }

}
