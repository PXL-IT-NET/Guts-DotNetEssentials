using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Guts.Client.Classic;
using Guts.Client.Classic.TestTools.WPF;
using Guts.Client.Shared;
using NUnit.Framework;

namespace Exercise11.Tests
{
    [ExerciseTestFixture("dotNet1", "H07", "Exercise11", @"Exercise11\MainWindow.xaml;Exercise11\MainWindow.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _window;

        private IList<Button> _buttons;
        private IList<Label> _labels;
        private IList<Random> _randomFields;
        private IList<TextBlock> _textBlocks;


        [SetUp]
        public void SetUp()
        {
            _window = new TestWindow<MainWindow>();

            _buttons = _window.GetUIElements<Button>();
            _labels = _window.GetUIElements<Label>();
            _textBlocks = _window.GetUIElements<TextBlock>().Where(tb => !string.IsNullOrEmpty(tb.Name)).ToList(); //where needed because the templates of the buttons also contain a textblock
            _randomFields = _window.GetPrivateFields<Random>();
        }

        [TearDown]
        public void TearDown()
        {
            _window?.Dispose();
        }

        [MonitoredTest("Should have all controls"), Order(1)]
        public void _1_ShouldHaveAllControls()
        {
            AssertAllControlsArePresent();
        }


        [MonitoredTest("Should have 2 fields of type Random"), Order(2)]
        public void _2_ShouldHave2FieldsOfTypeRandom()
        {
            AssertRandomGeneratorsArePresent();
        }

        [MonitoredTest("Should show the next random number in the matching label when the button is clicked"), Order(3)]
        public void _3_ShouldShowTheNextRandomNumberInTheMatchingLabelOnButtonClick()
        {
            AssertAllControlsArePresent();
            AssertRandomGeneratorsArePresent();

            var oldLabelContents = _labels.Select(l => Convert.ToString(l.Content)).Concat(_textBlocks.Select(tb => tb.Text))
                .ToList();

            _buttons.ElementAt(0).FireClickEvent();
            _buttons.ElementAt(1).FireClickEvent();

            var newLabelContents = _labels.Select(l => Convert.ToString(l.Content)).Concat(_textBlocks.Select(tb => tb.Text))
                .ToList();

            var firstLabelContent = newLabelContents.ElementAt(0);
            var secondLabelContent = newLabelContents.ElementAt(1);

            Assert.That(oldLabelContents, Has.All.Matches((string oldLabelContent) => oldLabelContent != firstLabelContent), 
                () => "The content of the first label did not change after a click on the first button");
            Assert.That(oldLabelContents, Has.All.Matches((string oldLabelContent) => oldLabelContent != secondLabelContent), 
                () => "The content of the second label did not change after a click on the second button");

            int randomNumber;
            Assert.That(int.TryParse(firstLabelContent, out randomNumber), Is.True, 
                () => $"The contents of the first label should be a number but was '{firstLabelContent}'");
            Assert.That(int.TryParse(secondLabelContent, out randomNumber), Is.True, 
                () => $"The contents of the second label should be a number but was '{secondLabelContent}'");
        }

        [MonitoredTest("Should generate different numbers in each label"), Order(4)]
        public void _4_ShouldGenerateDifferentNumbersInEachLabel()
        {
            AssertAllControlsArePresent();
            AssertRandomGeneratorsArePresent();

            foreach (var label in _labels)
            {
                label.Content = "";
            }
            foreach (var textblock in _textBlocks)
            {
                textblock.Text = "";
            }

            _buttons.ElementAt(0).FireClickEvent();
            _buttons.ElementAt(1).FireClickEvent();

            var newLabelContents = _labels.Select(l => (string)l.Content).Concat(_textBlocks.Select(tb => tb.Text))
                .ToList();

            var firstLabelContent = newLabelContents.ElementAt(0);
            var secondLabelContent = newLabelContents.ElementAt(1);

            Assert.That(firstLabelContent, Is.Not.EqualTo(secondLabelContent));
        }

        private void AssertAllControlsArePresent()
        {
            var textDisplayControlCount = _labels.Count + _textBlocks.Count;
            Assert.That(textDisplayControlCount, Is.EqualTo(2),
                () => $"The window should have exactly 2 controls to display text (Label or TextBlock). Number of Labels that were found: {_labels.Count}. Number of TextBlocks that were found: {_textBlocks.Count}");
            Assert.That(_buttons.Count, Is.EqualTo(2),
                () => $"The window should have exactly 2 Button controls. Number of Buttons that were found: {_buttons.Count}");
        }

        private void AssertRandomGeneratorsArePresent()
        {
            Assert.That(_randomFields.Count, Is.EqualTo(2),
                () =>
                    $"The window should have 2 fields of type Random. Number of fields that were found: {_randomFields.Count}");
        }


    }
}
