using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class ImageFileRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new VideoFileEffect
            {
                Filename = "test.mp4"
            };
            var item = ImageFileRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FilenameIsEmpty()
        {
            var effect = new ImageFileEffect
            {
                Filename = ""
            };
            var item = ImageFileRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = ImageFileRenameItem.CreateIfTarget(effect);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = new ImageFileRenameItem(effect);
            item.Rename("test_new.png");
            Assert.Equal("test_new.png", effect.Filename);
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = new ImageFileRenameItem(effect);
            item.Rename("test_new.png");
            item.Revert();
            Assert.Equal("test.png", effect.Filename);
        }
    }
}
