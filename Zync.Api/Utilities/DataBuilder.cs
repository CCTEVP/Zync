using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MongoDB.Bson;
using System;
using System.Globalization;
using System.Reflection;
using Zync.Api.Models.Output;

namespace Zync.Api.Utilities
{
    public class DataBuilder
    {
        public BsonDocument document = new BsonDocument();
        public Type modelType = typeof(string);
        public DateTime dateTimeValue;
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
                    case "BsonDouble":
                        if (name == "updatedAt")
                        {
                            long numericValue = long.Parse(input.GetElement(name).Value.ToString());
                            dateTimeValue = DateTime.FromFileTimeUtc(numericValue);
                            return dateTimeValue.ToString();
                        }
                        else
                        {
                            return input.GetElement(name).Value.AsDouble;
                        }
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
        public Format buildFormat()
        {
            Format format = new Format();
            this.modelType = typeof(Format);
            format.ID = buildValue(this.document, "_id");
            format.CreatedAt = buildValue(this.document, "createdAt");
            format.UpdatedAt = buildValue(this.document, "updatedAt");
            format.Name = buildValue(this.document, "name");
            format.Width = buildValue(this.document, "width");
            format.Height = buildValue(this.document, "height");
            format.Group = buildValue(this.document, "group");
            return format;
        }
        public Player buildPlayer()
        {
            Player player = new Player();
            this.modelType = typeof(Player);
            player.ID = buildValue(this.document, "_id");
            player.BsPlayerId = buildValue(this.document, "bsPlayerId");
            player.BsFrameId = buildValue(this.document, "bsFrameId");
            player.Latitude = buildValue(this.document, "latitude");
            player.Longitude = buildValue(this.document, "longitude");
            player.City = buildValue(this.document, "city");
            player.Country = buildValue(this.document, "country");
            player.PlayerName = buildValue(this.document, "playerName");
            player.DisplayUnitId = buildValue(this.document, "displayUnitId");
            player.PanelId = buildValue(this.document, "panelId");
            player.FaceId = buildValue(this.document, "faceId");
            player.DisplayUnitName = buildValue(this.document, "displayUnitName");
            player.Status = buildValue(this.document, "status");
            player.Frames = buildValue(this.document, "frames");
            player.FrameName = buildValue(this.document, "frameName");
            player.GeometryType = buildValue(this.document, "geometryType");
            player.FrameWidth = buildValue(this.document, "frameWidth");
            player.FrameHeight = buildValue(this.document, "frameHeight");
            player.Community = buildValue(this.document, "community");
            player.Province = buildValue(this.document, "province");
            player.Zone = buildValue(this.document, "zone");
            player.District = buildValue(this.document, "district");
            player.Neightborhood = buildValue(this.document, "neightborhood");
            player.Address = buildValue(this.document, "address");
            player.Street = buildValue(this.document, "street");
            player.StreetNumber = buildValue(this.document, "streetNumber");
            player.PostalCode = buildValue(this.document, "postalCode");
            player.Format = buildValue(this.document, "format");
            player.Furniture = buildValue(this.document, "furniture");
            player.Connect = buildValue(this.document, "connect");
            player.BusStation = buildValue(this.document, "busStation");
            player.TrainStation = buildValue(this.document, "trainStation");
            player.SubwayStation = buildValue(this.document, "subwayStation");
            player.PlayerClass = buildValue(this.document, "playerClass");
            player.TrafficOrigin = buildValue(this.document, "trafficOrigin");
            player.TrafficDestination = buildValue(this.document, "trafficDestination");
            player.TrafficBMFast = buildValue(this.document, "trafficBMFast");
            player.TrafficBMNormal = buildValue(this.document, "trafficBMNormal");
            player.TrafficBMSlow = buildValue(this.document, "trafficBMSlow");
            player.WifiProbesGroup = buildValue(this.document, "wifiProbesGroup");
            player.LanguageFR = buildValue(this.document, "languageFR");
            player.LanguageNL = buildValue(this.document, "languageNL");
            player.Geohash = buildValue(this.document, "geohash");
            player.AffluenceSiteId = buildValue(this.document, "affluenceSiteId");
            player.SonataLocation = buildValue(this.document, "sonataLocation");
            player.AppcelerateLocation = buildValue(this.document, "appcelerateLocation");
            player.Group = buildValue(this.document, "group");
            player.BusStopName = buildValue(this.document, "busStopName");
            player.BusStopId = buildValue(this.document, "busStopId");
            player.ClientId = buildValue(this.document, "clientId");
            player.UpdatedAt = buildValue(this.document, "updatedAt");
            player.BusStopPanel = buildValue(this.document, "busStopPanel");
            player.Name = buildValue(this.document, "name");
            player.FrameId = buildValue(this.document, "frameId");

            return player;
        }
    }
}
