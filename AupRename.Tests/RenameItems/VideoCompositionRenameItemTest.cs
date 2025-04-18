using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class VideoCompositionRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = VideoCompositionRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FilenameIsEmpty()
        {
            var effect = new VideoCompositionEffect
            {
                Filename = ""
            };
            var item = VideoCompositionRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success()
        {
            var effect = new VideoCompositionEffect
            {
                Filename = "test.mp4"
            };
            var item = VideoCompositionRenameItem.CreateIfTarget(effect);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new VideoCompositionEffect
            {
                Filename = "test.mp4"
            };
            var item = new VideoCompositionRenameItem(effect);
            item.Rename("test_new.mp4");
            Assert.Equal("test_new.mp4", effect.Filename);
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new VideoCompositionEffect
            {
                Filename = "test.mp4"
            };
            var item = new VideoCompositionRenameItem(effect);
            item.Rename("test_new.mp4");
            item.Revert();
            Assert.Equal("test.mp4", effect.Filename);
        }
    }
}
