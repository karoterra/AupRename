using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class ImageFileRenameItem(ImageFileEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Filename;
        public string OldName => _oldName;

        private readonly ImageFileEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Filename = newName;
        }

        public void Revert()
        {
            _effect.Filename = _oldName;
        }

        public static ImageFileRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is ImageFileEffect image && !string.IsNullOrEmpty(image.Filename))
            {
                return new ImageFileRenameItem(image);
            }
            return null;
        }
    }
}
