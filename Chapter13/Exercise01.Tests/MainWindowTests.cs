using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;
using NUnit.Framework;

namespace Exercise01.Tests;

[ExerciseTestFixture("dotNet1", "H13", "Exercise01", @"Exercise01\MainWindow.xaml;Exercise01\MainWindow.xaml.cs"),
 Apartment(ApartmentState.STA)]
public class MainWindowTests
{
    private TestWindow<MainWindow> _window;

    [SetUp]
    public void Setup()
    {
        _window = new TestWindow<MainWindow>();
    }

    [TearDown]
    public void TearDown()
    {
        _window.Dispose();
    }

    [MonitoredTest("Should have a ListBox"), Order(1)]
    public void _1_ShouldHaveAListBox()
    {
        GetTheListBox();
    }

    [MonitoredTest("Shoud have at least one item in the ListBox"), Order(2)]
    public void _2_ShouldHaveItemsInTheListBox()
    {
        var theListBox = GetTheListBox();

        Assert.That(theListBox.Items, Is.Not.Null, () => "Items property of the ListBox is null.");
        Assert.That(theListBox.Items.Count, Is.AtLeast(1), () => "ListBox has no items.");
        var listboxItems = theListBox.Items.OfType<ListBoxItem>().ToList();
        Assert.That(theListBox.Items.Count, Is.EqualTo(listboxItems.Count), () => "All items in the ListBox should be of type 'ListBoxItem'");
    }

    [MonitoredTest("Shoud delete a ListBox item when it is selected"), Order(3)]
    public void _3_ShoudDeleteItemWhenItIsSelected()
    {
        //Arrange
        var theListBox = GetTheListBox();
        theListBox.SelectedIndex = -1;
        var originalNumberOfItems = theListBox.Items.Count;
        var firstItem = (ListBoxItem)theListBox.Items[0];

        //Act
        firstItem.IsSelected = true; //trigger SelectionChanged event

        //Assert
        Assert.That(theListBox.Items.Count, Is.EqualTo(originalNumberOfItems - 1), () => "No item was deleted.");
        Assert.That(theListBox.Items, Does.Not.Contain(firstItem), () => "The wrong item was deleted.");
    }

    private ListBox GetTheListBox()
    {
        ListBox theListBox = _window.GetUIElements<ListBox>().FirstOrDefault();
        Assert.That(theListBox, Is.Not.Null, () => "Could not find a ListBox control.");
        return theListBox;
    }
}
