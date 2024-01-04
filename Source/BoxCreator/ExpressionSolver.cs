// Created by Hans-Jörg Schmid
// Licensed under MIT license

using ExpressionSolver;

namespace BoxCreator.BoxCreator;

internal sealed class ExpressionSolver
{
    public ExpressionSolver(Units activeUnit)
    {
        _activeUnit = activeUnit;
    }

    public (string Result, string? ErrorMessage) SolveExpression(string expression)
    {
        var unitsSolvedString = ReplaceInchConversions(expression);
        unitsSolvedString = ReplaceMmConversions(unitsSolvedString);

        try
        {
            return (_solver.Solve(unitsSolvedString), null);
        }
        catch (Exception e)
        {
            return (expression, e.Message);
        }
    }

    private string ReplaceInchConversions(string expression)
    {
        var inchReplacement = _activeUnit == Units.Inch ? "" : " * 25.4 ";

        return expression
            .Replace("\"", inchReplacement)
            .Replace("inch", inchReplacement, StringComparison.OrdinalIgnoreCase);
    }

    private string ReplaceMmConversions(string expression)
    {
        var mmReplacement = _activeUnit == Units.Millimeter ? "" : " / 25.4 ";

        return expression
            .Replace("mm", mmReplacement, StringComparison.OrdinalIgnoreCase);
    }

    private Units _activeUnit;
    private Solver _solver = new();
}
