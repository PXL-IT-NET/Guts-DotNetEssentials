using System;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using NUnit.Framework;

namespace Exercise06.Tests;

[ExerciseTestFixture("dotNet1", "H06", "Exercise06", @"Exercise06\MainWindow.xaml;Exercise06\MainWindow.xaml.cs")]
[Apartment(ApartmentState.STA)]
public class MainWindowTests
{
    private Rectangle _minutesRectangle;
    private Rectangle _secondsRectangle;
    private DispatcherTimer _dispatcherTimer;
    private EventHandler _tickEventHandler;
    private MainWindow _window;

    [SetUp]
    public void Setup()
    {
        _window = new MainWindow();
        Grid grid = (Grid)_window.Content;

        var allRectangles = _window.GetAllPrivateFieldValues<Rectangle>()
            .OrderBy(r => r.Margin.Top).ToList();
        // _minutesRectangle is located at the top,
        // _secondsRectangle at the bottom
        // of the canvas
        if (allRectangles.Count() > 0)
        {
            _minutesRectangle = allRectangles[0];
            if (allRectangles.Count() > 1)
            {
                _secondsRectangle = allRectangles[1];
            }
        }

        //var allRectangles = grid.FindVisualChildren<Rectangle>().ToList();
        //_minutesRectangle = allRectangles.Where(field => field.Name.ToLower().Contains("minu")).FirstOrDefault();
        //_secondsRectangle = allRectangles.Where(field => field.Name.ToLower().Contains("sec")).FirstOrDefault();

        _dispatcherTimer = _window.GetAllPrivateFieldValues<DispatcherTimer>().ToList().FirstOrDefault();
        if (_dispatcherTimer != null)
        {
            _tickEventHandler = _dispatcherTimer.GetPrivateFieldValueByName<EventHandler>(nameof(DispatcherTimer.Tick));
        }
    }

    [TearDown]
    public void Teardown()
    {
        _window?.Close();
    }

    [MonitoredTest("Should have 2 rectangles in a canvas"), Order(1)]
    public void _1_ShouldHaveTwoRectanglesInACanvas()
    {
        AssertHasRectangles();

        Assert.That(_secondsRectangle.Parent, Is.Not.Null, () => "The seconds Rectangle should be inside a Canvas");
        Assert.That(_secondsRectangle.Parent, Is.TypeOf<Canvas>(), () => "The seconds Rectangle should be inside a Canvas");

        Assert.That(_minutesRectangle.Parent, Is.Not.Null, () => "The minutes Rectangle should be inside a Canvas");
        Assert.That(_minutesRectangle.Parent, Is.TypeOf<Canvas>(), () => "The minutes Rectangle should be inside a Canvas");
    }

    [MonitoredTest("Should have a DispatcherTimer and a method that handles its ticks"), Order(2)]
    public void _2_ShouldHaveADispatcherTimerAndMethodThatHandlesTicks()
    {
        AssertHasDispatcherTimer();

        Assert.That(_tickEventHandler, Is.Not.Null, () => "No event handler set for the Tick event of the DispatcherTimer");
        var invocationList = _tickEventHandler.GetInvocationList();
        Assert.That(invocationList.Length, Is.GreaterThan(0), () => "No event handler set for the Tick event of the DispatcherTimer");
    }

    [MonitoredTest("Should tick every second"), Order(3)]
    public void _3_ShouldTickEverySecond()
    {
        AssertHasDispatcherTimer();

        Assert.That(_dispatcherTimer.Interval, Is.Not.Null, () => "No interval set for the Tick event of the DispatcherTimer");
        Assert.That(_dispatcherTimer.Interval, Is.EqualTo(TimeSpan.FromSeconds(1)), () => "The interval set for the Tick event of the DispatcherTimer should be exactly one second");
        Assert.That(_dispatcherTimer.IsEnabled, Is.True, () => "The dispatcher timer is not started. Use the 'Start' method to start the timer");
    }

    [MonitoredTest("Should make the seconds rectangle wider on every tick"), Order(4)]
    public void _4_ShouldMakeTheSecondsRectangleWiderOnEveryTick()
    {
        AssertHasDispatcherTimer();
        AssertHasRectangles();

        _dispatcherTimer.Stop();

        var originalWidth = _secondsRectangle.Width;

        InvokeTickEvent();

        var newWidth = _secondsRectangle.Width;

        Assert.That(newWidth, Is.GreaterThan(originalWidth));
    }

    [MonitoredTest("Should have a zero width seconds rectangle after 60 ticks"), Order(5)]
    public void _5_ShouldHaveAZeroWidthSecondsRectangleAfter60Ticks()
    {
        AssertHasDispatcherTimer();
        AssertHasRectangles();

        _dispatcherTimer.Stop();

         _secondsRectangle.Width = 0;

        InvokeTickEvent(60);

        Assert.That(_secondsRectangle.Width, Is.EqualTo(0));
    }

    [MonitoredTest("Should make the minutes rectangle wider every 60 ticks"), Order(6)]
    public void _6_ShouldMakeTheMinutesRectangleWiderEvery60Ticks()
    {
        AssertHasDispatcherTimer();
        AssertHasRectangles();

        _dispatcherTimer.Stop();

        var originalWidth = _minutesRectangle.Width;

        InvokeTickEvent(60);

        var newWidth = _minutesRectangle.Width;

        Assert.That(newWidth, Is.GreaterThan(originalWidth));
    }

    [MonitoredTest("Should have a zero width minutes rectangle after one hour"), Order(7)]
    public void _7_ShouldHaveAZeroWidthMinutesRectangleAfterAnHour()
    {
        AssertHasDispatcherTimer();
        AssertHasRectangles();

        _dispatcherTimer.Stop();

        _minutesRectangle.Width = 0;

        InvokeTickEvent(60 * 60);

        Assert.That(_minutesRectangle.Width, Is.EqualTo(0));
    }

    private void AssertHasRectangles()
    {
        Assert.That(_minutesRectangle, Is.Not.Null,
            () =>
                "No private Rectangle field found with a name that contains 'minu'. Create a Rectangle with a name like 'minutesRectangle'");
        Assert.That(_secondsRectangle, Is.Not.Null,
            () =>
                "No private Rectangle field found with a name that contains 'sec'. Create a Rectangle with a name like 'secondsRectangle'");
    }

    private void AssertHasDispatcherTimer()
    {
        Assert.That(_dispatcherTimer, Is.Not.Null, () => "No private field found of the type DispatcherTimer");
       
    }

    private void InvokeTickEvent(int numberOfTimes)
    {
        for (int i = 0; i < numberOfTimes; i++)
        {
            InvokeTickEvent();
        }
    }

    private void InvokeTickEvent()
    {
        if (_tickEventHandler == null) return;

        var invocationList = _tickEventHandler.GetInvocationList();
        Assert.That(invocationList.Length, Is.GreaterThan(0));

        foreach (var handlerDelegate in invocationList)
        {
            handlerDelegate.Method.Invoke(handlerDelegate.Target, new Object[] {_dispatcherTimer, EventArgs.Empty});
        }
    }
}
