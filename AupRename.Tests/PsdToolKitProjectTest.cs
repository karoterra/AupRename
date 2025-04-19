using Karoterra.AupDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AupRename.Tests
{
    public class PsdToolKitProjectTest
    {
        [Fact]
        public void Test_DumpData()
        {
            var json = string.Join("",
                "[{",
                "\"Image\":{",
                    "\"Version\":1,",
                    @"""FilePath"":""C:\\path\\to\\テスト.psd"",",
                    "\"Layer\":{},",
                    "\"PFV\":{\"Node\":null,\"FaviewNode\":null}",
                "},",
                "\"Tag\":676809393,",
                "\"Thumbnail\":\"\"",
                "}]\n"
            );
            var filter = new PsdToolKitProject(new RawFilterProject("PSDToolKit", Encoding.UTF8.GetBytes(json)));
            var dumpedJson = Encoding.UTF8.GetString(filter.DumpData());
            Assert.Single(filter.Images);
            Assert.Equal(json, dumpedJson);
        }
    }
}
