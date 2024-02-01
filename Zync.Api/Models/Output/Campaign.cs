namespace Zync.Api.Models.Output
{
    public class Campaign
    {
        public string ID { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Advertiser { get; set; }
        public int Week { get; set; }
        public string OrderAida { get; set; }
    }
}