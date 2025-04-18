using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class VideoFileRenameItem(VideoFileEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Filename;
        public string OldName => _oldName;

        private readonly VideoFileEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Filename = newName;
        }

        public void Revert()
        {
            _effect.Filename = _oldName;
        }

        public static VideoFileRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is VideoFileEffect video && !string.IsNullOrEmpty(video.Filename))
            {
                return new VideoFileRenameItem(video);
            }
            return null;
        }
    }
}
