using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;
using NUnit.Framework;

namespace Exercise13.Tests;

[ExerciseTestFixture("dotNet1", "H07", "Exercise13", @"Exercise13\MainWindow.xaml;Exercise13\MainWindow.xaml.cs")]
[Apartment(ApartmentState.STA)]
[SetCulture("nl-BE")]
public class MainWindowTests
{
    private MainWindow _window;

    private Label _priceLabel;
    private TextBox _priceTextBox;

    private CheckBox _checkBox;

    private Label _btwLabel;
    private TextBox _btwTextBox;

    private Label _totalLabel;
    private TextBox _totalTextBox;

    private Button _button;

    [SetUp]
    public void SetUp()
    {
        _window = new MainWindow();
        Grid grid = (Grid)_window.Content;

        var allLabels = grid.FindVisualChildren<Label>().ToList();
        _priceLabel = allLabels.Find(l => l.Content.ToString().ToLower().Contains("prijs"));
        _btwLabel = allLabels.Find(l => l.Content.ToString().ToLower().Contains("btw"));
        _totalLabel = allLabels.Find(l => l.Content.ToString().ToLower().Contains("totaal"));

        var allTextBoxes = grid.FindVisualChildren<TextBox>().ToList();
        _priceTextBox = allTextBoxes.FirstOrDefault(textBox =>
            textBox.Name.ToLower().Contains("price") || textBox.Name.ToLower().Contains("prijs"));

        _btwTextBox = allTextBoxes.FirstOrDefault(textBox => textBox.Name.ToLower().Contains("btw")
                                                             || textBox.Name.ToLower().Contains("vat"));

        _totalTextBox = allTextBoxes.FirstOrDefault(textBox => textBox.Name.ToLower().Contains("tot"));

        _checkBox = grid.FindVisualChildren<CheckBox>().ToList().FirstOrDefault();
        _button = grid.FindVisualChildren<Button>().ToList().FirstOrDefault();
    }

    [TearDown]
    public void TearDown()
    {
        _window?.Close();
    }

    [MonitoredTest("Should have price controls"), Order(1)]
    public void _1_ShouldHavePriceControls()
    {
        Assert.That(_priceLabel, Is.Not.Null, () => "Could not find a Label control with content 'Netto prijs:'");
        Assert.That(_priceTextBox, Is.Not.Null, () => "Could not find a TextBox control that has the text 'price' in its name. Consider using 'priceTextBox' as value for the 'Name' property");
    }

    [MonitoredTest("Should have a checkbox control"), Order(2)]
    public void _2_ShouldHaveACheckBoxControl()
    {
        Assert.That(_checkBox, Is.Not.Null, () => "Could not find a CheckBox control");
        Assert.That(_checkBox.Content, Contains.Substring("Verlaagd tarief").IgnoreCase, () => "The 'Content' of the checkbox should be 'Verlaagd tarief'");
    }

    [MonitoredTest("Should have BTW controls"), Order(3)]
    public void _3_ShouldHaveBtwControls()
    {
        Assert.That(_btwLabel, Is.Not.Null, () => "Could not find a Label control with name 'btwLabel' or 'vatLabel'");
        Assert.That(_btwTextBox, Is.Not.Null, () => "Could not find a TextBox control that has the text 'btw' in its name. Consider using 'btwTextBox' as value for the 'Name' property");
    }

    [MonitoredTest("Should have total controls"), Order(4)]
    public void _4_ShouldHaveTotalControls()
    {
        Assert.That(_totalLabel, Is.Not.Null, () => "Could not find a Label control with content 'Totaal:'");
        Assert.That(_totalTextBox, Is.Not.Null, () => "Could not find a TextBox control that has the text 'tot' in its name. Consider using 'totalTextBox' as value for the 'Name' property");
    }

    [MonitoredTest("Should have a button"), Order(5)]
    public void _5_ShouldHaveAButton()
    {
        Assert.That(_button, Is.Not.Null, () => "Could not find a Button control");
        Assert.That(_button.Content, Contains.Substring("bereken").IgnoreCase, () => "The 'Content' of the button should be 'Bereken'");
    }

    [MonitoredTest("Should display btw and total as readonly textboxes"), Order(6)]
    public void _6_ShouldDisplayBtwAndTotalAsReadOnlyTextBoxes()
    {
        AssertAllControlsArePresent();

        Assert.That(!_btwTextBox.IsEnabled || _btwTextBox.IsReadOnly, Is.True, () => "The BTW TextBox is not readonly. Tip: 'IsEnabled' or 'IsReadOnly' property.");
        Assert.That(!_totalTextBox.IsEnabled || _totalTextBox.IsReadOnly, Is.True, () => "The total TextBox is not readonly. Tip: 'IsEnabled' or 'IsReadOnly' property.");
    }

    [MonitoredTest("Should calculate Btw at 21% when checkbox is unchecked"), Order(7)]
    public void _7_ShouldCalculateBtwAtRate21WhenCheckBoxIsUnchecked()
    {
        AssertAllControlsArePresent();

        var netPrice = "50";
        _priceTextBox.Text = netPrice;
        _checkBox.IsChecked = false;

        _button.FireClickEvent();

        Assert.That(_btwTextBox.Text, Is.EqualTo("10,5"), () => $"Btw for net price of '{netPrice}' is not correct");
        Assert.That(_totalTextBox.Text, Is.EqualTo("60,5"), () => $"Total for net price of '{netPrice}' is not correct");
    }

    [MonitoredTest("Should calculate Btw at 6% when checkbox is checked"), Order(8)]
    public void _8_ShouldCalculateBtwAtRate6WhenCheckBoxIsChecked()
    {
        AssertAllControlsArePresent();

        var netPrice = "200";
        _priceTextBox.Text = netPrice;
        _checkBox.IsChecked = true;

        _button.FireClickEvent();

        Assert.That(_btwTextBox.Text, Is.EqualTo("12"), () => $"Btw for net price of '{netPrice}' is not correct");
        Assert.That(_totalTextBox.Text, Is.EqualTo("212"), () => $"Total for net price of '{netPrice}' is not correct");
    }

    private void AssertAllControlsArePresent()
    {
        _1_ShouldHavePriceControls();
        _2_ShouldHaveACheckBoxControl();
        _3_ShouldHaveBtwControls();
        _4_ShouldHaveTotalControls();
        _5_ShouldHaveAButton();
    }
}
