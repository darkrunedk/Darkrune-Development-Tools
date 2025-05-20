using Darkrune_Development_Tools.Core.Models;
using System.Text.Json;

namespace Darkrune_Development_Tools.Core.ConfigHandlers
{
    public class AppsettingsHandler: ConfigHandlerBase
    {
        private readonly List<ConfigInfoDto> _keyValues = [];

        public override List<ConfigInfoDto> ProcessFile(string filePath)
        {
            var content = File.ReadAllText(filePath);

            JsonDocumentOptions options = new()
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            };

            using (JsonDocument doc = JsonDocument.Parse(content, options))
            {
                EnumerateSettings(doc.RootElement);
            }

            return _keyValues;
        }

        private void EnumerateSettings(JsonElement element, string name = "")
        {
            foreach (var property in element.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    string newName = GetKey(property, name);
                    EnumerateSettings(property.Value, newName);
                }
                else
                {
                    string key = GetKey(property, name);
                    _keyValues.Add(new ConfigInfoDto
                    {
                        Key = key,
                        Value = property.Value.ToString()
                    });
                }
            }
        }

        private static string GetKey(JsonProperty property, string name)
        {
            string newName = property.Name;
            if (!string.IsNullOrWhiteSpace(name))
                newName = $"{name}.{newName}";

            return newName;
        }
    }
}
