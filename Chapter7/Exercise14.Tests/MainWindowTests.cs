using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;
using NUnit.Framework;

namespace Exercise14.Tests;

[ExerciseTestFixture("dotNet1", "H07", "Exercise14", @"Exercise14\MainWindow.xaml;Exercise14\MainWindow.xaml.cs")]
[Apartment(ApartmentState.STA)]
public class MainWindowTests
{
    private MainWindow _window;
    private Grid _grid;

    private GroupBox _genderGroupBox;

    private GroupBox _ageGroupBox;

    private Button _button;

    [SetUp]
    public void SetUp()
    {
        _window = new MainWindow();
        _grid = (Grid)_window.Content;

        var allGroupBoxes = _grid.FindVisualChildren<GroupBox>().ToList();

        _genderGroupBox =
            allGroupBoxes.FirstOrDefault(groupBox => ((string) groupBox.Header).ToLower().Contains("geslacht"));

        _ageGroupBox =
            allGroupBoxes.FirstOrDefault(groupBox => ((string)groupBox.Header).ToLower().Contains("leeftijd"));

        _button = _grid.FindVisualChildren<Button>().ToList().FirstOrDefault();
    }

    [TearDown]
    public void TearDown()
    {
        _window?.Close();
    }

    [MonitoredTest("Should have name and firstname controls"), Order(1)]
    public void _1_ShouldHaveNameAndFirstNameControls()
    {
        var allLabels = _grid.FindVisualChildren<Label>().ToList();
        var lastnameLabel = allLabels.Find(l => l.Content.ToString().ToLower().Contains("naam"));
        var firstnameLabel = allLabels.Find(l => l.Content.ToString().ToLower().Contains("voornaam"));

        var allTextBoxes = _grid.FindVisualChildren<TextBox>().ToList();
        var lastnameTextBox = allTextBoxes.Find(tb => tb.Name.ToLower().Contains("lastname"));
        var firstnameTextBox = allTextBoxes.Find(tb => tb.Name.ToLower().Contains("firstname"));

        Assert.That(lastnameLabel, Is.Not.Null, () => "Could not find a Label control with content 'Naam:'");
        Assert.That(firstnameLabel, Is.Not.Null, () => "Could not find a Label control with content 'Voornaam:'");

        Assert.That(lastnameTextBox, Is.Not.Null, () => "Could not find a TextBox control for lastname");
        Assert.That(firstnameTextBox, Is.Not.Null, () => "Could not find a TextBox control for firstname");
    }

    [MonitoredTest("Should have a gender groupbox"), Order(2)]
    public void _2_ShouldHaveAGenderGroupBox()
    {
        Assert.That(_genderGroupBox, Is.Not.Null, () => "Could not find a GroupBox control with header 'Geslacht'");
        
        Grid grid = _genderGroupBox.Content as Grid;
        Assert.That(grid, Is.Not.Null, () => "GroupBox control 'Geslacht' should have a Grid element as its child element");            

        var radioButtons = grid.FindVisualChildren<RadioButton>().ToList();
        Assert.That(radioButtons.Count, Is.EqualTo(2), () => "Could not find 2 RadioButtons within the gender groupbox");
    }

    [MonitoredTest("Should have a age groupbox"), Order(3)]
    public void _3_ShouldHaveAnAgeGroupBox()
    {
        Assert.That(_ageGroupBox, Is.Not.Null, () => "Could not find a GroupBox control with header 'Leeftijd'");

        Grid grid = _ageGroupBox.Content as Grid;
        Assert.That(grid, Is.Not.Null, () => "GroupBox control 'Leeftijd' should have a Grid element as its child element");

        var radioButtons = grid.FindVisualChildren<RadioButton>().ToList();
        Assert.That(radioButtons.Count, Is.EqualTo(4), () => "Could not find 4 RadioButtons within the age groupbox");
    }

    [MonitoredTest("Should have a button"), Order(4)]
    public void _4_ShouldHaveAButton()
    {
        Assert.That(_button, Is.Not.Null, () => "Could not find a Button control");
        Assert.That(_button.Content, Contains.Substring("bereken").IgnoreCase, () => "The 'Content' of the button should be 'Bereken'");
    }

    [MonitoredTest("Should check which radiobuttons are checked after button click"), Order(5)]
    public void _5_ShouldCheckWhichRadioButtonsAreCheckedAfterButtonClick()
    {
        var sourceCode = Solution.Current.GetFileContent(@"Exercise14\MainWindow.xaml.cs");
        sourceCode = CodeCleaner.StripComments(sourceCode);

        Assert.That(sourceCode, Contains.Substring("(object sender, RoutedEventArgs e)"), () => "No eventhandler method found for the click event of the button");
        Assert.That(sourceCode, Contains.Substring(".IsChecked"), () => "No code found where the state of a radiobutton is checked");
    }
}
