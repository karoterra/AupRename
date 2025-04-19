using System.Collections.Generic;
using Karoterra.AupDotNet;
using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;
using Karoterra.AupDotNet.Extensions;

namespace AupRename.RenameItems
{
    public class ScriptFileRenameItem(ScriptFileEffect effect) : IRenameItem
    {
        private readonly string _oldName = effect.Params?["file"][1..^1].Replace(@"\\", @"\") ?? "";
        public string OldName => _oldName;

        private readonly ScriptFileEffect _effect = effect;

        public void Rename(string newName)
        {
            if (_effect.Params == null) return;
            _effect.Params["file"] = '"' + newName.Replace(@"\", @"\\") + '"';
            if (_effect.BuildParams().GetSjisByteCount() >= ScriptFileEffect.MaxParamsLength)
            {
                throw new MaxByteCountOfStringException();
            }
        }

        public void Revert()
        {
            Rename(_oldName);
        }

        public static ScriptFileRenameItem? CreateIfTarget(Effect effect)
        {
            if (
                effect is ScriptFileEffect script
                && !string.IsNullOrEmpty(script.Params?.GetValueOrDefault("file"))
            )
            {
                return new ScriptFileRenameItem(script);
            }
            return null;
        }
    }
}
