using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename.Tests.RenameItems
{
    public class FigureRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_CreateIfTarget_InvalidEffect()
        {
            var effect = new ImageFileEffect
            {
                Filename = "test.png"
            };
            var item = FigureRenameItem.CreateIfTarget(effect);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_NameIsEmpty()
        {
            var effect = new FigureEffect
            {
                Name = "",
            };
            var item = FigureRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.BuiltIn, effect.NameType);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_FilenameIsEmpty()
        {
            var effect = new FigureEffect
            {
                Filename = "",
            };
            var item = FigureRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.File, effect.NameType);
            Assert.Null(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success_Figure()
        {
            var effect = new FigureEffect
            {
                Name = "test",
            };
            var item = FigureRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.Figure, effect.NameType);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_CreateIfTarget_Success_File()
        {
            var effect = new FigureEffect
            {
                Filename = @"C:\test.png",
            };
            var item = FigureRenameItem.CreateIfTarget(effect);
            Assert.Equal(FigureNameType.File, effect.NameType);
            Assert.NotNull(item);
        }

        [Fact]
        public void Test_Rename()
        {
            var effect = new FigureEffect
            {
                Name = "test"
            };
            var item = new FigureRenameItem(effect);
            item.Rename("test_new");
            Assert.Equal("test_new", effect.Name);
        }

        [Fact]
        public void Test_Revert()
        {
            var effect = new FigureEffect
            {
                Name = "test"
            };
            var item = new FigureRenameItem(effect);
            item.Rename("test_new");
            item.Revert();
            Assert.Equal("test", effect.Name);
        }
    }
}
