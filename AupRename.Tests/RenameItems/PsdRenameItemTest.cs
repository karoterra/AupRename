using AupRename.RenameItems;
using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AupRename.Tests.RenameItems
{
    public class PsdRenameItemTest : IClassFixture<EncodingFixture>
    {
        [Fact]
        public void Test_Rename()
        {
            var json = JArray.Parse("""
                [
                    {
                        "Image": {
                            "Version": 1,
                            "FilePath": "C:\\path\\to\\テスト.psd",
                            "Layer": {},
                            "PFV": {"Node": null, "FaviewNode": null}
                        },
                        "Tag": 676809393,
                        "Thumbnail": ""
                    }
                ]
                """);
            Assert.Equal(676809393, json[0]["Tag"]?.ToObject<int>());
            Assert.Equal(@"C:\path\to\テスト.psd", json[0]["Image"]?["FilePath"]?.ToObject<string>());

            var text = new TextEffect
            {
                Text = string.Join("\r\n",
                    "<?-- テスト.psd ",
                    "",
                    "o={ -- オプション設定",
                    "lipsync = 0    ,-- 口パク準備のレイヤー番号",
                    "mpslider = 0    ,-- 多目的スライダーのレイヤー番号",
                    "scene = 0    ,-- シーン番号",
                    "tag = 676809393    ,-- 識別用タグ",
                    "sendguard = 1    ,-- 「送る」誤送信保護",
                    "",
                    "-- 口パク準備のデフォルト設定",
                    "ls_locut = 100    ,-- ローカット",
                    "ls_hicut = 1000    ,-- ハイカット",
                    "ls_threshold = 20    ,-- しきい値",
                    "ls_sensitivity = 1    ,-- 感度",
                    "",
                    "-- 以下は書き換えないでください",
                    @"ptkf=""C:\\path\\to\\テスト.psd"",ptkl=""""}PSD,subobj=require(""PSDToolKit"").PSDState.init(obj,o)?>"
                )
            };
            var anm = new AnimationEffect
            {
                Name = "描画@PSD"
            };
            List<TimelineObject> objects = [ExEditHelper.CreateTimelineObject()];
            objects[0].Effects.Add(text);
            objects[0].Effects.Add(anm);

            var item = new PsdRenameItem(json[0], objects);

            item.Rename(@"C:\path\to\テスト_リネーム後.psd");

            Assert.Equal(@"C:\path\to\テスト_リネーム後.psd", json[0]["Image"]?["FilePath"]);

            Assert.Equal(string.Join("\r\n",
                    "<?-- テスト_リネーム後.psd ",
                    "",
                    "o={ -- オプション設定",
                    "lipsync = 0    ,-- 口パク準備のレイヤー番号",
                    "mpslider = 0    ,-- 多目的スライダーのレイヤー番号",
                    "scene = 0    ,-- シーン番号",
                    "tag = 676809393    ,-- 識別用タグ",
                    "sendguard = 1    ,-- 「送る」誤送信保護",
                    "",
                    "-- 口パク準備のデフォルト設定",
                    "ls_locut = 100    ,-- ローカット",
                    "ls_hicut = 1000    ,-- ハイカット",
                    "ls_threshold = 20    ,-- しきい値",
                    "ls_sensitivity = 1    ,-- 感度",
                    "",
                    "-- 以下は書き換えないでください",
                    @"ptkf=""C:\\path\\to\\テスト_リネーム後.psd"",ptkl=""""}PSD,subobj=require(""PSDToolKit"").PSDState.init(obj,o)?>"
                ),
                text.Text
            );
        }
    }
}
