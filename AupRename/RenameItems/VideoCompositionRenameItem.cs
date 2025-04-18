using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class VideoCompositionRenameItem(VideoCompositionEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Filename;
        public string OldName => _oldName;

        private readonly VideoCompositionEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Filename = newName;
        }

        public void Revert()
        {
            _effect.Filename = _oldName;
        }

        public static VideoCompositionRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is VideoCompositionEffect video && !string.IsNullOrEmpty(video.Filename))
            {
                return new VideoCompositionRenameItem(video);
            }
            return null;
        }
    }
}
