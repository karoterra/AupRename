using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class FigureRenameItem(FigureEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Name;
        public string OldName => _oldName;

        private readonly FigureEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Name = newName;
        }

        public void Revert()
        {
            _effect.Name = _oldName;
        }

        public static FigureRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is FigureEffect figure
                && (
                    (figure.NameType == FigureNameType.File && !string.IsNullOrEmpty(figure.Filename))
                    || figure.NameType == FigureNameType.Figure
                )
            )
            {
                return new FigureRenameItem(figure);
            }
            return null;
        }
    }
}
