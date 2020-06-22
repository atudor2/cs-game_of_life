using GameOfLife.Boards;

namespace GameOfLife.Rules
{
    public interface IBoardRulesEngine<T>
    {
        IBoard<T> ApplyRulesToBoard(IBoard<T> board);
    }
}
