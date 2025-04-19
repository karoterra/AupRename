using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class DisplacementRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = DisplacementRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_NameIsEmpty()
        {
            var effect = new DisplacementEffect
            {
                Name = "",
            };
            var item = DisplacementRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.BuiltIn, effect.NameType);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FilenameIsEmpty()
        {
            var effect = new DisplacementEffect
            {
                Filename = "",
            };
            var item = DisplacementRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.File, effect.NameType);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success_Figure()
        {
            var effect = new DisplacementEffect
            {
                Name = "test",
            };
            var item = DisplacementRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.Figure, effect.NameType);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success_File()
        {
            var effect = new DisplacementEffect
            {
                Filename = @"C:\test.png",
            };
            var item = DisplacementRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.File, effect.NameType);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new DisplacementEffect
            {
                Name = "test"
            };
            var item = new DisplacementRenameItem(effect);
            item.Rename("test_new");
            Assert.Equal("test_new", effect.Name);
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new DisplacementEffect
            {
                Name = "test"
            };
            var item = new DisplacementRenameItem(effect);
            item.Rename("test_new");
            item.Revert();
            Assert.Equal("test", effect.Name);
        }
    }
}
