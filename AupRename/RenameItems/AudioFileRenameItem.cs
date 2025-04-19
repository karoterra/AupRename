using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.RenameItems
{
    public class AudioFileRenameItem(AudioFileEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Filename;
        public string OldName => _oldName;

        private readonly AudioFileEffect _effect = effect;

        public void Rename(string newName)
        {
            _effect.Filename = newName;
        }

        public void Revert()
        {
            _effect.Filename = _oldName;
        }

        public static AudioFileRenameItem? CreateIfTarget(Effect effect)
        {
            if (effect is AudioFileEffect audio && !string.IsNullOrEmpty(audio.Filename))
            {
                return new AudioFileRenameItem(audio);
            }
            return null;
        }
    }
}
