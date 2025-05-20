using Darkrune_Development_Tools.Core.Models;

namespace Darkrune_Development_Tools.Core.ConfigHandlers
{
    public abstract class ConfigHandlerBase
    {
        public abstract List<ConfigInfoDto> ProcessFile(string filePath);
    }
}
