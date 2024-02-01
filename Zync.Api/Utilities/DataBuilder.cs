using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MongoDB.Bson;
using System.Globalization;
using System.Reflection;
using Zync.Api.Models.Output;

namespace Zync.Api.Utilities
{
    public class DataBuilder
    {
        public BsonDocument document = new BsonDocument();
        public Type modelType = typeof(string);
        public DataBuilder(BsonDocument document) { 
            this.document = document;
        }
        private dynamic buildValue(dynamic input, string name)
        {
            Console.WriteLine("-> "+name+": ");

            if (input.Contains(name))
            {
                Type inputType = input.GetElement(name).Value.GetType();
                string inputTypeName = inputType.Name;
                //Console.WriteLine("Input is " + name + " (" + inputTypeName + ") from: ");
                //Console.WriteLine(input.GetElement(name));
                switch (inputTypeName)
                {
                    case "BsonObjectId":
                        BsonObjectId idValue = input.GetElement(name).Value;
                        return idValue.ToString();
                    case "BsonDateTime":
                        BsonDateTime dateTimeValue = input.GetElement(name).Value;
                        return dateTimeValue.ToString();
                    case "BsonString":
                        BsonString stringValue = input.GetElement(name).Value;
                        return stringValue.ToString();
                    case "BsonDocument":
                        if (name.StartsWith("_"))
                        {
                            BsonDocument document = input.GetElement(name).Value;
                            string valueDoc = document.GetElement("_id").Value.ToString();
                            return valueDoc;
                        }
                        else
                        {
                            return input.GetElement(name).Value.AsBsonDocument;
                        }
                    case "BsonArray":
                        return input.GetElement(name).Value.AsBsonArray;
                    case "BsonBoolean":
                        BsonBoolean booleanValue = input.GetElement(name).Value;
                        return booleanValue.AsBoolean;
                    case "BsonInt32":
                        return input.GetElement(name).Value.AsInt32;
                    case "BsonNull":
                        return null;
                    case "bool":
                        return input.GetElement(name).Value.AsBoolean;
                    default:
                        return input.GetElement(name).Value;
                }
            }
            else
            {
                string propertyName = name.StartsWith("_") ? char.ToUpper(name[1]) + name.Substring(2) : char.ToUpper(name[0]) + name.Substring(1);
                Console.WriteLine(propertyName);
                PropertyInfo propertyInfo = this.modelType.GetProperty(propertyName);
                Type outputType = propertyInfo.PropertyType;
                string outputTypeName = outputType.Name;

                Console.WriteLine("Is " + outputTypeName);

                switch (outputTypeName)
                {
                    case "Int32":
                        return 0;
                    case "String":
                        return "";
                    case "Boolean":
                        return false;
                }
            }
            return null;
        }
        private Style buildStyle(BsonDocument input)
        {
            Style style = new Style();
            style.Top = buildValue(input, "top");
            style.Left = buildValue(input, "left");
            style.Width = buildValue(input, "width");
            style.Height = buildValue(input, "height");
            style.ZIndex = buildValue(input, "zIndex");
            return style;
        }
        private Rule buildRule(BsonDocument input)
        {
            Rule rule = new Rule();
            rule.Comparator = buildValue(input, "comparator");
            rule.Type = buildValue(input, "type");
            rule.Value = buildValue(input, "value");
            rule.Category = buildValue(input, "category");
            return rule;
        }
        private Visibility buildVisibility(BsonDocument input)
        {
            Visibility visibility = new Visibility();
            visibility.State = buildValue(input, "state");
            visibility.EnterTransition = buildValue(input, "enterTransition");
            visibility.ExitTransition = buildValue(input, "exitTransition");
            if (input.Contains("rules"))
            {
                visibility.Rules = new List<Rule>();
                foreach (BsonDocument item in buildValue(input, "rules"))
                {
                    visibility.Rules.Add(buildRule(item));
                }
            }
            return visibility;
        }
        private Source buildSource(BsonDocument input)
        {
            Source source = new Source();
            source.Url = buildValue(input, "url");
            source.Type = buildValue(input, "type");
            return source;
        }
        private Element buildElement(BsonDocument input)
        {
            Element element = new Element();
            element.Name = buildValue(input, "name");
            element.Type = buildValue(input, "type");
            element.Anchor = buildValue(input, "anchor");
            element.Interactive = buildValue(input, "interactive");
            element.Style = buildStyle(buildValue(input, "style"));
            element.Visibility = buildVisibility(buildValue(input, "visibility"));
            if (input.Contains("sources"))
            {
                element.Sources = new List<Source>();
                foreach (BsonDocument item in buildValue(input, "sources"))
                {
                    element.Sources.Add(buildSource(item));
                }
            }
            element.Url = buildValue(input, "url");
            return element;
        }
        private Slide buildSlide(BsonDocument input)
        {
            Slide slide = new Slide();
            slide.Name = buildValue(input, "name");
            slide.Anchor = buildValue(input, "anchor");
            slide.Interactive = buildValue(input, "interactive");
            slide.SlideVisibility = buildVisibility(buildValue(input, "visibility"));
            slide.Elements = new List<Element>();
            foreach (BsonDocument item in buildValue(input, "elements"))
            {
                slide.Elements.Add(buildElement(item));
            }
            return slide;
        }
        public Creative buildCreative()
        {
            Creative creative = new Creative();
            this.modelType = typeof(Creative);
            creative.ID = buildValue(this.document, "_id");
            creative.CreatedAt = buildValue(this.document, "createdAt");
            creative.UpdatedAt = buildValue(this.document, "updatedAt");
            creative.PublishedAt = buildValue(this.document, "publishedAt");
            creative.Name = buildValue(this.document, "name");
            creative.Type = buildValue(this.document, "type");
            creative.Interactive = buildValue(this.document, "interactive");
            creative.WebsocketActive = buildValue(this.document, "websocketActive");
            creative.PlaybacksActive = buildValue(this.document, "playbacksActive");
            creative.CustomPop = buildValue(this.document, "customPop");
            creative.Draft = buildValue(this.document, "draft");
            creative.Format = buildValue(this.document, "_format");
            creative.Campaign = buildValue(this.document, "_campaign");
            creative.SonataAudience = buildValue(this.document, "_sonataAudience");
            creative.Slides = new List<Slide>();
            foreach (BsonDocument item in buildValue(this.document, "slides"))
            {
                creative.Slides.Add(buildSlide(item));
            }
            return creative;
        }

        public Campaign buildCampaign()
        {
            Campaign campaign = new Campaign();
            this.modelType = typeof(Campaign);
            campaign.ID = buildValue(this.document, "_id");
            campaign.CreatedAt = buildValue(this.document, "createdAt");
            campaign.UpdatedAt = buildValue(this.document, "updatedAt");
            campaign.Name = buildValue(this.document, "name");
            campaign.StartDate = buildValue(this.document, "startDate");
            campaign.EndDate = buildValue(this.document, "endDate");
            campaign.Advertiser = buildValue(this.document, "advertiser");
            campaign.Week = buildValue(this.document, "week");
            campaign.OrderAida = buildValue(this.document, "orderAida");
            return campaign;
        }
    }
}
