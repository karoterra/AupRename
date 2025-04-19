using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class ShadowRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = ShadowRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FilenameIsEmpty()
        {
            var effect = new ShadowEffect
            {
                Filename = ""
            };
            var item = ShadowRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success()
        {
            var effect = new ShadowEffect
            {
                Filename = "test.png"
            };
            var item = ShadowRenameItem.CreateIfTarget(effect);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new ShadowEffect
            {
                Filename = "test.png"
            };
            var item = new ShadowRenameItem(effect);
            item.Rename("test_new.png");
            Assert.Equal("test_new.png", effect.Filename);
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new ShadowEffect
            {
                Filename = "test.png"
            };
            var item = new ShadowRenameItem(effect);
            item.Rename("test_new.png");
            item.Revert();
            Assert.Equal("test.png", effect.Filename);
        }
    }
}
