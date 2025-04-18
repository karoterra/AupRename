using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AupRename.RenameItems
{
    public interface IRenameItem
    {
        // リネーム前の名前
        string OldName { get; }

        // リネーム実行
        void Rename(string newName);

        // 元に戻す
        void Revert();
    }
}
