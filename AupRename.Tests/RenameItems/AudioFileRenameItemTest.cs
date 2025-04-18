using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class AudioFileRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = AudioFileRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FilenameIsEmpty()
        {
            var effect = new AudioFileEffect
            {
                Filename = ""
            };
            var item = AudioFileRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success()
        {
            var effect = new AudioFileEffect
            {
                Filename = "test.wav"
            };
            var item = AudioFileRenameItem.CreateIfTarget(effect);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new AudioFileEffect
            {
                Filename = "test.wav"
            };
            var item = new AudioFileRenameItem(effect);
            item.Rename("test_new.wav");
            Assert.Equal("test_new.wav", effect.Filename);
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new AudioFileEffect
            {
                Filename = "test.wav"
            };
            var item = new AudioFileRenameItem(effect);
            item.Rename("test_new.wav");
            item.Revert();
            Assert.Equal("test.wav", effect.Filename);
        }
    }
}
