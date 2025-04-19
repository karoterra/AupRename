using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class ImageCompositionRenameItem(ImageCompositionEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Filename;
        public string OldName => _oldName;

        private readonly ImageCompositionEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Filename = newName;
        }

        public void Revert()
        {
            _effect.Filename = _oldName;
        }

        public static ImageCompositionRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is ImageCompositionEffect image && !string.IsNullOrEmpty(image.Filename))
            {
                return new ImageCompositionRenameItem(image);
            }
            return null;
        }
    }
}
