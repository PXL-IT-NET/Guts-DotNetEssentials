using System;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Guts.Client.Classic;
using Guts.Client.Classic.TestTools.WPF;
using Guts.Client.Shared;

namespace Exercise07.Tests
{
    [ExerciseTestFixture("dotNet1", "H08", "Exercise07", @"Exercise07\MainWindow.xaml;Exercise07\MainWindow.xaml.cs"),
     Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _testWindow;

        private TextBox _textBox;
        private Button _button;
        private TextBlock _textBlock;

        [SetUp]
        public void Setup()
        {
            _testWindow = new TestWindow<MainWindow>();
            _textBox = _testWindow.GetUIElements<TextBox>().FirstOrDefault();
            _button = _testWindow.GetContentControlByPartialContentText<Button>("Teken");
            _textBlock = _testWindow.GetUIElements<TextBlock>().FirstOrDefault(tb => tb.Parent is Grid);
        }

        [TearDown]
        public void TearDown()
        {
            _testWindow?.Dispose();
        }

        [MonitoredTest("Should have a TextBox, Button and TextBlock"), Order(1)]
        public void _1_ShouldHaveATextBoxAndButtonAndTextBlock()
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
            
            _textBox.Text = multiplicationNumber.ToString(); 

            _button.FireClickEvent();

            var expectedLines = expectedResult.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var actualLines = _textBlock.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

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
            Assert.That(_textBox, Is.Not.Null,
                () => "Could not find a TextBox control.");
            Assert.That(_button, Is.Not.Null,
                () => "Could not find a Button control with the text 'Teken'.");
            Assert.That(_textBlock, Is.Not.Null,
                () => "Could not find a TextBlock control in the 'Grid' to display the results.");
        }
    }
}
