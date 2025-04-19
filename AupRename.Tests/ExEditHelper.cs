using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AupRename.Tests
{
    internal class ExEditHelper
    {
        public static TimelineObject CreateTimelineObject()
        {
            var data = new byte[0x5C8];
            for (int i = 0; i < TimelineObject.MaxEffect; i++)
            {
                int typeIndex = -1;
                typeIndex.ToBytes().CopyTo(data, 0x54 + i * 12);
            }
            List<EffectType> effectTypes = [];
            return new TimelineObject(data, 0, effectTypes);
        }
    }
}
