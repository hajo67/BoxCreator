// Created by Hans-Jörg Schmid
// Licensed under MIT license

using System.Globalization;
using BoxCreator.Geometry;

namespace BoxCreator.BoxCreator;

internal sealed class ApplicationWindow : Form
{
    public ApplicationWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Size = new Size(580, 520);
        this.Text = "Box Creator";
        this.Padding = new Padding(10);

        var groupBox = new GroupBox()
        {
            Text = "Basic Box Settings",
            MinimumSize = new Size(340, 240),
            Padding = new Padding(10)
        };
        var tableLayout = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill
        };
        var createButton = new Button()
        {
            Text = "Create Box",
            Size = new Size(120, 40),
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom
        };

        createButton.Click += CreateBoxClicked;
        _units.SelectedIndexChanged += SelectedUnitsChanged;
        _units.Items.Add("Millimeter");
        _units.Items.Add("Inch");
        _units.SelectedIndex = 0;
        _boxType.SelectedIndexChanged += SelectedBoxTypeChanged;
        _boxType.Items.Add("Finger Joints");
        _boxType.SelectedIndex = 0;
        tableLayout.Controls.Add(CreateLabel("Units"), 0, 0);
        tableLayout.Controls.Add(_units, 1, 0);
        tableLayout.Controls.Add(CreateLabel("Box Type"), 0, 1);
        tableLayout.Controls.Add(_boxType, 1, 1);
        tableLayout.Controls.Add(CreateLabel("Box Length"), 0, 2);
        tableLayout.Controls.Add(_boxLength, 1, 2);
        tableLayout.Controls.Add(CreateLabel("Box Width"), 0, 3);
        tableLayout.Controls.Add(_boxWidth, 1, 3);
        tableLayout.Controls.Add(CreateLabel("Box Height"), 0, 4);
        tableLayout.Controls.Add(_boxHeight, 1, 4);
        tableLayout.Controls.Add(CreateLabel("Material Thickness"), 0, 5);
        tableLayout.Controls.Add(_materialThickness, 1, 5);

        this.Controls.Add(_mainLayout);
        _mainLayout.Controls.Add(groupBox, 0, 0);
        _mainLayout.Controls.Add(createButton, 0, 2);
        groupBox.Controls.Add(tableLayout);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _units.Focus();
    }

    private void SelectedUnitsChanged(object? sender, EventArgs e)
    {
        var changedUnit = _units.SelectedIndex == 0 ? Units.Millimeter : Units.Inch;

        if (changedUnit != _activeUnit)
        {
            _activeUnit = changedUnit;
            ExpressionNumberBox[] allTextBoxes =
                [
                    _boxLength,
                    _boxWidth,
                    _boxHeight,
                    _materialThickness,
                    _fingerLength,
                    _endmillDiameter
                ];

            foreach (var textBox in allTextBoxes)
            {
                textBox.ActiveUnit = _activeUnit;
            }
        }
    }

    private void SelectedBoxTypeChanged(object? sender, EventArgs e)
    {
        var control = _mainLayout.GetControlFromPosition(0, 1);

        if (control is not null)
        {
            _mainLayout.Controls.Remove(control);
        }

        var groupBox = new GroupBox()
        {
            Text = "Finger Joint Settings",
            MinimumSize = new Size(340, 140),
            Padding = new Padding(10)
        };
        var tableLayout = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill
        };

        _cornerRelief.Items.Add("None");
        _cornerRelief.Items.Add("Standard Dog Bone");
        _cornerRelief.Items.Add("Minimal Dog Bone");
        _cornerRelief.Items.Add("Hidden");
        _cornerRelief.SelectedIndex = 0;
        tableLayout.Controls.Add(CreateLabel("Min. Finger Length"), 0, 0);
        tableLayout.Controls.Add(_fingerLength, 1, 0);
        tableLayout.Controls.Add(CreateLabel("End Mill Diameter"), 0, 1);
        tableLayout.Controls.Add(_endmillDiameter, 1, 1);
        tableLayout.Controls.Add(CreateLabel("Corner Relief"), 0, 2);
        tableLayout.Controls.Add(_cornerRelief, 1, 2);

        groupBox.Controls.Add(tableLayout);
        _mainLayout.Controls.Add(groupBox, 0, 1);
    }

    private void CreateBoxClicked(object? sender, EventArgs e)
    {
        var lengthOk = GetFloatValueFromTextBox(_boxLength, out var boxLength);
        var widthOk = GetFloatValueFromTextBox(_boxWidth, out var boxWidth);
        var heightOk = GetFloatValueFromTextBox(_boxHeight, out var boxHeight);
        var thicknessOk = GetFloatValueFromTextBox(_materialThickness, out var thickness);

        if (lengthOk && widthOk && heightOk && thicknessOk)
        {
            var basicBoxParameters = new BasicBoxParameters(boxLength,
                boxWidth,
                boxHeight,
                thickness);

            if (_boxType.SelectedIndex == 0)
            {
                var fingerLengthOk = GetFloatValueFromTextBox(_fingerLength, out var fingerLength);
                var endMillDiameterOk = GetFloatValueFromTextBox(_endmillDiameter, out var endmillDiameter);

                if (fingerLengthOk && endMillDiameterOk)
                {
                    var cornerReliefType = ToCornerRelief(_cornerRelief.SelectedIndex);
                    var fingerJointParameters = new FingerJointParameters(fingerLength,
                        endmillDiameter,
                        cornerReliefType);
                    var fingerJointBoxCreator = new FingerJointBoxCreator()
                    {
                        BasicBoxParameters = basicBoxParameters,
                        FingerJointParameters = fingerJointParameters
                    };
                }
            }
        }
    }

    private static Label CreateLabel(string text)
    {
        return new Label()
        {
            Text = text,
            Width = 160
        };
    }

    private bool GetFloatValueFromTextBox(ExpressionNumberBox textBox, out float floatValue)
    {
        if (!textBox.EvaluationSucceeded)
        {
            textBox.Focus();
            ShowErrorMessage(
                $"Failed to evaluate expression:\n'{textBox.Text}' ->\n{textBox.EvaluationErrorMessage}",
                "Error evaluating value");
            floatValue = float.NaN;
            return false;
        }

        var success = float.TryParse(textBox.EvaluatedText,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out floatValue);

        if (!success)
        {
            textBox.Focus();
            ShowErrorMessage(
                $"Failed to convert text to number:\n'{textBox.Text}' ->\n'{textBox.EvaluatedText}'",
                "Error converting text to number");
            floatValue = float.NaN;
            return false;
        }

        return success;
    }

    private void ShowErrorMessage(string message, string title)
    {
        MessageBox.Show(Control.FromHandle(this.Handle),
            message,
            title,
            MessageBoxButtons.OK,
            MessageBoxIcon.Stop);
    }

    private static CornerReliefs ToCornerRelief(int comboBoxSelectedIndex) => comboBoxSelectedIndex switch
    {
        0 => CornerReliefs.None,
        1 => CornerReliefs.StandardDogBone,
        2 => CornerReliefs.MinimalDogBone,
        3 => CornerReliefs.Hidden,
        _ => throw new ArgumentOutOfRangeException(nameof(comboBoxSelectedIndex), $"Not expected value: {comboBoxSelectedIndex}")
    };

    private const int INPUT_CONTROL_WIDTH = 140;
    private Units _activeUnit = Units.Millimeter;
    private readonly TableLayoutPanel _mainLayout = new() { Dock = DockStyle.Fill };
    private readonly ComboBox _units = new() { Width = INPUT_CONTROL_WIDTH };
    private readonly ComboBox _boxType = new() { Width = INPUT_CONTROL_WIDTH };
    private readonly ExpressionNumberBox _boxLength = new() { Text = "400 mm", Width = INPUT_CONTROL_WIDTH, HighlightEvaluationErrors = true };
    private readonly ExpressionNumberBox _boxWidth = new() { Text = "300 mm", Width = INPUT_CONTROL_WIDTH, HighlightEvaluationErrors = true };
    private readonly ExpressionNumberBox _boxHeight = new() { Text = "100 mm", Width = INPUT_CONTROL_WIDTH, HighlightEvaluationErrors = true };
    private readonly ExpressionNumberBox _materialThickness = new() { Text = "12 mm", Width = INPUT_CONTROL_WIDTH, HighlightEvaluationErrors = true };
    private readonly ExpressionNumberBox _fingerLength = new() { Text = "20 mm", Width = INPUT_CONTROL_WIDTH, HighlightEvaluationErrors = true };
    private readonly ExpressionNumberBox _endmillDiameter = new() { Text = "1/8 \"", Width = INPUT_CONTROL_WIDTH, HighlightEvaluationErrors = true };
    private readonly ComboBox _cornerRelief = new() { Width = INPUT_CONTROL_WIDTH };
}
