using System;
using System.Collections.Generic;
using System.Text;

namespace TinyOthello.Kernel {
    public interface IPlayer {
        void PlayOneMove(Board board);
        Color Color { get; }
    }
}
