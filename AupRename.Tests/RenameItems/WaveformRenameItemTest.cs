using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class WaveformRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = WaveformRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FilenameIsEmpty()
        {
            var effect = new WaveformEffect
            {
                Filename = ""
            };
            var item = WaveformRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success()
        {
            var effect = new WaveformEffect
            {
                Filename = "test.wav"
            };
            var item = WaveformRenameItem.CreateIfTarget(effect);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new WaveformEffect
            {
                Filename = "test.wav"
            };
            var item = new WaveformRenameItem(effect);
            item.Rename("test_new.wav");
            Assert.Equal("test_new.wav", effect.Filename);
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new WaveformEffect
            {
                Filename = "test.wav"
            };
            var item = new WaveformRenameItem(effect);
            item.Rename("test_new.wav");
            item.Revert();
            Assert.Equal("test.wav", effect.Filename);
        }
    }
}
