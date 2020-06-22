using GameOfLife.Rules;

namespace GameOfLife
{
   public static class ExtensionMethods
    {
        public static bool IsAlive(this ConwayCellState self) => self == ConwayCellState.Live;
    }
}
