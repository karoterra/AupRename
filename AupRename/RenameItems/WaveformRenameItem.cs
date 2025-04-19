using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class WaveformRenameItem(WaveformEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Filename;
        public string OldName => _oldName;

        private readonly WaveformEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Filename = newName;
        }

        public void Revert()
        {
            _effect.Filename = _oldName;
        }

        public static WaveformRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is WaveformEffect wave && !string.IsNullOrEmpty(wave.Filename))
            {
                return new WaveformRenameItem(wave);
            }
            return null;
        }
    }
}
