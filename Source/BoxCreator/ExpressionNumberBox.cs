// Created by Hans-Jörg Schmid
// Licensed under MIT license


using System.Globalization;

namespace BoxCreator.BoxCreator;

internal sealed class ExpressionNumberBox : TextBox
{
    public bool HighlightEvaluationErrors { get; set; }
    public Units ActiveUnit
    {
        get => _activeUnit;
        set
        {
            if (_activeUnit != value)
            {
                _activeUnit = value;
                _expressionSolver = new ExpressionSolver(_activeUnit);
                _lastEvaluatedText = null;
                EvaluateText();
            }
        }
    }
    public string EvaluatedText
    {
        get
        {
            EvaluateText();
            return _evaluatedText;
        }
    }
    public string EvaluationErrorMessage
    {
        get
        {
            EvaluateText();
            return _evaluationError is null ? string.Empty : _evaluationError;
        }
    }

    public bool EvaluationSucceeded
    {
        get
        {
            EvaluateText();
            return _evaluationSucceded;
        }
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        EvaluateText();
    }

    private void EvaluateText()
    {
        if (_lastEvaluatedText is null ||
            _lastEvaluatedText != this.Text)
        {
            _lastEvaluatedText = this.Text;
            (_evaluatedText, _evaluationError) = _expressionSolver.SolveExpression(this.Text);
            _evaluationSucceded = _evaluationError is null;
            _toolTip.SetToolTip(this, _evaluationSucceded ? _evaluatedText : _evaluationError);

            if (HighlightEvaluationErrors)
            {
                var isNumber = float.TryParse(_evaluatedText,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var _);
                if (_evaluationSucceded && isNumber)
                {
                    BackColor = Color.White;
                }
                else
                {
                    BackColor = Color.Orange;
                }
            }
        }
    }

    private ToolTip _toolTip = new() { AutomaticDelay = 50, AutoPopDelay = 2000 };
    private Units _activeUnit = Units.Millimeter;
    private ExpressionSolver _expressionSolver = new(Units.Millimeter);
    private string? _lastEvaluatedText;
    private string _evaluatedText = string.Empty;
    private string? _evaluationError;
    private bool _evaluationSucceded;
}
