// Created by Hans-Jörg Schmid
// Licensed under MIT license

using System.Drawing;

namespace BoxCreator.BoxCreator;

internal sealed class ApplicationWindow : Form
{
    public ApplicationWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Size = new Size(580, 480);
        this.Text = "Box Creator";
        this.Padding = new Padding(10);

        var groupBox = new GroupBox()
        {
            Text = "Basic Box Settings",
            MinimumSize = new Size(320, 210),
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
        _boxType.SelectedIndexChanged += SelectedBoxTypeChanged;
        _boxType.Items.Add("Finger Joints");
        _boxType.SelectedIndex = 0;
        tableLayout.Controls.Add(CreateLabel("Box Type"), 0, 0);
        tableLayout.Controls.Add(_boxType, 1, 0);
        tableLayout.Controls.Add(CreateLabel("Box Length"), 0, 1);
        tableLayout.Controls.Add(_boxLength, 1, 1);
        tableLayout.Controls.Add(CreateLabel("Box Width"), 0, 2);
        tableLayout.Controls.Add(_boxWidth, 1, 2);
        tableLayout.Controls.Add(CreateLabel("Box Height"), 0, 3);
        tableLayout.Controls.Add(_boxHeight, 1, 3);
        tableLayout.Controls.Add(CreateLabel("Material Thickness"), 0, 4);
        tableLayout.Controls.Add(_materialThickness, 1, 4);

        this.Controls.Add(_mainLayout);
        _mainLayout.Controls.Add(groupBox, 0, 0);
        _mainLayout.Controls.Add(createButton, 0, 2);
        groupBox.Controls.Add(tableLayout);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _boxType.Focus();
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
            MinimumSize = new Size(320, 110),
            Padding = new Padding(10)
        };
        var tableLayout = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill
        };

        tableLayout.Controls.Add(CreateLabel("Min. Finger Length"), 0, 0);
        tableLayout.Controls.Add(_fingerLength, 1, 0);
        tableLayout.Controls.Add(CreateLabel("End Mill Diameter"), 0, 1);
        tableLayout.Controls.Add(_endmillDiameter, 1, 1);

        groupBox.Controls.Add(tableLayout);
        _mainLayout.Controls.Add(groupBox, 0, 1);
    }

    private void CreateBoxClicked(object? sender, EventArgs e)
    {
        MessageBox.Show("Create Box");
    }

    private static Label CreateLabel(string text)
    {
        return new Label()
        {
            Text = text,
            Width = 160
        };
    }

    private readonly TableLayoutPanel _mainLayout = new() { Dock = DockStyle.Fill };
    private readonly ComboBox _boxType = new();
    private readonly TextBox _boxLength = new() { Text = "400" };
    private readonly TextBox _boxWidth = new() { Text = "300" };
    private readonly TextBox _boxHeight = new() { Text = "100" };
    private readonly TextBox _materialThickness = new() { Text = "12" };
    private readonly TextBox _fingerLength = new() { Text = "10" };
    private readonly TextBox _endmillDiameter = new() { Text = "3" };
}
