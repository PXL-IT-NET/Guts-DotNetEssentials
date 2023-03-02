using System;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;
using System.Windows;

namespace Exercise07.Tests
{
    [ExerciseTestFixture("dotNet1", "H08", "Exercise07", @"Exercise07\MainWindow.xaml;Exercise07\MainWindow.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _testWindow;

        private TextBox _sizeTextBox;
        private Button _drawButton;
        private TextBox _tableTextBox;

        [SetUp]
        public void Setup()
        {
            _testWindow = new MainWindow();
            Grid grid = (Grid)_testWindow.Content;

            _drawButton = grid.FindVisualChildren<Button>().ToList().FirstOrDefault();

            var allTextBoxes = grid.FindVisualChildren<TextBox>().ToList();
            _sizeTextBox = allTextBoxes.Find(tb => tb.Name.ToLower().Contains("size"));
            _tableTextBox = allTextBoxes.Find(tb => tb.Name.ToLower().Contains("table"));
        }

        [TearDown]
        public void TearDown()
        {
            _testWindow?.Close();
        }

        [MonitoredTest("Should have two TextBoxes and a Button"), Order(1)]
        public void _1_ShouldHaveTwoTextBoxAndButton()
        {
            ValidateControls();
        }

        [MonitoredTest("Should make multiplication table"), Order(2)]
        [TestCase(1, "\t1\t\r\n\n1\t1\t")]
        [TestCase(5, "\t1\t2\t3\t4\t5\t\r\n\n1\t1\t2\t3\t4\t5\t\n2\t2\t4\t6\t8\t10\t\n3\t3\t6\t9\t12\t15\t\n4\t4\t8\t12\t16\t20\t\n5\t5\t10\t15\t20\t25\t")]
        [TestCase(10, "\t1\t2\t3\t4\t5\t6\t7\t8\t9\t10\t\r\n\n1\t1\t2\t3\t4\t5\t6\t7\t8\t9\t10\t\n2\t2\t4\t6\t8\t10\t12\t14\t16\t18\t20" +
            "\t\n3\t3\t6\t9\t12\t15\t18\t21\t24\t27\t30\t\n4\t4\t8\t12\t16\t20\t24\t28\t32\t36\t40\t\n5\t5\t10\t15\t20\t25\t30\t35\t40\t45" +
            "\t50\t\n6\t6\t12\t18\t24\t30\t36\t42\t48\t54\t60\t\n7\t7\t14\t21\t28\t35\t42\t49\t56\t63\t70\t\n8\t8\t16\t24\t32\t40\t48\t56\t64" +
            "\t72\t80\t\n9\t9\t18\t27\t36\t45\t54\t63\t72\t81\t90\t\n10\t10\t20\t30\t40\t50\t60\t70\t80\t90\t100\t")]
        public void _2_ShouldMakeMultiplicationTable(int multiplicationNumber, string expectedResult)
        {
            ValidateControls();
            
            _sizeTextBox.Text = multiplicationNumber.ToString(); 

            _drawButton.FireClickEvent();

            var expectedLines = expectedResult.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var actualLines = _tableTextBox.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Assert.That(expectedLines.Length, Is.EqualTo(actualLines.Length),
                () =>
                    $"For the number {multiplicationNumber} there should be {expectedLines.Length} lines in the 'TextBox' " +
                    $"but was {actualLines.Length}.");


            for (var index = 0; index < expectedLines.Length; index++)
            {
                var expectedLine = expectedLines[index].Trim();
                var actualLine = actualLines[index].Trim();

                var lineNumber = index + 1;
                Assert.That(actualLine, Is.EqualTo(expectedLine),
                    () =>
                        $"For the number '{multiplicationNumber}', the line {lineNumber} should be '{expectedLine}' but was '{actualLine}'.");
            }
        }

        private void ValidateControls()
        {
            Assert.That(_sizeTextBox, Is.Not.Null,
                () => "Could not find a TextBox control called 'sizeTextBox' for entering the size of the multiplication table.");
            Assert.That(_drawButton, Is.Not.Null,
                () => "Could not find a Button control called sizeButton.");
            Assert.That(_tableTextBox, Is.Not.Null,
                () => "Could not find a TextBox control called 'tableTextBox' to display the results.");
            Assert.That(_tableTextBox.TextWrapping, Is.EqualTo(TextWrapping.NoWrap),
                () => "'tableTextBox' should set its 'TextWrapping' property to 'NoWrap'");
            Assert.That(_tableTextBox.VerticalScrollBarVisibility, Is.EqualTo(ScrollBarVisibility.Auto),
                () => "'tableTextBox' should set its 'VerticalScrollBarVisibility' property to 'Auto'");
            Assert.That(_tableTextBox.HorizontalScrollBarVisibility, Is.EqualTo(ScrollBarVisibility.Auto),
               () => "'tableTextBox' should set its 'HorizontalScrollBarVisibility' property to 'Auto'");
        }
    }
}
