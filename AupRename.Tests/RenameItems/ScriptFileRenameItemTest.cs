using AupRename.RenameItems;
using Karoterra.AupDotNet;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class ScriptFileRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = ScriptFileRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_ParamsIsNull()
        {
            var effect = new AnimationEffect
            {
                Params = null
            };
            var item = ScriptFileRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_ParamsIsEmpty()
        {
            var effect = new AnimationEffect
            {
                Params = { }
            };
            var item = ScriptFileRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FileIsEmpty()
        {
            var effect = new AnimationEffect
            {
                Params = new() { { "file", "" } }
            };
            var item = ScriptFileRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success()
        {
            var effect = new AnimationEffect
            {
                Params = new() { { "file", @"""C:\\test.txt""" } }
            };
            var item = ScriptFileRenameItem.CreateIfTarget(effect);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_OldName()
        {
            var effect = new AnimationEffect
            {
                Params = new() { { "file", @"""C:\\test.txt""" } }
            };
            var item = new ScriptFileRenameItem(effect);
            Assert.Equal(@"C:\test.txt", item.OldName);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new AnimationEffect
            {
                Params = new() { { "file", @"""C:\\test.txt""" } }
            };
            var item = new ScriptFileRenameItem(effect);
            item.Rename(@"C:\test_new.txt");
            Assert.Equal(@"""C:\\test_new.txt""", effect.Params?["file"]);
        }

        [Fact]
        public void Test_Rename_MaxByteCountOfStringException()
        {
            var effect = new AnimationEffect
            {
                Params = new() { { "file", @"""C:\\test.txt""" } }
            };
            var item = new ScriptFileRenameItem(effect);
            int charCount = ScriptFileEffect.MaxParamsLength
                - 7     // file=""
                - 4     // C:\\
                - 4     // .txt
                ;
            string longPath = @"C:\" + new string('a', charCount) + ".txt";
            Assert.Throws<MaxByteCountOfStringException>(() => item.Rename(longPath));
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new AnimationEffect
            {
                Params = new() { { "file", @"""C:\\test.txt""" } }
            };
            var item = new ScriptFileRenameItem(effect);
            item.Rename(@"C:\test_new.txt");
            item.Revert();
            Assert.Equal(@"""C:\\test.txt""", effect.Params?["file"]);
        }
    }
}
