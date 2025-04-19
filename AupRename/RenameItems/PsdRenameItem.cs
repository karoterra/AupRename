using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AupRename.RenameItems
{
    public partial class PsdRenameItem : IRenameItem
    {
        public string OldName { get; }

        public JToken PsdImage { get; }
        private readonly JToken _image;
        public int Tag { get; }

        public List<TextEffect> Texts { get; } = [];
        public List<string> OldTexts { get; } = [];

        [GeneratedRegex(@"tag\s*=\s*(\d+)")]
        private static partial Regex _regTag();

        [GeneratedRegex(@"ptkf\s*=\s*""([^""]+)""")]
        private static partial Regex _regPtkf();

        [GeneratedRegex(@"<\?--.*\r?\n")]
        private static partial Regex _regFilenameComment();

        public PsdRenameItem(JToken psdImage, List<TimelineObject> objects)
        {
            PsdImage = psdImage;
            var image = psdImage["Image"];
            if (image == null)
            {
                throw new ArgumentException("Imageが見つかりませんでした", nameof(psdImage));
            }
            _image = image;
            var tag = psdImage["Tag"];
            if (tag == null)
            {
                throw new ArgumentException("Tagが見つかりませんでした", nameof(psdImage));
            }
            Tag = tag.ToObject<int>();
            var tagStr = Tag.ToString();

            OldName = _image["FilePath"]?.ToObject<string>() ?? "";
            foreach (var obj in objects)
            {
                bool hasPsd = obj.Effects.Any(x => x is AnimationEffect anm && anm.Name == "描画@PSD");
                if (hasPsd && obj.Effects.Count > 0 && obj.Effects[0] is TextEffect text)
                {
                    var tagMatch = _regTag().Match(text.Text);
                    if (!tagMatch.Success || tagMatch.Groups[1].Value != tagStr)
                    {
                        continue;
                    }
                    Texts.Add(text);
                    OldTexts.Add(text.Text);
                }
            }
        }

        public void Rename(string newName)
        {
            _image["FilePath"] = newName;

            var escapedNewName = newName.Replace(@"\", @"\\");
            var newFilename = Path.GetFileName(newName);
            foreach (var text in Texts)
            {
                string newText = _regPtkf().Replace(text.Text, $@"ptkf=""{escapedNewName}""");
                newText = _regFilenameComment().Replace(newText, $"<?-- {newFilename} \r\n");
                text.Text = newText;
            }
        }

        public void Revert()
        {
            _image["FilePath"] = OldName;

            foreach (var (text, old) in Texts.Zip(OldTexts))
            {
                text.Text = old;
            }
        }
    }
}
