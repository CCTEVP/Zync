using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MongoDB.Bson;
using System;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using Zync.Api.Models.Output;

namespace Zync.Api.Utilities
{
    public class DataBuilder
    {
        public BsonDocument currentDocument = new BsonDocument();
        public BsonArray array = new BsonArray();
        public Type modelType = typeof(string);
        public DateTime dateTimeValue;
        public DataBuilder(dynamic current)
        {
            Type inputType = current.GetType();
            string inputTypeName = inputType.Name;

            Console.WriteLine(inputTypeName);
            this.currentDocument = current;
        }
        private dynamic buildValue(dynamic input, string name)
        {
            Console.WriteLine("-> " + name + ": ");

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
                            BsonDocument currentDocument = input.GetElement(name).Value;
                            string valueDoc = currentDocument.GetElement("_id").Value.ToString();
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
            creative.ID = buildValue(this.currentDocument, "_id");
            creative.CreatedAt = buildValue(this.currentDocument, "createdAt");
            creative.UpdatedAt = buildValue(this.currentDocument, "updatedAt");
            creative.PublishedAt = buildValue(this.currentDocument, "publishedAt");
            creative.Name = buildValue(this.currentDocument, "name");
            creative.Type = buildValue(this.currentDocument, "type");
            creative.Interactive = buildValue(this.currentDocument, "interactive");
            creative.WebsocketActive = buildValue(this.currentDocument, "websocketActive");
            creative.PlaybacksActive = buildValue(this.currentDocument, "playbacksActive");
            creative.CustomPop = buildValue(this.currentDocument, "customPop");
            creative.Draft = buildValue(this.currentDocument, "draft");
            creative.Format = buildValue(this.currentDocument, "_format");
            creative.Campaign = buildValue(this.currentDocument, "_campaign");
            creative.SonataAudience = buildValue(this.currentDocument, "_sonataAudience");
            creative.Slides = new List<Slide>();
            foreach (BsonDocument item in buildValue(this.currentDocument, "slides"))
            {
                creative.Slides.Add(buildSlide(item));
            }
            return creative;
        }

        public Campaign buildCampaign()
        {
            Campaign campaign = new Campaign();
            this.modelType = typeof(Campaign);
            campaign.ID = buildValue(this.currentDocument, "_id");
            campaign.CreatedAt = buildValue(this.currentDocument, "createdAt");
            campaign.UpdatedAt = buildValue(this.currentDocument, "updatedAt");
            campaign.Name = buildValue(this.currentDocument, "name");
            campaign.StartDate = buildValue(this.currentDocument, "startDate");
            campaign.EndDate = buildValue(this.currentDocument, "endDate");
            campaign.Advertiser = buildValue(this.currentDocument, "advertiser");
            campaign.Week = buildValue(this.currentDocument, "week");
            campaign.OrderAida = buildValue(this.currentDocument, "orderAida");
            return campaign;
        }
        public Format buildFormat()
        {
            Format format = new Format();
            this.modelType = typeof(Format);
            format.ID = buildValue(this.currentDocument, "_id");
            format.CreatedAt = buildValue(this.currentDocument, "createdAt");
            format.UpdatedAt = buildValue(this.currentDocument, "updatedAt");
            format.Name = buildValue(this.currentDocument, "name");
            format.Width = buildValue(this.currentDocument, "width");
            format.Height = buildValue(this.currentDocument, "height");
            format.Group = buildValue(this.currentDocument, "group");
            return format;
        }
        public Player buildPlayer()
        {
            Player player = new Player();
            this.modelType = typeof(Player);
            player.ID = buildValue(this.currentDocument, "_id");
            player.BsPlayerId = buildValue(this.currentDocument, "bsPlayerId");
            player.BsFrameId = buildValue(this.currentDocument, "bsFrameId");
            player.Latitude = buildValue(this.currentDocument, "latitude");
            player.Longitude = buildValue(this.currentDocument, "longitude");
            player.City = buildValue(this.currentDocument, "city");
            player.Country = buildValue(this.currentDocument, "country");
            player.PlayerName = buildValue(this.currentDocument, "playerName");
            player.DisplayUnitId = buildValue(this.currentDocument, "displayUnitId");
            player.PanelId = buildValue(this.currentDocument, "panelId");
            player.FaceId = buildValue(this.currentDocument, "faceId");
            player.DisplayUnitName = buildValue(this.currentDocument, "displayUnitName");
            player.Status = buildValue(this.currentDocument, "status");
            player.Frames = buildValue(this.currentDocument, "frames");
            player.FrameName = buildValue(this.currentDocument, "frameName");
            player.GeometryType = buildValue(this.currentDocument, "geometryType");
            player.FrameWidth = buildValue(this.currentDocument, "frameWidth");
            player.FrameHeight = buildValue(this.currentDocument, "frameHeight");
            player.Community = buildValue(this.currentDocument, "community");
            player.Province = buildValue(this.currentDocument, "province");
            player.Zone = buildValue(this.currentDocument, "zone");
            player.District = buildValue(this.currentDocument, "district");
            player.Neightborhood = buildValue(this.currentDocument, "neightborhood");
            player.Address = buildValue(this.currentDocument, "address");
            player.Street = buildValue(this.currentDocument, "street");
            player.StreetNumber = buildValue(this.currentDocument, "streetNumber");
            player.PostalCode = buildValue(this.currentDocument, "postalCode");
            player.Format = buildValue(this.currentDocument, "format");
            player.Furniture = buildValue(this.currentDocument, "furniture");
            player.Connect = buildValue(this.currentDocument, "connect");
            player.BusStation = buildValue(this.currentDocument, "busStation");
            player.TrainStation = buildValue(this.currentDocument, "trainStation");
            player.SubwayStation = buildValue(this.currentDocument, "subwayStation");
            player.PlayerClass = buildValue(this.currentDocument, "playerClass");
            player.TrafficOrigin = buildValue(this.currentDocument, "trafficOrigin");
            player.TrafficDestination = buildValue(this.currentDocument, "trafficDestination");
            player.TrafficBMFast = buildValue(this.currentDocument, "trafficBMFast");
            player.TrafficBMNormal = buildValue(this.currentDocument, "trafficBMNormal");
            player.TrafficBMSlow = buildValue(this.currentDocument, "trafficBMSlow");
            player.WifiProbesGroup = buildValue(this.currentDocument, "wifiProbesGroup");
            player.LanguageFR = buildValue(this.currentDocument, "languageFR");
            player.LanguageNL = buildValue(this.currentDocument, "languageNL");
            player.Geohash = buildValue(this.currentDocument, "geohash");
            player.AffluenceSiteId = buildValue(this.currentDocument, "affluenceSiteId");
            player.SonataLocation = buildValue(this.currentDocument, "sonataLocation");
            player.AppcelerateLocation = buildValue(this.currentDocument, "appcelerateLocation");
            player.Group = buildValue(this.currentDocument, "group");
            player.BusStopName = buildValue(this.currentDocument, "busStopName");
            player.BusStopId = buildValue(this.currentDocument, "busStopId");
            player.ClientId = buildValue(this.currentDocument, "clientId");
            player.UpdatedAt = buildValue(this.currentDocument, "updatedAt");
            player.BusStopPanel = buildValue(this.currentDocument, "busStopPanel");
            player.Name = buildValue(this.currentDocument, "name");
            player.FrameId = buildValue(this.currentDocument, "frameId");

            return player;
        }
        
        public Player buildPlayer(BsonDocument input) {
            Player player = new Player();
            this.modelType = typeof(Player);
            player.ID = buildValue(input, "_id");
            player.BsPlayerId = buildValue(input, "bsPlayerId");
            player.BsFrameId = buildValue(input, "bsFrameId");
            player.Latitude = buildValue(input, "latitude");
            player.Longitude = buildValue(input, "longitude");
            player.City = buildValue(input, "city");
            player.Country = buildValue(input, "country");
            player.PlayerName = buildValue(input, "playerName");
            player.DisplayUnitId = buildValue(input, "displayUnitId");
            player.PanelId = buildValue(input, "panelId");
            player.FaceId = buildValue(input, "faceId");
            player.DisplayUnitName = buildValue(input, "displayUnitName");
            player.Status = buildValue(input, "status");
            player.Frames = buildValue(input, "frames");
            player.FrameName = buildValue(input, "frameName");
            player.GeometryType = buildValue(input, "geometryType");
            player.FrameWidth = buildValue(input, "frameWidth");
            player.FrameHeight = buildValue(input, "frameHeight");
            player.Community = buildValue(input, "community");
            player.Province = buildValue(input, "province");
            player.Zone = buildValue(input, "zone");
            player.District = buildValue(input, "district");
            player.Neightborhood = buildValue(input, "neightborhood");
            player.Address = buildValue(input, "address");
            player.Street = buildValue(input, "street");
            player.StreetNumber = buildValue(input, "streetNumber");
            player.PostalCode = buildValue(input, "postalCode");
            player.Format = buildValue(input, "format");
            player.Furniture = buildValue(input, "furniture");
            player.Connect = buildValue(input, "connect");
            player.BusStation = buildValue(input, "busStation");
            player.TrainStation = buildValue(input, "trainStation");
            player.SubwayStation = buildValue(input, "subwayStation");
            player.PlayerClass = buildValue(input, "playerClass");
            player.TrafficOrigin = buildValue(input, "trafficOrigin");
            player.TrafficDestination = buildValue(input, "trafficDestination");
            player.TrafficBMFast = buildValue(input, "trafficBMFast");
            player.TrafficBMNormal = buildValue(input, "trafficBMNormal");
            player.TrafficBMSlow = buildValue(input, "trafficBMSlow");
            player.WifiProbesGroup = buildValue(input, "wifiProbesGroup");
            player.LanguageFR = buildValue(input, "languageFR");
            player.LanguageNL = buildValue(input, "languageNL");
            player.Geohash = buildValue(input, "geohash");
            player.AffluenceSiteId = buildValue(input, "affluenceSiteId");
            player.SonataLocation = buildValue(input, "sonataLocation");
            player.AppcelerateLocation = buildValue(input, "appcelerateLocation");
            player.Group = buildValue(input, "group");
            player.BusStopName = buildValue(input, "busStopName");
            player.BusStopId = buildValue(input, "busStopId");
            player.ClientId = buildValue(input, "clientId");
            player.UpdatedAt = buildValue(input, "updatedAt");
            player.BusStopPanel = buildValue(input, "busStopPanel");
            player.Name = buildValue(input, "name");
            player.FrameId = buildValue(input, "frameId");
            Console.WriteLine(player.Name);
            return player;
        }
        public Players buildPlayerList()
        {
            Players playersList = new Players();
            this.modelType = typeof(Player);
            playersList.PlayersList = new List<Player>();
            foreach (BsonDocument item in this.currentDocument.AsBsonArray)
            {
                playersList.PlayersList.Add(buildPlayer(item));
            }
            return playersList;

        }
    }
}
