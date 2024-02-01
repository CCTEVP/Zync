namespace Zync.Api.Models.Output
{

    public class Style
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int ZIndex { get; set; }
    }
    public class Rule
    {
        public string Comparator { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Category { get; set; }
    }

    public class Visibility
    {
        public string State { get; set; }
        public string EnterTransition { get; set; }
        public string ExitTransition { get; set; }
        public IList<Rule>? Rules { get; set; }
    }

    public class Source
    {
        public string Url { get; set; }
        public string Type { get; set; }
    }

    public class Element
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Anchor { get; set; }
        public bool Interactive { get; set; }
        public Style Style { get; set; }
        public Visibility Visibility { get; set; }
        public IList<Source> Sources { get; set; }
        public string Url { get; set; }
    }

    public class Slide
    {
        public string Name { get; set; }
        public string Anchor { get; set; }
        public bool Interactive { get; set; }
        public Visibility SlideVisibility { get; set; }
        public IList<Element> Elements { get; set; }
    }


    public class Creative
    {
        public string ID { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string PublishedAt { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Interactive { get; set; }
        public bool WebsocketActive { get; set; }
        public bool PlaybacksActive { get; set; }
        public bool CustomPop { get; set; }
        public string Format { get; set; }
        public string Campaign { get; set; }
        public string Draft { get; set; }
        public string? SonataAudience { get; set; }
        public IList<Slide>? Slides { get; set; }
    }


}