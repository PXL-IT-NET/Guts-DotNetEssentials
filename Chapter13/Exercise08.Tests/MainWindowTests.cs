﻿using System;
using System.Collections;
using System.Collections.Generic;
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

[ExerciseTestFixture("dotNet1", "H13", "Exercise08", @"Exercise08\MainWindow.xaml;Exercise08\MainWindow.xaml.cs")]
[Apartment(ApartmentState.STA)]
public class MainWindowTests
{
    private TypeInfo _personTypeInfo;
    private MainWindow _window;
    
    private ListBox _theListBox;
    private TypeInfo _detailsWindowType;

    [SetUp]
    public void Setup()
    {
        var testedAssembly = typeof(MainWindow).Assembly;
        _personTypeInfo = testedAssembly.DefinedTypes.FirstOrDefault(t => t.Name == "Person");
        _detailsWindowType =
            testedAssembly.DefinedTypes.FirstOrDefault(t =>
                typeof(Window).IsAssignableFrom(t) && t.Name == "DetailsWindow");

        _window = new MainWindow();
        var grid = _window.Content as Grid;
        
        _theListBox = grid.FindVisualChildren<ListBox>().FirstOrDefault();

    }

    [TearDown]
    public void TearDown()
    {
        _window?.Close();
    }

    [MonitoredTest("Should have a public Person class"), Order(1)]
    public void _1_ShouldHaveAPublicPersonClass()
    {
        AssertHasPersonClass();
    }

    [MonitoredTest("Person should have the correct properties"), Order(2)]
    public void _2_PersonShouldHaveTheCorrectProperties()
    {
        AssertHasPersonClass();
        var nameProperty = FindProperty(typeof(string), new[] { "naam", "name" });
        AssertProperty(nameProperty, "Name", "string");

        var firstnameProperty = FindProperty(typeof(string), new[] { "voornaam", "firstname" });
        AssertProperty(firstnameProperty, "Firstname", "string");

        var genderProperty = FindProperty(new[] { typeof(string), typeof(bool), typeof(Enum) }, new[] { "geslacht", "gender", "man", "vrouw", "male", "female" });
        AssertProperty(genderProperty, "Gender", "string or bool or enum");

        var addressProperty = FindProperty(typeof(string), new[] { "adres", "address" });
        AssertProperty(addressProperty, "Address", "string");

        var phoneProperty = FindProperty(typeof(string), new[] { "telefoon", "phone" });
        AssertProperty(phoneProperty, "Phone", "string");

        var birthDateProperty = FindProperty(new[] { typeof(DateTime), typeof(DateTime?) }, new[] { "datum", "date" });
        AssertProperty(birthDateProperty, "BirthDate", "DateTime or Nullable<DateTime>");
    }

    [MonitoredTest("Should use a generic List of Persons"), Order(3)]
    public void _3_ShouldUseAGenericListOfPersons()
    {
        var listOfPersons = GetListOfPersons();

        Assert.That(listOfPersons, Is.Not.Null, () => "Could not find a field that can hold a list of Persons");
        Assert.That(listOfPersons.Count, Is.GreaterThanOrEqualTo(2), () => "The list of persons should hold at least 2 persons");
    }

    [MonitoredTest("Should have a ListBox"), Order(4)]
    public void _4_ShouldHaveAListBox()
    {
        AssertListBoxPresent();
    }

    [MonitoredTest("Shoud have an item in the ListBox for each person"), Order(5)]
    public void _5_ShouldHaveAnItemInTheListBoxForEachPerson()
    {
        AssertListBoxPresent();
        var listOfPersons = GetListOfPersons();

        Assert.That(_theListBox.Items, Is.Not.Null, () => "Items property of the ListBox is null.");
        Assert.That(_theListBox.Items.Count, Is.EqualTo(listOfPersons.Count), () => "ListBox should have as many items as there are persons");
    }

    [MonitoredTest("Should open a Person detail window on click of 'Details' button"), Order(6)]
    public void _6_ShouldOpenAPersonDetailWindowWhenClickingDetailsButton()
    {
        AssertListBoxPresent();

        var xamlCode = Solution.Current.GetFileContent(@"Exercise08\MainWindow.xaml");

        Assert.That(_detailsWindowType, Is.Not.Null, () => "No class found that can be used to display person details");

        var hasConstructorThatAcceptsAPersonInstance = _detailsWindowType.DeclaredConstructors.Any(c =>
            c.GetParameters().Any(p => p.ParameterType.Name == _personTypeInfo.Name));

        Assert.That(hasConstructorThatAcceptsAPersonInstance, Is.True, 
            () => "The window that will display person details should have a constructor that accepts a person instance");

        var csCode = Solution.Current.GetFileContent(@"Exercise08\MainWindow.xaml.cs");
        csCode = CodeCleaner.StripComments(csCode);

        Assert.That(csCode, Contains.Substring(".SelectedIndex").Or.Contains(".SelectedItem"),
            () => "No code found where the selected person is retrieved.");

        Assert.That(csCode, Contains.Substring(".Show()").Or.Contains(".ShowDialog()"), 
            () => "No code found where a person window is showed. Use the 'Show' method a window to achieve this.");

    }

    private IList GetListOfPersons()
    {
        var genericListType = typeof(List<>);
        var personListType = genericListType.MakeGenericType(_personTypeInfo.UnderlyingSystemType);


        Type mainWindowType = _window.GetType();

        // Get all private fields
        var privateFields = new List<FieldInfo>(mainWindowType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic));
        var peopleListField = privateFields.Find(field => field.FieldType == personListType);

        if (peopleListField is null)
        {
            return null; // there is no IList<Person> field
        }
        return peopleListField.GetValue(_window) as IList;
    }

    private void AssertListBoxPresent()
    {
        Assert.That(_theListBox, Is.Not.Null, () => "Could not find a ListBox control.");
    }

    private void AssertProperty(PropertyInfo property, string propertyDescription, string propertyTypeDescription, bool hasPublicGetter = true, bool hasPublicSetter = true)
    {
        Assert.That(property, Is.Not.Null, () => $"Could not find a property '{propertyDescription}' of type '{propertyTypeDescription}'");
        Assert.That(char.IsUpper(property.Name[0]), Is.True, () => $"The name of the property '{propertyDescription}' should begin with a capital character.");
        if (hasPublicGetter)
        {
            Assert.That(property.GetMethod, Is.Not.Null, () => $"The property '{propertyDescription}' should have a get method.");
            Assert.That(property.GetMethod.IsPublic, Is.True, () => $"The get method of the property '{propertyDescription}' should be publicly accessible");
        }
        if (hasPublicSetter)
        {
            Assert.That(property.SetMethod, Is.Not.Null, () => $"The property '{propertyDescription}' should have a set method.");
            Assert.That(property.SetMethod.IsPublic, Is.True, () => $"The setter of the property '{propertyDescription}' should be publicly accessible");
        }
    }

    private PropertyInfo FindProperty(Type propertyType, IEnumerable<string> possiblePartialNames)
    {
        return FindProperty(new[] { propertyType }, possiblePartialNames);
    }

    private PropertyInfo FindProperty(IEnumerable<Type> propertyTypes, IEnumerable<string> possiblePartialNames)
    {
        var allProperties = _personTypeInfo.GetProperties();

        return allProperties.FirstOrDefault(p =>
            propertyTypes.Any(t => t.IsAssignableFrom(p.PropertyType))
            && possiblePartialNames.Any(namePart => p.Name.ToLower().Contains(namePart)));
    }

    private void AssertHasPersonClass()
    {
        Assert.That(_personTypeInfo, Is.Not.Null, () => "No class named 'Person' can be found");
        Assert.That(_personTypeInfo.IsPublic, Is.True, () => "The person class is not public");
    }
}
