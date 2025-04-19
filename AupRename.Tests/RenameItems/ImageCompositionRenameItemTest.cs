using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class ImageCompositionRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = ImageCompositionRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FilenameIsEmpty()
        {
            var effect = new ImageCompositionEffect
            {
                Filename = ""
            };
            var item = ImageCompositionRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success()
        {
            var effect = new ImageCompositionEffect
            {
                Filename = "test.png"
            };
            var item = ImageCompositionRenameItem.CreateIfTarget(effect);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new ImageCompositionEffect
            {
                Filename = "test.png"
            };
            var item = new ImageCompositionRenameItem(effect);
            item.Rename("test_new.png");
            Assert.Equal("test_new.png", effect.Filename);
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new ImageCompositionEffect
            {
                Filename = "test.png"
            };
            var item = new ImageCompositionRenameItem(effect);
            item.Rename("test_new.png");
            item.Revert();
            Assert.Equal("test.png", effect.Filename);
        }
    }
}
