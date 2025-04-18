using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class ShadowRenameItem(ShadowEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Filename;
        public string OldName => _oldName;

        private readonly ShadowEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Filename = newName;
        }

        public void Revert()
        {
            _effect.Filename = _oldName;
        }

        public static ShadowRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is ShadowEffect shadow && !string.IsNullOrEmpty(shadow.Filename))
            {
                return new ShadowRenameItem(shadow);
            }
            return null;
        }
    }
}
