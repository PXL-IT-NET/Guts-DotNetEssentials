using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;
using NUnit.Framework;

namespace Exercise09.Tests;

[ExerciseTestFixture("dotNet1", "H13", "Exercise09", @"Exercise09\MainWindow.xaml;Exercise09\MainWindow.xaml.cs"), 
 Apartment(ApartmentState.STA)]
public class MainWindowTests
{
    private TestWindow<MainWindow> _window;
    private ComboBox _colorComboBox;
    private Label _colorLabel;

    [SetUp]
    public void Setup()
    {
        _window = new TestWindow<MainWindow>();

        _colorComboBox = _window.GetUIElements<ComboBox>().FirstOrDefault();
        _colorLabel = _window.GetUIElements<Label>().FirstOrDefault();
    }

    [TearDown]
    public void TearDown()
    {
        _window.Dispose();
    }

    [MonitoredTest("Should all controls"), Order(1)]
    public void _1_ShouldHaveAllControls()
    {
        AssertHasComboBoxAndLabel();
    }

    [MonitoredTest("The combobox should have a ComboBoxItem for each color"), Order(2)]
    public void _2_ComboBoxShouldHaveAComboBoxItemForEachColor()
    {
        AssertHasComboBoxAndLabel();
        Assert.That(_colorComboBox.Items.Count, Is.EqualTo(3), () => "The combobox should have exactly 3 items.");
        for (var index = 0; index < _colorComboBox.Items.Count; index++)
        {
            var item = _colorComboBox.Items[index];
            var comboBoxItem = item as ComboBoxItem;
            Assert.That(comboBoxItem, Is.Not.Null,
                () => "Each item of the combobox should be of type 'ComboBoxItem'.");

            switch (index)
            {
                case 0:
                    Assert.That(comboBoxItem.Content, Is.EqualTo("Red").IgnoreCase,
                        () => "The first item of the combobox should be 'Red'.");
                    break;
                case 1:
                    Assert.That(comboBoxItem.Content, Is.EqualTo("Green").IgnoreCase,
                        () => "The second item of the combobox should be 'Green'.");
                    break;
                case 2:
                    Assert.That(comboBoxItem.Content, Is.EqualTo("Blue").IgnoreCase,
                        () => "The third item of the combobox should be 'Blue'.");
                    break;
            }
        }
    }

    [MonitoredTest("Should display the correct color when an item is selected"), Order(3)]
    public void _3_ShouldDispalyTheCorrectColorWhenAnItemIsSelected()
    {
        AssertHasComboBoxAndLabel();
        _2_ComboBoxShouldHaveAComboBoxItemForEachColor();

        _colorComboBox.SelectedIndex = -1;

        AssertColorChange(0, Colors.Red, "Colors.Red");
        AssertColorChange(1, Colors.Green, "Colors.Green");
        AssertColorChange(2, Colors.Blue, "Colors.Blue");
    }

    private void AssertColorChange(int selectedIndex, Color expectedColor, string expectedColorName)
    {
        _colorComboBox.SelectedIndex = selectedIndex;

        var colorLabelBrush = _colorLabel.Background as SolidColorBrush;
        Assert.That(colorLabelBrush, Is.Not.Null, () => "The background of the Label should be of type SolidColorBrush");

        Assert.That(colorLabelBrush.Color, Is.EqualTo(expectedColor),
            () => $"When the first item is selected the color should change to '{expectedColor.ToString()}' ('{expectedColorName}')");
    }

    private void AssertHasComboBoxAndLabel()
    {
        Assert.That(_colorComboBox, Is.Not.Null, () => "No ComboBox control found in the window");
        Assert.That(_colorLabel, Is.Not.Null, () => "No Label control found in the window");
    }
}
