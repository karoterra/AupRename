using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class PartialFilterRenameItem(PartialFilterEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Name;
        public string OldName => _oldName;

        private readonly PartialFilterEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Name = newName;
        }

        public void Revert()
        {
            _effect.Name = _oldName;
        }

        public static PartialFilterRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is PartialFilterEffect pf
                && (
                    (pf.NameType == FigureNameType.File && !string.IsNullOrEmpty(pf.Filename))
                    || pf.NameType == FigureNameType.Figure
                )
            )
            {
                return new PartialFilterRenameItem(pf);
            }
            return null;
        }
    }
}
