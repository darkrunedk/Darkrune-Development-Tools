using Darkrune_Development_Tools.Commands;

namespace Darkrune_Development_Tools.ViewModel
{
    public class MainViewModel
    {
        private readonly CopyCommand _copyCommand = new();

        public MainViewModel()
        {
            
        }

        public CopyCommand CopyCommand
        {
            get { return _copyCommand; }
        }
    }
}
