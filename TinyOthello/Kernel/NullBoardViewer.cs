using System;
using System.Collections.Generic;
using System.Text;

namespace TinyOthello.Kernel {
    public class NullBoardViewer : IBoardViewer {

        #region IBoardViewer Members

        public void ShowBoard(Board board) {
            return;
        }

        #endregion
    }
}
