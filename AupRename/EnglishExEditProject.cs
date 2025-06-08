using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karoterra.AupDotNet;
using Karoterra.AupDotNet.ExEdit;

namespace AupRename
{
    /// <summary>
    /// 英語版拡張編集に対応するためのクラス
    /// </summary>
    public class EnglishExEditProject : ExEditProject
    {
        public EnglishExEditProject(RawFilterProject rawFilter, IEffectFactory? effectFactory = null) : base(rawFilter, effectFactory)
        {
            Name = "Advanced Editing";
        }
    }
}
