using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class MaskRenameItem(MaskEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Name;
        public string OldName => _oldName;

        private readonly MaskEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Name = newName;
        }

        public void Revert()
        {
            _effect.Name = _oldName;
        }

        public static MaskRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is MaskEffect mask
                && (
                    (mask.NameType == FigureNameType.File && !string.IsNullOrEmpty(mask.Filename))
                    || mask.NameType == FigureNameType.Figure
                )
            )
            {
                return new MaskRenameItem(mask);
            }
            return null;
        }
    }
}
