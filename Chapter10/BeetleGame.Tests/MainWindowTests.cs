using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BeetleGame.Tests;

[ExerciseTestFixture("dotNet1", "H10", "BeetleGame", @"BeetleGame\MainWindow.xaml;BeetleGame\MainWindow.xaml.cs")]
[Apartment(ApartmentState.STA)]
public class MainWindowTests
{
    private MainWindow _testWindow;
    private object _beetleObject;
    private DispatcherTimer _dispatcherTimer;
    private EventHandler _tickEventHandler;
    private Slider _speedSlider;
    private Slider _sizeSlider;
    private Label _speedLabel;
    private Label _sizeLabel;
    private Label _messageLabel;
    private Button _startButton, _resetButton;
    private Button _leftButton, _downButton, _upButton, _rightButton;
    private Canvas _paperCanvas;

    [SetUp]
    public void Setup()
    {
        _testWindow = new MainWindow();
        Grid grid = (Grid)_testWindow.Content;

        _paperCanvas = grid.FindVisualChildren<Canvas>().ToList().Find(c => c.Name.Contains("Canvas"));
        
        _beetleObject = _testWindow.GetPrivateFieldValue<Beetle>();
        _dispatcherTimer = _testWindow.GetPrivateFieldValue<DispatcherTimer>();
        _tickEventHandler = _dispatcherTimer?.GetPrivateFieldValueByName<EventHandler>(nameof(DispatcherTimer.Tick));
        _speedSlider = grid.FindVisualChildren<Slider>().FirstOrDefault(
            (slider) => slider.Name.ToUpper().Contains("SPEED"));
        _sizeSlider = grid.FindVisualChildren<Slider>().FirstOrDefault(
            (slider) => slider.Name.ToUpper().Contains("SIZE"));
        _startButton = grid.FindVisualChildren<Button>().ToList()
                         .Find(b => b.Content.ToString().ToUpper().Contains("START"));
        _resetButton = grid.FindVisualChildren<Button>().ToList()
                         .Find(b => b.Content.ToString().ToUpper().Contains("RESET"));
        _leftButton = grid.FindVisualChildren<Button>().ToList()
                         .Find(b => b.Content.ToString().ToUpper().Contains("LEFT"));
        _rightButton = grid.FindVisualChildren<Button>().ToList()
                         .Find(b => b.Content.ToString().ToUpper().Contains("RIGHT"));
        _downButton = grid.FindVisualChildren<Button>().ToList()
                         .Find(b => b.Content.ToString().ToUpper().Contains("DOWN"));
        _upButton = grid.FindVisualChildren<Button>().ToList()
                         .Find(b => b.Content.ToString().ToUpper().Contains("UP"));
        _speedLabel = grid.FindVisualChildren<Label>().FirstOrDefault(
            (label) => label.Name.ToUpper().Contains("SPEED"));
        _sizeLabel = grid.FindVisualChildren<Label>().FirstOrDefault(
            (label) => label.Name.ToUpper().Contains("SIZE"));
        _messageLabel = grid.FindVisualChildren<Label>().FirstOrDefault(
            (label) => label.Name.ToUpper().Contains("MESSAGE"));
    }

    [TearDown]
    public void TearDown()
    {
        _testWindow?.Close();
    }

    [MonitoredTest("MainWindow - Should have a private member of class Beetle"), Order(1)]
    public void _M01_ShouldHaveAPrivateBeetleMember()
    {
        Assert.That(_beetleObject, Is.Not.Null, "Mainwindow should have a private member variable of class Beetle named _beetle");
        Assert.That(_beetleObject, Is.TypeOf(BeetleHelper.BeetleType), "Mainwindow should have a private member variable of class Beetle");
    }

    [MonitoredTest("MainWindow - Should have a private member of class DispatcherTimer and a method that handles its ticks"), Order(2)]
    public void _M02_ShouldHaveAPrivateDispatcherTimerMember()
    {
        AssertHasDispatcherTimer();
        Assert.That(_tickEventHandler, Is.Not.Null, "No event handler set for the Tick event of the DispatcherTimer");
        var invocationList = _tickEventHandler.GetInvocationList();
        Assert.That(invocationList.Length, Is.GreaterThan(0), () => "No event handler set for the Tick event of the DispatcherTimer");
    }

    [MonitoredTest("MainWindow - Should have a slider for controlling Speed"), Order(3)]
    public void _M03_ShouldHaveSliderForSpeed()
    {
        double expectedMinimum = 0.5;
        double expectedMaximum = 10;
        Assert.That(_speedSlider, Is.Not.Null, "Mainwindow should have a Slider for speed (speedSlider)");
        Assert.That(_speedSlider.Minimum, Is.EqualTo(expectedMinimum),
            $"Slider for controlling speed should have a minimum value of {expectedMinimum}");
        Assert.That(_speedSlider.Maximum, Is.EqualTo(expectedMaximum), 
            $"Slider for controlling speed should have a maximum value of {expectedMaximum}");
    }

    [MonitoredTest("MainWindow - Should have a slider for controlling Size"), Order(4)]
    public void _M04_ShouldHaveSliderForSize()
    {
        double expectedMinimum = 10;
        double expectedMaximum = 20;
        double expectedTickFrequency = 2.0;
        bool expectedIsSnapToTickEnabled = true;
        Assert.That(_sizeSlider, Is.Not.Null, "Mainwindow should have a Slider for size (sizeSlider)");
        Assert.That(_sizeSlider.Minimum, Is.EqualTo(expectedMinimum),
            $"Slider for controlling size should have a minimum value of {expectedMinimum}");
        Assert.That(_sizeSlider.Maximum, Is.EqualTo(expectedMaximum), 
            $"Slider for controlling size should have a maximum value of {expectedMaximum}");
        Assert.That(_sizeSlider.TickFrequency, Is.EqualTo(expectedTickFrequency),
            $"Slider for controlling size should have a tick frequency of {expectedTickFrequency}");
        Assert.That(_sizeSlider.IsSnapToTickEnabled, Is.EqualTo(expectedIsSnapToTickEnabled),
            $"Slider for controlling size should have a IsSnapToTickEnabled property set to {expectedIsSnapToTickEnabled}");
    }

    [MonitoredTest("MainWindow - Should have buttons for controlling start and reset"), Order(5)]
    public void _M05_ShouldHaveButtonsForStartReset()
    {
        Assert.That(_startButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Start>");
        Assert.That(_resetButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Reset>");
    }

    [MonitoredTest("MainWindow - Should have buttons for controlling left, right, up, down control"), Order(6)]
    public void _M06_ShouldHaveButtonsForLeftRightUpDown()
    {
        Assert.That(_leftButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Left>");
        Assert.That(_rightButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Right>");
        Assert.That(_upButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Up>");
        Assert.That(_downButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Down>");
    }

    [MonitoredTest("MainWindow - Should have labels for size, speed and distance values"), Order(7)]
    public void _M07_ShouldHaveLabelsForSizeAndSpeedValues()
    {
        Assert.That(_speedLabel, Is.Not.Null, @"MainWindow should contain a Label named speedLabel");
        Assert.That(_sizeLabel, Is.Not.Null, @"MainWindow should contain a Label named sizeLabel");
        Assert.That(_messageLabel, Is.Not.Null, @"MainWindow should contain a Label named messageLabel");
    }

    [MonitoredTest("MainWindow - Hitting Start Button should change its content text to Stop (and back)"), Order(8)]
    public void _M08_ShouldChangeContentLabelToStopWhenHittingStartButtonAndBack()
    {
        Assert.That(_startButton.Content, Is.EqualTo("Start"), @"MainWindow should contain a Button labeled <Start>");
        _startButton.FireClickEvent();
        Assert.That(_startButton.Content, Is.EqualTo("Stop"), @"After clicking the Start-button, the content text should change to <Stop>");
        _startButton.FireClickEvent();
        Assert.That(_startButton.Content, Is.EqualTo("Start"), @"After clicking the Stop-button, the content text should change to <Start>");
    }

    [MonitoredTest("MainWindow - Hitting Start Button should disable sliders, stop enables them"), Order(9)]
    public void _M09_ShouldDisableSlidersWhenHittingStartAndBack()
    {
        _startButton.FireClickEvent();
        Assert.That(_sizeSlider.IsEnabled, Is.False, "Slider for size should be disabled when hitting <Start>");
        Assert.That(_speedSlider.IsEnabled, Is.False, "Slider for speed should be disabled when hitting <Start>");
        _startButton.FireClickEvent();
        Assert.That(_sizeSlider.IsEnabled, Is.True, "Slider for size should be enabled again when hitting <Stop>");
        Assert.That(_speedSlider.IsEnabled, Is.True, "Slider for speed should be enabled again when hitting <Stop>");
    }

    [MonitoredTest("MainWindow - Hitting Stop Button should display distance in label"), Order(10)]
    public void _M10_ShouldDisplayDistanceWhenHittingStop()
    {
        _startButton.FireClickEvent(); // start
        _startButton.FireClickEvent(); // stop
        Assert.That(_messageLabel.Content.ToString().ToLower().Contains("total distance in meter"),
            "messageLabel should provide a message with the total distance in meter, rounded with 2 digits");
    }

    [MonitoredTest("MainWindow - Hitting Left Button should set property on Beetle object"), Order(11)]
    public void _M11_ShouldSetBeetleRightPropertyToFalseWhenHittingLeftButton()
    {
        _leftButton.FireClickEvent();
        Assert.That(BeetleHelper.GetRightProperty(_beetleObject), Is.False, "Hitting the <Left> button should change Right property on Beetle to false");
    }

    [MonitoredTest("MainWindow - Hitting Right Button should set property on Beetle object"), Order(12)]
    public void _M12_ShouldSetBeetleRightPropertyToTrueWhenHittingRightButton()
    {
        _rightButton.FireClickEvent();
        Assert.That(BeetleHelper.GetRightProperty(_beetleObject), Is.True, "Hitting the <Right> button should change Right property on Beetle to true");
    }

    [MonitoredTest("MainWindow - Hitting Up Button should set property on Beetle object"), Order(13)]
    public void _M14_ShouldSetBeetleUpPropertyToTrueWhenHittingUpButton()
    {
        _upButton.FireClickEvent();
        Assert.That(BeetleHelper.GetUpProperty(_beetleObject), Is.True, "Hitting the <Up> button should change Up property on Beetle to true");
    }

    [MonitoredTest("MainWindow - Hitting Down Button should set property on Beetle object"), Order(14)]
    public void _M14_ShouldSetBeetleUpPropertyToFalseWhenHittingDownButton()
    {
        _downButton.FireClickEvent();
        Assert.That(BeetleHelper.GetUpProperty(_beetleObject), Is.False, "Hitting the <Down> button should change Up property on Beetle to false");
    }

    [MonitoredTest("MainWindow - Hitting Reset Button should reset the application to its initial state"), Order(15)]
    public void _M15_ShouldResetTheScreenWhenHittingResetButton()
    {
        _resetButton.FireClickEvent();
        Assert.That(_speedSlider.Value, Is.EqualTo(_speedSlider.Minimum),
            "Hitting the <Reset> button should set the speed slider value to its minimum");
        Assert.That(_sizeSlider.Value, Is.EqualTo(_sizeSlider.Minimum), 
            "Hitting the <Reset> button should set the size slider value to its minimum");
        Assert.That(_speedSlider.IsEnabled, Is.True,
            "Hitting the <Reset> button should set the enable the speed slider");
        Assert.That(_sizeSlider.IsEnabled, Is.True,
            "Hitting the <Reset> button should set the enable the size slider");
        Assert.That(_startButton.Content, Is.EqualTo("Start"),
            "Hitting the <Reset> button should set the <Start> button content to Start");
        Assert.That(BeetleHelper.GetIsVisibleProperty(_beetleObject), Is.False,
            "Hitting the <Reset> button should set the IsVisible property of the beetle to false");
        Assert.That(_dispatcherTimer.IsEnabled, Is.False,
            "Hitting the <Reset> button should disable the timer");
    }

    [MonitoredTest("MainWindow - timer interval in msec should be set with respect to beetle size and speed"), Order(16)]
    public void _M16_ShouldHaveTimerIntervalWithRespectToBeetleSizeAndSpeed()
    {
        AssertHasDispatcherTimer();
        AssertDispatcherTimerIntervalWithRespectToBeetleSizeAndSpeed();
    }

    [MonitoredTest("MainWindow - Changing size slider should set property on Beetle object and interval on timer"), Order(17)]
    public void _M17_ShouldChangeTimerIntervalSizePropertyAndLabelWhenChangingSliderValue()
    {
        AssertHasDispatcherTimer();
        AssertDispatcherTimerIntervalWithRespectToBeetleSizeAndSpeed();
        Assert.That(BeetleHelper.GetSizeProperty(_beetleObject), Is.EqualTo(_sizeSlider.Value),
            $"Beetle object size expected to be the same as slider value ({_sizeSlider.Value}) but was ({BeetleHelper.GetSizeProperty(_beetleObject)})");
        _sizeSlider.Value = 14;
        AssertDispatcherTimerIntervalWithRespectToBeetleSizeAndSpeed();
        Assert.That(BeetleHelper.GetSizeProperty(_beetleObject), Is.EqualTo(_sizeSlider.Value),
            $"Beetle object size expected to be the same as slider value ({_sizeSlider.Value}) but was ({BeetleHelper.GetSizeProperty(_beetleObject)})");
        Assert.That(Convert.ToDouble(_sizeLabel.Content), Is.EqualTo(_sizeSlider.Value),
            $"Label for size expected to be the same as slider value ({_sizeSlider.Value}) but was ({_sizeLabel.Content})");
    }

    [MonitoredTest("MainWindow - Changing speed slider should set property on Beetle object and interval on timer"), Order(18)]
    public void _M18_ShouldChangeTimerIntervalSpeedPropertyAndLabelWhenChangingSliderValue()
    {
        AssertHasDispatcherTimer();
        AssertDispatcherTimerIntervalWithRespectToBeetleSizeAndSpeed();
        Assert.That(BeetleHelper.GetSpeedProperty(_beetleObject), Is.EqualTo(_speedSlider.Value),
            $"Beetle object speed expected to be the same as slider value ({_speedSlider.Value}) but was ({BeetleHelper.GetSpeedProperty(_beetleObject)})");
        _speedSlider.Value = 5.5;
        AssertDispatcherTimerIntervalWithRespectToBeetleSizeAndSpeed();
        Assert.That(BeetleHelper.GetSpeedProperty(_beetleObject), Is.EqualTo(_speedSlider.Value),
            $"Beetle object size expected to be the same as slider value ({_speedSlider.Value}) but was ({BeetleHelper.GetSpeedProperty(_beetleObject)})");
        Assert.That(Convert.ToDouble(_speedLabel.Content), Is.EqualTo(_speedSlider.Value),
           $"Label for speed expected to be the same as slider value ({_speedSlider.Value}) but was ({_speedLabel.Content})");
    }

    [MonitoredTest("MainWindow - Should move the beetle after every Tick"), Order(19)]
    public void _M19_ShouldMoveBeetleAfterEveryTick()
    {
        var code = Solution.Current.GetFileContent(@"BeetleGame\MainWindow.xaml.cs");
        Assert.That(code, Is.Not.Null);

        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        var root = syntaxTree.GetRoot();
        var method = root
            .DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(md => md.Identifier.ValueText.Contains("Tick"));
        Assert.That(method, Is.Not.Null,
            "Could not find the 'Tick' event handler. You have to create a DispatcherTimer object with a handler method for the Tick event.");

        var statements = method.Body.Statements;
        bool changePositionCallFound = false;
        foreach (var statement in statements)
        {
            changePositionCallFound = statement.ToString().Contains("ChangePosition()");
        }
        Assert.That(changePositionCallFound, Is.True,
            "Tick event handler does not call the ChangePosition method on Beetle.");
    }

    private void AssertHasDispatcherTimer()
    {
        Assert.That(_dispatcherTimer, Is.Not.Null, "Mainwindow should have a private member variable of class DispatcherTimer");
    }

    private void AssertDispatcherTimerIntervalWithRespectToBeetleSizeAndSpeed()
    {
        // smallest beetle (10 pixels) moves 0.1 cm per step
        // a beetle (12 pixels) moves 0.12 cm per step
        // ...
        // largest beetle (20 pixel) moves 0.2cm per step
        // a tick for the smalles beetle: 1/10 sec interval => moves 1 cm / sec
        // a tick for the largest beetle: 1/20 sec interval => movies 2 cm / sec
        TimeSpan expectedInterval = TimeSpan.FromMilliseconds(100 / _speedSlider.Value * _sizeSlider.Value / 10);
        Assert.That(_dispatcherTimer.Interval, Is.EqualTo(expectedInterval), 
            "Timer Interval should be set to 100 / speedSlider.Value * sizeSlider.Value / 10 (in msec)");
    }
}
