using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class DisplacementRenameItem(DisplacementEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Name;
        public string OldName => _oldName;

        private readonly DisplacementEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Name = newName;
        }

        public void Revert()
        {
            _effect.Name = _oldName;
        }

        public static DisplacementRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is DisplacementEffect disp
                && (
                    (disp.NameType == FigureNameType.File && !string.IsNullOrEmpty(disp.Filename))
                    || disp.NameType == FigureNameType.Figure
                )
            )
            {
                return new DisplacementRenameItem(disp);
            }
            return null;
        }
    }
}
