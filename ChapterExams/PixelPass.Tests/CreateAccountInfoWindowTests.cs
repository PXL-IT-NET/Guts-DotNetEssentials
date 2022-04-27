using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using NUnit.Framework;
using PixelPass.Tests.Helpers;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;

namespace PixelPass.Tests;

[ExerciseTestFixture("dotNet1", "Exams", "PixelPass", @"PixelPass\CreateAccountInfoWindow.xaml.cs")]
[Apartment(ApartmentState.STA)]
public class CreateAccountInfoWindowTests
{
    private static readonly Random Random = new Random();

    private CreateAccountInfoWindow _testWindow;
    private IAccountInfoCollection _accountInfoCollectionParameter;

    private IAccountInfoCollection _accountInfoCollectionField;
    private AccountInfo _accountInfoField;
    private TextBox _titleTextBox;
    private TextBox _usernameTextBox;
    private TextBox _passwordTextBox;
    private TextBox _notesTextBox;
    private Slider _expirationSlider;
    private TextBlock _expirationDateTextBlock;
    private TextBlock _passwordStrengthTextBlock;
    private Button _createButton;

    private string _windowClassContent;

    [OneTimeSetUp]
    public void BeforeAllTests()
    {
        _windowClassContent = Solution.Current.GetFileContent(@"PixelPass\CreateAccountInfoWindow.xaml.cs");
    }

    [SetUp]
    public void SetUp()
    {
        var accountInfoList = AccountInfoCollectionHelper.CreateTestListWithAccountInfo();
        var mockAccountInfoCollection = new Mock<IAccountInfoCollection>();
        mockAccountInfoCollection.SetupGet(aic => aic.AccountInfos).Returns(accountInfoList);
        _accountInfoCollectionParameter = mockAccountInfoCollection.Object;
    }

    [TearDown]
    public void TearDown()
    {
        _testWindow?.Close();
    }

    [MonitoredTest("CreateAccountInfoWindow - class should have a constructor with parameter IAccountInfoCollection"), Order(1)]
    public void _01_CreateAccountInfoWindow_ShouldHaveConstructorWithParameterIAccountInfoCollection()
    {
        Assert.That(CreateAccountInfoWindowHelper.HasConstructorWithParameterIAccountInfoCollection(), Is.True);
    }

    [MonitoredTest("CreateAccountInfoWindow - moving slider should update expirationTextBlock"), Order(2)]
    public void _02_MovingSlider_ShouldUpdateExpirationTextBlock()
    {
        InitializeWindow(_accountInfoCollectionParameter);

        _expirationSlider.Value = Random.Next(31) + 1;
        Assert.That(_accountInfoField.Expiration.ToString("dd/MM/yyyy"),
            Is.EqualTo($"{(DateTime.Now + TimeSpan.FromDays(_expirationSlider.Value)):dd/MM/yyyy}"),
            "_accountInfo.Expiration should be updated with date of today, added with the value of expirationSlider");

        Assert.That(_expirationDateTextBlock.Text, Is.EqualTo($"({_accountInfoField.Expiration:dd/MM/yyyy})"),
            "expirationDateTextBlock should be updated with the expirationDate of the _accountInfo field");
    }

    [MonitoredTest("CreateAccountInfoWindow - updating password should update strength"), Order(3)]
    public void _03_UpdatingPasswordTextBox_ShouldUpdateStrength()
    {
        // Arrange
        InitializeWindow(_accountInfoCollectionParameter);

        // Act
        _passwordTextBox.Text = "abcdefgh";
        // Assert
        Assert.That(_passwordStrengthTextBlock.Text, Is.EqualTo("(Weak)"),
            $"passWordTextBox contains {_passwordTextBox.Text}, so passwordStrengthTextBlock should be (Weak)");

        // Act
        _passwordTextBox.Text = "abcdefghA";
        // Assert
        Assert.That(_passwordStrengthTextBlock.Text, Is.EqualTo("(Average)"),
            $"passWordTextBox contains {_passwordTextBox.Text}, so passwordStrengthTextBlock should be (Weak)");

        // Act
        _passwordTextBox.Text = "abcdefghA1234";
        // Assert
        Assert.That(_passwordStrengthTextBlock.Text, Is.EqualTo("(Strong)"),
            $"passWordTextBox contains {_passwordTextBox.Text}, so passwordStrengthTextBlock should be (Strong)");

        // The code should use also PasswordValidator
        var syntaxTree = CSharpSyntaxTree.ParseText(_windowClassContent);
        var root = syntaxTree.GetRoot();
        MethodDeclarationSyntax passwordTextBoxTextChangedMethod = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(md => (md.Identifier.ValueText.Equals("PasswordTextBox_TextChanged")) ||
                                  (md.Identifier.ValueText.Equals("passwordTextBox_TextChanged")));

        Assert.That(passwordTextBoxTextChangedMethod, Is.Not.Null,
            "Cannot find a method 'PasswordTextBox_TextChanged' or 'passwordTextBox_TextChanged' in CreateAccountInfoWindow.xaml.cs");

        var bodyBuilder = new StringBuilder();
        foreach (var statement in passwordTextBoxTextChangedMethod.Body.Statements)
        {
            bodyBuilder.AppendLine(statement.ToString());
        }
        string body = bodyBuilder.ToString();

        Assert.That(body, Contains.Substring("CalculateStrength"),
            "The method 'CalculateStrength' from class PasswordValidator should be used to calculate password strength");
    }

    [MonitoredTest("CreateAccountInfoWindow - clicking the create button should create new AccountInfo and add to the collection"), Order(4)]
    public void _04_ClickingCreateButton_ShouldCreateNewAccountInfoAndAddToCollection()
    {
        // Arrange
        InitializeWindow(_accountInfoCollectionParameter);
        _titleTextBox.Text = "TestCompany";
        _usernameTextBox.Text = "TestUser";
        _notesTextBox.Text = "Some Test Notes";
        _passwordTextBox.Text = "SomeStrongPassWord12345";
        var numberOfAccountsBeforeAdd = _accountInfoCollectionField.AccountInfos.Count();

        // Act
        _createButton.FireClickEvent();

        //Assert
        Assert.That(_accountInfoCollectionField.AccountInfos.Count(), Is.EqualTo(numberOfAccountsBeforeAdd + 1),
            "A new AccountInfo object should be added to the _accountInfoCollection.AccountInfos list");

        // Check that the window gets closed: use Roslyn for that
        var syntaxTree = CSharpSyntaxTree.ParseText(_windowClassContent);
        var root = syntaxTree.GetRoot();
        MethodDeclarationSyntax createButtonClickMethod = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(md => (md.Identifier.ValueText.Equals("CreateButton_Click")) ||
                                  (md.Identifier.ValueText.Equals("createButton_Click")));

        Assert.That(createButtonClickMethod, Is.Not.Null,
            "Cannot find a method 'CreateButton_Click' or 'createButton_Click' in CreateAccountInfoWindow.xaml.cs");

        var bodyBuilder = new StringBuilder();
        foreach (var statement in createButtonClickMethod.Body.Statements)
        {
            bodyBuilder.AppendLine(statement.ToString());
        }
        string body = bodyBuilder.ToString();

        Assert.That(body, Contains.Substring("Close()"),
            "CreateAccountInfoWindow should close itself using the Close() method");
    }

    private void InitializeWindow(IAccountInfoCollection accountInfoCollection)
    {
        _testWindow = CreateAccountInfoWindowHelper.Create(accountInfoCollection);
        Assert.That(_testWindow, Is.Not.Null, "Failed to create a CreateAccountInfoWindow");

        _accountInfoCollectionField = _testWindow.GetPrivateFieldValue<IAccountInfoCollection>();
        Assert.That(_accountInfoCollectionField, Is.Not.Null, 
            "CreateAccountInfoWindow should have a private field of type IAccountInfoCollection and set its value from the constructor parameter");

        _accountInfoField = _testWindow.GetPrivateFieldValue<AccountInfo>();
        Assert.That(_accountInfoField, Is.Not.Null,
            "CreateAccountInfoWindow should have a private field of type AccountInfo and set it to a new instance");

        _expirationSlider = _testWindow.GetPrivateFieldValue<Slider>();
        Assert.That(_expirationSlider, Is.Not.Null,
            "CreateAccountInfoWindow should have a Slider called expirationSlider, did you remove it?");

        _titleTextBox = _testWindow.GetPrivateFieldValueByName<TextBox>("titleTextBox");
        Assert.That(_titleTextBox, Is.Not.Null,
            "CreateAccountInfoWindow should have a TextBox called titleTextBox, did you remove it?");

        _usernameTextBox = _testWindow.GetPrivateFieldValueByName<TextBox>("usernameTextBox");
        Assert.That(_usernameTextBox, Is.Not.Null,
            "CreateAccountInfoWindow should have a TextBox called passwordTextBox, did you remove it?");

        _passwordTextBox = _testWindow.GetPrivateFieldValueByName<TextBox>("passwordTextBox");
        Assert.That(_passwordTextBox, Is.Not.Null,
            "CreateAccountInfoWindow should have a TextBox called usernameTextBox, did you remove it?");

        _notesTextBox = _testWindow.GetPrivateFieldValueByName<TextBox>("notesTextBox");
        Assert.That(_notesTextBox, Is.Not.Null,
            "CreateAccountInfoWindow should have a TextBox called notesTextBox, did you remove it?");

        _expirationDateTextBlock = _testWindow.GetPrivateFieldValueByName<TextBlock>("expirationDateTextBlock");
        Assert.That(_expirationDateTextBlock, Is.Not.Null,
            "CreateAccountInfoWindow should have a TextBlock called expirationDateTextBlock, did you remove it?");

        _passwordStrengthTextBlock = _testWindow.GetPrivateFieldValueByName<TextBlock>("passwordStrengthTextBlock");
        Assert.That(_passwordStrengthTextBlock, Is.Not.Null,
            "CreateAccountInfoWindow should have a TextBlock called passwordStrengthTextBlock, did you remove it?");
        Assert.That(_passwordStrengthTextBlock.Text, Is.EqualTo($"(Weak)"),
            "passwordStrengthTextBlock should be initialized (weak) as there is no password yet");

        Assert.That(_accountInfoField.Expiration.ToString("dd/MM/yyyy"),
            Is.EqualTo($"{(DateTime.Now + TimeSpan.FromDays(_expirationSlider.Value)):dd/MM/yyyy}"),
            "_accountInfo.Expiration should be initialized with date of today, added with the value of expirationSlider");

        Assert.That(_expirationDateTextBlock.Text, Is.EqualTo($"({_accountInfoField.Expiration:dd/MM/yyyy})"),
            "expirationDateTextBlock should be initialized with the expirationDate of the _accountInfo field");

        _createButton = _testWindow.GetPrivateFieldValueByName<Button>("createButton");
        Assert.That(_createButton, Is.Not.Null,
           "CreateAccountInfoWindow should have a Button called createButton, did you remove it?");

        _testWindow.Show();
    }
}
