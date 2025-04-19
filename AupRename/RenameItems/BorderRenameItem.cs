using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class BorderRenameItem(BorderEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Filename;
        public string OldName => _oldName;

        private readonly BorderEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Filename = newName;
        }

        public void Revert()
        {
            _effect.Filename = _oldName;
        }

        public static BorderRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is BorderEffect border && !string.IsNullOrEmpty(border.Filename))
            {
                return new BorderRenameItem(border);
            }
            return null;
        }
    }
}
