using GameOfLife.Boards;

namespace GameOfLife.Rules
{
    public interface IBoardRulesEngine<T>
    {
        IBoard<T> ExecuteRules(IBoard<T> board);
    }
}
