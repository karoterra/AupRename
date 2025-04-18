using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class BorderRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = BorderRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FilenameIsEmpty()
        {
            var effect = new BorderEffect
            {
                Filename = ""
            };
            var item = BorderRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success()
        {
            var effect = new BorderEffect
            {
                Filename = "test.png"
            };
            var item = BorderRenameItem.CreateIfTarget(effect);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new BorderEffect
            {
                Filename = "test.png"
            };
            var item = new BorderRenameItem(effect);
            item.Rename("test_new.png");
            Assert.Equal("test_new.png", effect.Filename);
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new BorderEffect
            {
                Filename = "test.png"
            };
            var item = new BorderRenameItem(effect);
            item.Rename("test_new.png");
            item.Revert();
            Assert.Equal("test.png", effect.Filename);
        }
    }
}
