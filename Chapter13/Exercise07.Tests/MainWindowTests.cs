using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;
using NUnit.Framework;

namespace Exercise07.Tests;

[ExerciseTestFixture("dotNet1", "H13", "Exercise07", @"Exercise07\MainWindow.xaml;Exercise07\MainWindow.xaml.cs")]
[Apartment(ApartmentState.STA)]
public class MainWindowTests
{
    private MainWindow _window;
    private TextBox _monthNumberTextBox;
    private TextBox _monthNameTextBox;
    private Button _lookupButton;
    private IList<string> _listOfMonths;

    [SetUp]
    public void Setup()
    {
        _window = new MainWindow();
        var grid = _window.Content as Grid;
        var allTextBoxes = grid.FindVisualChildren<TextBox>().ToList();
        _monthNumberTextBox = allTextBoxes.FirstOrDefault(textBox => textBox.Name == "monthNumberTextBox");
        _monthNameTextBox = allTextBoxes.FirstOrDefault(textBox => textBox.Name == "monthNameTextBox");
        _lookupButton = grid.FindVisualChildren<Button>().FirstOrDefault(b => b.Name == "lookupButton");

        _listOfMonths = RetrieveListOrIList();
    }

    [TearDown]
    public void TearDown()
    {
        _window?.Close();
    }

    [MonitoredTest("Should have 2 TextBoxes and a Button"), Order(1)]
    public void _1_ShouldHave2TextBoxesAndAButton()
    {
        AssertHasAllControls();
    }

    [MonitoredTest("Should not have a ListBox"), Order(2)]
    public void _2_ShouldNotHaveAListBox()
    {
        Assert.That(() => _window.GetPrivateFieldValue<ListBox>(), Throws.TypeOf<FieldAccessException>());
    }

    [MonitoredTest("Should use a generic List"), Order(3)]
    public void _3_ShouldUseAGenericList()
    {
        Assert.That(_listOfMonths, Is.Not.Null, 
            () => "Cannot find a declaration of a field (instance variable) in the class that is a generic list that can contain names of months.");

        Assert.That(_listOfMonths.Count, Is.EqualTo(12),
            () => "The generic list should contain 12 items (after construction of the 'MainWindow')");
    }

    [MonitoredTest("Should lookup months correctly"), Order(4)]
    public void _4_ShouldLookUpMonthsCorrectly()
    {
        AssertHasAllControls();
        AssertMonthLookup(1, "January", "Januari");
        AssertMonthLookup(2, "February", "Februari");
        AssertMonthLookup(3, "March", "Maart");
        AssertMonthLookup(4, "April", "April");
        AssertMonthLookup(5, "May", "Mei");
        AssertMonthLookup(6, "June", "Juni");
        AssertMonthLookup(7, "July", "Juli");
        AssertMonthLookup(8, "August", "Augustus");
        AssertMonthLookup(9, "September", "September");
        AssertMonthLookup(10, "October", "Oktober");
        AssertMonthLookup(11, "November", "November");
        AssertMonthLookup(12, "December", "December");
    }

    private void AssertMonthLookup(int monthNumber, string expectedMonthName, string alternativeMonthName)
    {
        _monthNumberTextBox.Text = monthNumber.ToString();
        _lookupButton.FireClickEvent();
        Assert.That(_monthNameTextBox.Text, 
            Is.EqualTo(expectedMonthName).IgnoreCase.Or.EqualTo(alternativeMonthName).IgnoreCase, 
            () => $"Looking up month number {monthNumber}, results in '{_monthNameTextBox.Text}' while '{expectedMonthName}' is expected.");
    }

    private void AssertHasAllControls()
    {
        Assert.That(_monthNumberTextBox, Is.Not.Null, () => "No TextBox with the name 'monthNumberTextBox' can be found.");
        Assert.That(_monthNameTextBox, Is.Not.Null, () => "No TextBox with the name 'monthNameTextBox' can be found.");
        Assert.That(_lookupButton, Is.Not.Null, () => "No Button with the name 'lookupButton' can be found.");
    }

    private IList<string> RetrieveListOrIList()
    {
        IList<string> foundList = null;
        try
        {
            foundList = _window.GetPrivateFieldValue<IList<string>>();
        }
        catch(FieldAccessException)
        {
            // it wasn't an IList, try a List
            try
            {
                foundList = _window.GetPrivateFieldValue<List<string>>();
            }
            catch (FieldAccessException)
            { 
                // swallow it and return null
            }
        }
        return foundList;
    }

}
