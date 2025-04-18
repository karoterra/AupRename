using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class MaskRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = MaskRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_NameIsEmpty()
        {
            var effect = new MaskEffect
            {
                Name = "",
            };
            var item = MaskRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.BuiltIn, effect.NameType);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FilenameIsEmpty()
        {
            var effect = new MaskEffect
            {
                Filename = "",
            };
            var item = MaskRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.File, effect.NameType);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success_Figure()
        {
            var effect = new MaskEffect
            {
                Name = "test",
            };
            var item = MaskRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.Figure, effect.NameType);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success_File()
        {
            var effect = new MaskEffect
            {
                Filename = @"C:\test.png",
            };
            var item = MaskRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.File, effect.NameType);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new MaskEffect
            {
                Name = "test"
            };
            var item = new MaskRenameItem(effect);
            item.Rename("test_new");
            Assert.Equal("test_new", effect.Name);
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new MaskEffect
            {
                Name = "test"
            };
            var item = new MaskRenameItem(effect);
            item.Rename("test_new");
            item.Revert();
            Assert.Equal("test", effect.Name);
        }
    }
}
