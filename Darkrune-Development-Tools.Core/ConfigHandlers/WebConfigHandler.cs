using Darkrune_Development_Tools.Core.Models;
using System.Xml;

namespace Darkrune_Development_Tools.Core.ConfigHandlers
{
    public class WebConfigHandler : ConfigHandlerBase
    {
        private readonly List<ConfigInfoDto> _keyValues = [];

        public override List<ConfigInfoDto> ProcessFile(string filePath)
        {
            var content = File.ReadAllText(filePath);

            try
            {
                XmlDocument doc = new();
                doc.LoadXml(content);

                var root = doc.DocumentElement;
                var appsettings = root.SelectSingleNode("appSettings");
                if (appsettings != null)
                {
                    XmlNodeList elements = appsettings.SelectNodes("add");
                    foreach (XmlElement element in elements)
                    {
                        ConfigInfoDto configInfoDto = new()
                        {
                            Key = element.GetAttribute("key"),
                            Value = element.GetAttribute("value")
                        };
                        _keyValues.Add(configInfoDto);
                    }
                }

                var connectionStrings = root.SelectSingleNode("connectionStrings");
                if (connectionStrings != null)
                {
                    var elements = connectionStrings.SelectNodes("add");
                    foreach (XmlElement element in elements)
                    {
                        ConfigInfoDto configInfoDto = new()
                        {
                            Key = element.GetAttribute("name"),
                            Value = element.GetAttribute("connectionString")
                        };
                        _keyValues.Add(configInfoDto);
                    }
                }
            }
            catch (Exception)
            {
                return [];
            }

            return _keyValues;
        }
    }
}
