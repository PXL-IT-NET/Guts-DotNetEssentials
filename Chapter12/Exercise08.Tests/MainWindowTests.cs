using Guts.Client.Classic;
using Guts.Client.Classic.TestTools.WPF;
using Guts.Client.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Exercise08.Tests
{
    [ExerciseTestFixture("dotNet1", "H12", "Exercise08", @"Exercise08\MainWindow.xaml;Exercise08\MainWindow.xaml.cs"),
     Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _mainWindow;
        private TextBlock _errorTextBlock;
        private readonly string _errorTextBlockName = "errorTextBlock";
        private TextBlock _areaTextBlock;
        private readonly string _areaTextBlockName = "areaTextBlock";

        [SetUp]
        public void SetUp()
        {
            _mainWindow = new TestWindow<MainWindow>();
            _errorTextBlock = _mainWindow.GetUIElements<TextBlock>().FirstOrDefault((tb) => tb.Name == _errorTextBlockName);
            _areaTextBlock = _mainWindow.GetUIElements<TextBlock>().FirstOrDefault((tb) => tb.Name == _areaTextBlockName);
        }

        [TearDown]
        public void TearDown()
        {
            _mainWindow?.Dispose();
        }

        [MonitoredTest("Should have two TextBlocks for area and error"), Order(1)]
        public void _1_ShouldHaveTwoTextBlocksForAreaAndError()
        {
            AssertHaveTwoTextBlocks();
        }

        private void AssertHaveTwoTextBlocks()
        {
            Assert.That(_areaTextBlock, Is.Not.Null, () => $"No TextBlock found with name = {_areaTextBlockName}");
            Assert.That(_errorTextBlock, Is.Not.Null, () => $"No TextBlock found with name = {_errorTextBlockName}");
        }
    }
}
