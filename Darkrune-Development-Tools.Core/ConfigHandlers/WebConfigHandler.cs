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
                        string key = element.GetAttribute("key");
                        bool keyExist = _keyValues.Any(x => x.Key == key);
                        if (keyExist)
                            continue;

                        ConfigInfoDto configInfoDto = new()
                        {
                            Key = key,
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
                        string key = element.GetAttribute("name");
                        bool keyExist = _keyValues.Any(x => x.Key == key);
                        if (keyExist)
                            continue;

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
