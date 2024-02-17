using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;
using NUnit.Framework;

namespace Exercise08.Tests;

[ExerciseTestFixture("dotNet1", "H13", "Exercise08", @"Exercise08\DetailsWindow.xaml;Exercise08\DetailsWindow.xaml.cs")]
[Apartment(ApartmentState.STA)]

public class DetailsWindowTests
{
    private DetailsWindow _detailsWindow;
    private Grid _grid;

    private object _person;

    private TextBox _lastNameTextBox;
    private TextBox _firstNameTextBox;
    private TextBox _addressTextBox;
    private TextBox _phoneTextBox;
    private TextBox _birthdateTextBox;
    private Button _okButton;
    private Button _cancelButton;

    [SetUp]
    public void Setup()
    {
        _person = PersonHelper.CreatePerson();
        if (_person != null)
        {
            // set some props
            PersonHelper.SetPropertyValue(_person, "LastName", "Doe");
            PersonHelper.SetPropertyValue(_person, "FirstName", "John");
            PersonHelper.SetPropertyValue(_person, "Address", "Main street 1");
            PersonHelper.SetPropertyValue(_person, "Phone", "123456789");
            PersonHelper.SetPropertyValue(_person, "BirthDate", new DateTime(1980, 1, 1));

            // create window
            var detailsWindowObj = CreateDetailsWindow(_person);
            if (detailsWindowObj != null)
            {
                _detailsWindow = detailsWindowObj as DetailsWindow;
                _grid = _detailsWindow.Content as Grid;
            }
        }
        if (_grid != null)
        {
            var allTextBlocks = _grid.FindVisualChildren<TextBlock>().ToList();
            _lastNameTextBox = _grid.FindVisualChildren<TextBox>().FirstOrDefault(tb => tb.Name.ToLower().Contains("lastname"));
            _firstNameTextBox = _grid.FindVisualChildren<TextBox>().FirstOrDefault(tb => tb.Name.ToLower().Contains("firstname"));
            _addressTextBox = _grid.FindVisualChildren<TextBox>().FirstOrDefault(tb => tb.Name.ToLower().Contains("address"));
            _phoneTextBox = _grid.FindVisualChildren<TextBox>().FirstOrDefault(tb => tb.Name.ToLower().Contains("phone"));
            _birthdateTextBox = _grid.FindVisualChildren<TextBox>().FirstOrDefault(tb => tb.Name.ToLower().Contains("birthdate"));
        }
    }

    [TearDown]
    public void TearDown()
    {
        _detailsWindow?.Close();
    }

    [MonitoredTest]
    public void ShouldHaveAConstructorThatAcceptsAPerson()
    {
        Assert.That(_detailsWindow, Is.Not.Null,
            () => "The window that will display person details should have a constructor that accepts a person instance");
    }

    [MonitoredTest]
    public void ShouldHaveAllControlsToShowAPerson()
    {
        var allTextBlocks = _grid.FindVisualChildren<TextBlock>().ToList();
        Assert.That(allTextBlocks.Any(tb => tb.Text.ToLower().Contains("naam:")),
            () => "The window should have a textblock that shows the last name of the person");
        Assert.That(allTextBlocks.Any(tb => tb.Text.ToLower().Contains("voornaam:")),
            () => "The window should have a textblock that shows the first name of the person");
        Assert.That(allTextBlocks.Any(tb => tb.Text.ToLower().Contains("adres:")),
            () => "The window should have a textblock that shows the address of the person");
        Assert.That(allTextBlocks.Any(tb => tb.Text.ToLower().Contains("tel:")),
            () => "The window should have a textblock that shows the phone number of the person");
        Assert.That(allTextBlocks.Any(tb => tb.Text.ToLower().Contains("geboren:")),
            () => "The window should have a textblock that shows the birthdate of the person");

        Assert.That(_lastNameTextBox, Is.Not.Null,
            () => "The window should have a textbox to show the last name of the person");
        Assert.That(_firstNameTextBox, Is.Not.Null,
            () => "The window should have a textbox to show the first name of the person");
        Assert.That(_addressTextBox, Is.Not.Null,
            () => "The window should have a textbox to show the address of the person");
        Assert.That(_phoneTextBox, Is.Not.Null,
            () => "The window should have a textbox to show the phone number of the person");
        Assert.That(_birthdateTextBox, Is.Not.Null,
            () => "The window should have a textbox to show the birthdate of the person");

        var allRadioButtons = _grid.FindVisualChildren<RadioButton>().ToList();
        Assert.That(allRadioButtons.Any(rb => rb.Name.ToLower().Contains("male")),
            () => "The window should have a radiobutton to indicate a male person");
        Assert.That(allRadioButtons.Any(rb => rb.Name.ToLower().Contains("female")),
            () => "The window should have a radiobutton to indicate a female person");
    }

    [MonitoredTest]
    public void ShouldHaveOKAndCancelButtons()
    {
        var allButtons = _grid.FindVisualChildren<Button>().ToList();
        Assert.That(allButtons.Any(b => b.Name.ToLower().Contains("ok")),
            () => "The window should have a OK button");
        Assert.That(allButtons.Any(b => b.Name.ToLower().Contains("cancel")),
            () => "The window should have a Cancel button");
    }

    [MonitoredTest]
    public void WhenClickingOK_ShouldCloseTheWindow()
    {
        Assert.That(_detailsWindow, Is.Not.Null, () => "The window should be created in the setup method");
        _okButton = _grid.FindVisualChildren<Button>().FirstOrDefault(b => b.Name.ToLower().Contains("ok"));
        Assert.That(_okButton, Is.Not.Null, () => "DetailsWindows should contain a button with the name 'OK'");
        _detailsWindow.Show();
        _okButton.FireClickEvent();
        Assert.That(_detailsWindow.IsVisible, Is.False, () => "After clicking the OK button the window should be closed");
    }

    [MonitoredTest]
    public void WhenClickingCancel_ShouldCloseTheWindow()
    {
        Assert.That(_detailsWindow, Is.Not.Null, () => "The window should be created in the setup method");
        _cancelButton = _grid.FindVisualChildren<Button>().FirstOrDefault(b => b.Name.ToLower().Contains("cancel"));
        Assert.That(_cancelButton, Is.Not.Null, () => "DetailsWindows should contain a button with the name 'Cancel'");
        _detailsWindow.Show();
        _cancelButton.FireClickEvent();
        Assert.That(_detailsWindow.IsVisible, Is.False, () => "After clicking the Cancel button the window should be closed");
    }

    [MonitoredTest]
    public void WhenLoaded_ShouldShowThePersonInfo()
    {
        Assert.That(_detailsWindow, Is.Not.Null, () => "The window should be created in the setup method");
        _detailsWindow.Show();
        Assert.That(_lastNameTextBox.Text, Is.EqualTo("Doe"), () => "The last name of the person should be shown in the last name textbox");
        Assert.That(_firstNameTextBox.Text, Is.EqualTo("John"), () => "The first name of the person should be shown in the first name textbox");
        Assert.That(_addressTextBox.Text, Is.EqualTo("Main street 1"), () => "The address of the person should be shown in the address textbox");
        Assert.That(_phoneTextBox.Text, Is.EqualTo("123456789"), () => "The phone number of the person should be shown in the phone textbox");
        Assert.That(_birthdateTextBox.Text, Is.EqualTo($"{new DateTime(1980, 1, 1):d}"), () => "The birthdate of the person should be shown in the birthdate textbox and formatted as date");
    }

    private object CreateDetailsWindow(object person)
    {
        Assert.That(person, Is.Not.Null);
        object[] parameters = new object[] { person };
        object detailsWindow = null;
        try
        {
            detailsWindow = Activator.CreateInstance(typeof(DetailsWindow), parameters);
        }
        catch (Exception)
        {
            // swallow
        }
        return detailsWindow;
    }
}
