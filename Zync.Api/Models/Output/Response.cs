using System.Reflection;

namespace Zync.Api.Models.Output
{
    public class Response
    {
        public DateTime TS { get; set; }
        public string? Result { get; set; }
        public string? Mode { get; set; }

        public override string ToString()
        {
            Response reponse = this;
            Type type = reponse.GetType();
            PropertyInfo[] properties = type.GetProperties();
            int size = properties.Length;
            string outer = "{";
            foreach (PropertyInfo property in properties)
            {
                outer += $"\"{property.Name}\":\"{property.GetValue(reponse)}\"";
                if (size > 1) outer += ", ";
                size -= 1;
            }
            outer += "}";
            return outer;
        }
    }
}
