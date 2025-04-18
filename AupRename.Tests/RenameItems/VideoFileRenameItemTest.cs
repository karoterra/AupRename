using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class VideoFileRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new AudioFileEffect
            {
                Filename = "test.wav"
            };
            var item = VideoFileRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FilenameIsEmpty()
        {
            var effect = new VideoFileEffect
            {
                Filename = ""
            };
            var item = VideoFileRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success()
        {
            var effect = new VideoFileEffect
            {
                Filename = "test.mp4"
            };
            var item = VideoFileRenameItem.CreateIfTarget(effect);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new VideoFileEffect
            {
                Filename = "test.mp4"
            };
            var item = new VideoFileRenameItem(effect);
            item.Rename("test_new.mp4");
            Assert.Equal("test_new.mp4", effect.Filename);
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new VideoFileEffect
            {
                Filename = "test.mp4"
            };
            var item = new VideoFileRenameItem(effect);
            item.Rename("test_new.mp4");
            item.Revert();
            Assert.Equal("test.mp4", effect.Filename);
        }
    }
}
