using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using PixelPass.Tests.Helpers;

namespace PixelPass.Tests;

[ExerciseTestFixture("dotNet1", "Exams", "PixelPass", @"PixelPass\MainWindow.xaml.cs")]
[Apartment(ApartmentState.STA)]
public class MainWindowTests
{
    private MainWindow _testWindow;
    private ListBox _accountInfoListBox;
    private AccountInfo _currentAccountInfo;
    private TextBlock _titleTextBlock;
    private TextBlock _usernameTextBlock;
    private TextBlock _notesTextBlock;
    private TextBlock _expirationTextBlock;
    private Canvas _detailsCanvas;
    private Button _copyButton;
    private DispatcherTimer _copyTimer;
    private ProgressBar _expirationProgressBar;

    private string _windowClassContent;

    [OneTimeSetUp]
    public void BeforeAllTests()
    {
        _windowClassContent = Solution.Current.GetFileContent(@"PixelPass\MainWindow.xaml.cs");
        Assert.That(_windowClassContent, Is.Not.Null);
    }

    [SetUp]
    public void Setup()
    {
        _testWindow = new MainWindow();
        _accountInfoListBox = _testWindow.GetPrivateFieldValue<ListBox>();
        _titleTextBlock = _testWindow.GetPrivateFieldValueByName<TextBlock>("titleTextBlock");
        _usernameTextBlock = _testWindow.GetPrivateFieldValueByName<TextBlock>("usernameTextBlock");
        _notesTextBlock = _testWindow.GetPrivateFieldValueByName<TextBlock>("notesTextBlock");
        _expirationTextBlock = _testWindow.GetPrivateFieldValueByName<TextBlock>("expirationTextBlock");
        _detailsCanvas = _testWindow.GetPrivateFieldValueByName<Canvas>("detailsCanvas");
        _copyButton = _testWindow.GetPrivateFieldValueByName<Button>("copyButton");
        _expirationProgressBar = _testWindow.GetPrivateFieldValueByName<ProgressBar>("expirationProgressBar");
        try
        {
            _copyTimer = _testWindow.GetPrivateFieldValue<DispatcherTimer>();
        } catch (Exception)
        {
            _copyTimer = null;
        }
    }

    [TearDown]
    public void TearDown()
    {
        _testWindow?.Close();
    }

    [MonitoredTest("MainWindow - pietpienter.txt should be available on the desktop"), Order(1)]
    public void _01_PietPienterFile_ShouldBeCopiedToTheDesktopByTheUser()
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string[] files = Directory.GetFileSystemEntries(folderPath, "pietpienter.txt");

        Assert.That(new List<string>(files).Count, Is.EqualTo(1),
                        "pietpienter.txt should be be available on the desktop");
    }

    [MonitoredTest("MainWindow - Open Item should have a click event handler"), Order(2)]
    public void _02_OpenItem_ShouldHave_ClickEventHandler()
    {
        MethodDeclarationSyntax method = FindOpenItemClickHandlerSource();
        Assert.That(method, Is.Not.Null,
            "Could not find OpenItem_Click event handler");
    }

    [MonitoredTest("MainWindow - Open Item should call OpenFileDialog"), Order(3)]
    public void _03_OpenItem_ShouldCall_OpenFileDialog()
    {
        MethodDeclarationSyntax method = FindOpenItemClickHandlerSource();
        Assert.That(method, Is.Not.Null,
            "Could not find OpenItem_Click event handler");

        var statements = method.Body.Statements;
        var statement = statements.Where(st => st.ToString().Contains("OpenFileDialog()"))
                                                    .FirstOrDefault();
        Assert.That(statement, Is.Not.Null,
            "OpenItem_Click handler should create an OpenFileDialog instance");
    }

    [MonitoredTest("MainWindow - Open Item should mention to Environment.SpecialFolder.Desktop"), Order(4)]
    public void _04_OpenItem_ShouldMention_EnvironmentSpecialFolderDesktop()
    {
        MethodDeclarationSyntax method = FindOpenItemClickHandlerSource();
        Assert.That(method, Is.Not.Null,
            "Could not find OpenItem_Click event handler");

        var statements = method.Body.Statements;
        var statement = statements.Where(st => st.ToString().Contains("Environment.SpecialFolder.Desktop"))
                                                    .FirstOrDefault();
        Assert.That(statement, Is.Not.Null,
            "OpenItem_Click handler should point the OpenFileDialog to Environment.SpecialFolder.Desktop");
    }

    [MonitoredTest("MainWindow - Open Item should call AccountInfoCollectionReader_Read"), Order(5)]
    public void _05_OpenItem_ShouldCall_AccountInfoCollectionReaderRead()
    {
        MethodDeclarationSyntax method = FindOpenItemClickHandlerSource();
        Assert.That(method, Is.Not.Null,
            "Could not find OpenItem_Click event handler");

        var statements = method.Body.Statements;
        var statement = statements.Where(st => st.ToString().Contains("AccountInfoCollectionReader.Read"))
                                                    .FirstOrDefault();

        Assert.That(statement, Is.Not.Null,
           "OpenItem_Click handler should call AccountInfoCollectionReader.Read");

    }

    [MonitoredTest("MainWindow - Open Item should set accountInfoListBox.ItemsSource"), Order(6)]
    public void _06_OpenItem_ShouldSet_accountInfoListBoxItemsSource()
    {
        MethodDeclarationSyntax method = FindOpenItemClickHandlerSource();
        Assert.That(method, Is.Not.Null,
            "Could not find OpenItem_Click event handler");

        var statements = method.Body.Statements;
        var statement = statements.Where(st => st.ToString().Contains("accountInfoListBox.ItemsSource"))
                                                    .FirstOrDefault();

        Assert.That(statement, Is.Not.Null,
           "OpenItem_Click handler should set accountInfoListBox.ItemsSource to AccountInfos property from AccountInfoCollection");
    }

    [MonitoredTest("MainWindow - Open Item should catch ParseException"), Order(7)]
    public void _07_OpenItem_ShouldCatch_ParseException()
    {
        MethodDeclarationSyntax method = FindOpenItemClickHandlerSource();
        Assert.That(method, Is.Not.Null,
            "Could not find OpenItem_Click event handler");

        var statements = method.Body.Statements;
        var statement = statements.Where(st => st.ToString().Contains("catch (ParseException"))
                                                    .FirstOrDefault();

        Assert.That(statement, Is.Not.Null,
           "OpenItem_Click handler should catch a ParseException to display a MessageBox if the file seems corrupt.");
    }

    [MonitoredTest("MainWindow - Open Item should enable newAccountInfoButton"), Order(8)]
    public void _08_OpenItem_ShouldEnable_NewAccountInfoButton()
    {
        MethodDeclarationSyntax method = FindOpenItemClickHandlerSource();
        Assert.That(method, Is.Not.Null,
            "Could not find OpenItem_Click event handler");

        var statements = method.Body.Statements;
        var statement = statements.Where(st => st.ToString().Contains("newAccountInfoButton.IsEnabled"))
                                                    .FirstOrDefault();

        Assert.That(statement, Is.Not.Null,
           "OpenItem_Click handler should enable newAccountInfoButton.");
    }

    [MonitoredTest("MainWindow - Selecting an account from the listbox should show the details"), Order(9)]
    public void _09_SelectingAnAccountFromListBox_ShouldShowDetails()
    {
        // Arrange
        Assert.That(_accountInfoListBox, Is.Not.Null, "There should be a ListBox on the MainWindow, did you remove it?");
        Assert.That(_titleTextBlock, Is.Not.Null, "There should be a TextBlock named titleTextBlock, did you remove it?");
        Assert.That(_usernameTextBlock, Is.Not.Null, "There should be a TextBlock named usernameTextBlock, did you remove it?");
        Assert.That(_notesTextBlock, Is.Not.Null, "There should be a TextBlock named notesTextBlock, did you remove it?");
        Assert.That(_expirationTextBlock, Is.Not.Null, "There should be a TextBlock named expirationTextBlock, did you remove it?");

        List<AccountInfo> testList = AccountInfoCollectionHelper.CreateTestListWithAccountInfo();
        AccountInfo testAccount = testList[0];
        _accountInfoListBox.ItemsSource = testList;

        // Act
        _accountInfoListBox.SelectedIndex = 0;
        Thread.Sleep(50); // wait for SelectionChanged event handler to complete

        //Assert
        Assert.That(_titleTextBlock.Text, Is.EqualTo(testAccount.Title),
                    $"titleTextBlock.Text should contain ({testAccount.Title})");
        Assert.That(_usernameTextBlock.Text, Is.EqualTo(testAccount.Username),
                    $"usernameTextBlock.Text should contain ({testAccount.Username})");
        Assert.That(_notesTextBlock.Text, Is.EqualTo(testAccount.Notes),
                    $"notesTextBlock.Text should contain ({testAccount.Notes})");
        Assert.That(_expirationTextBlock.Text, Is.EqualTo($"{testAccount.Expiration:dd/MM/yyyy}"),
                    $"expirationTextBlock.Text should contain ({testAccount.Expiration:dd/MM/yyyy})");
        _currentAccountInfo = _testWindow.GetPrivateFieldValueByName<AccountInfo>("_currentAccountInfo");
        Assert.That(_currentAccountInfo, Is.Not.Null, "There should be a private field called _currentAccountInfo, did you remove it?");
        Assert.That(_currentAccountInfo, Is.EqualTo(testAccount), "_currentAccount should contain the selected account from the listbox");
    }

    [MonitoredTest("MainWindow - Selecting an expired account from the listbox should make the detailsCanvas LightCoral"), Order(10)]
    public void _10_SelectingAnAccountFromListBox_ShouldChangeColorDetailsCanvasBasedOnExpiration()
    {
        // Arrange
        Assert.That(_accountInfoListBox, Is.Not.Null, "There should be a ListBox on the MainWindow, did you remove it?");
        Assert.That(_detailsCanvas, Is.Not.Null, "There should be a Canvas on the MainWindow, did you remove it?");

        List<AccountInfo> testList = AccountInfoCollectionHelper.CreateTestListWithAccountInfo();
        _accountInfoListBox.ItemsSource = testList;

        // Act
        _accountInfoListBox.SelectedIndex = 0;
        Thread.Sleep(50); // wait for SelectionChanged event handler to complete

        // Assert
        Assert.That(_detailsCanvas.Background, Is.InstanceOf<SolidColorBrush>());
        Assert.That((_detailsCanvas.Background as SolidColorBrush).Color, Is.EqualTo(Colors.LightCoral),
                    "detailsCanvas.Background should change to LightCoral for expired accounts");

        // Act
        _accountInfoListBox.SelectedIndex = 1;
        Thread.Sleep(50); // wait for SelectionChanged event handler to complete

        // Assert
        Assert.That(_detailsCanvas.Background, Is.InstanceOf<SolidColorBrush>());
        Assert.That((_detailsCanvas.Background as SolidColorBrush).Color, Is.EqualTo(Colors.White),
                    "detailsCanvas.Background should change to White for nonexpired accounts");
    }

    [MonitoredTest("MainWindow - Clicking the copy button should start the copy action"), Order(11)]
    public void _11_ClickingTheCopyButton_ShouldStartTheCopyAction()
    {
        // Arrange
        Assert.That(_accountInfoListBox, Is.Not.Null, "There should be a ListBox on the MainWindow, did you remove it?");
        Assert.That(_copyButton, Is.Not.Null, "There should be a copy Button on the MainWindow, did you remove it?");
        Assert.That(_expirationProgressBar, Is.Not.Null, "There should be a progressbar on the MainWindow, did you remove it?");
        Assert.That(_copyTimer, Is.Not.Null, "There should be a private member variabele of type DispatcherTimer for limiting copying time?");
        Assert.That(_copyTimer.Interval, Is.EqualTo(TimeSpan.FromSeconds(1)), "The dispatchertimer interval should be set to 1 sec");
        List<AccountInfo> testList = AccountInfoCollectionHelper.CreateTestListWithAccountInfo();
        _accountInfoListBox.ItemsSource = testList;
        _accountInfoListBox.SelectedIndex = 0;

        // Act
        _copyButton.FireClickEvent();

        Assert.That(Clipboard.GetText(), Is.EqualTo(_currentAccountInfo.Password),
            "The clipboard should contain the password of the _currentAccountInfo object");
        Assert.That(_copyButton.IsEnabled, Is.False,
            "The copyButton should be disabled while copying");
        Assert.That(_expirationProgressBar.Value, Is.GreaterThan(0),
            "The expirationProgressBar should contain a value indicating time the user has left for pasting");
        Assert.That(_copyTimer.IsEnabled, Is.True,
            "The _copyTimer should be started");
    }

    [MonitoredTest("MainWindow - Clicking the copy button should empty the clipboard upon expiration"), Order(12)]
    public void _12_ClickingTheCopyButton_ShouldEmptyTheClipboardUponExpiration()
    {
        // Arrange
        Assert.That(_accountInfoListBox, Is.Not.Null, "There should be a ListBox on the MainWindow, did you remove it?");
        Assert.That(_copyButton, Is.Not.Null, "There should be a copy Button on the MainWindow, did you remove it?");
        Assert.That(_expirationProgressBar, Is.Not.Null, "There should be a progressbar on the MainWindow, did you remove it?");
        Assert.That(_copyTimer, Is.Not.Null, "There should be a private member variabele of type DispatcherTimer for limiting copying time?");
        Assert.That(_copyTimer.Interval, Is.EqualTo(TimeSpan.FromSeconds(1)), "The dispatchertimer interval should be set to 1 sec");
        List<AccountInfo> testList = AccountInfoCollectionHelper.CreateTestListWithAccountInfo();
        AccountInfo testAccount = testList[1];
        _accountInfoListBox.ItemsSource = testList;
        _accountInfoListBox.SelectedIndex = 1;
        _copyTimer.Interval = TimeSpan.FromMilliseconds(1); // speed it up, just for testing

        // Act
        _copyButton.FireClickEvent();
        DispatcherUtil.DoEventsWhile(() => _expirationProgressBar.Value != 0, 1000);
        
        // Assert
        Assert.That(Clipboard.GetText(), Is.EqualTo(string.Empty),
            "Upon expiration, the clipboard should contain the empty string");
        Assert.That(_expirationProgressBar.Value, Is.EqualTo(0),
            "Upon expiration, the progressbar value should be 0");
        Assert.That(_copyButton.IsEnabled, Is.True,
            "Upon expiration, the copy button should be enabled again");
        Assert.That(_copyTimer.IsEnabled, Is.False,
            "Upon expiration, the copyTimer should be stopped");
    }

    private MethodDeclarationSyntax FindOpenItemClickHandlerSource()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(_windowClassContent);
        var root = syntaxTree.GetRoot();
        var method = root
            .DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(md => md.Identifier.ValueText.ToLower().Contains("openitem"));
        return method;
    }

    [MonitoredTest("MainWindow - Clicking the new accountinfo button should open CreateAccountInfoWindow and call Items.Refresh"), Order(13)]
    public void _13_ClickingTheNewAccountInfoButton_Should_OpenCreateAccountInfoWindow()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(_windowClassContent);
        var root = syntaxTree.GetRoot();
        MethodDeclarationSyntax newAccountInfoButtonClickMethod = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(md => (md.Identifier.ValueText.Equals("NewAccountInfoButton_Click")) ||
                                  (md.Identifier.ValueText.Equals("newAccountInfoButton_Click")));
        Assert.That(newAccountInfoButtonClickMethod, Is.Not.Null,
            "Cannot find a method 'NewAccountInfoButton_Click' or 'newAccountInfoButton_Click' in MainWindow.xaml.cs");
        var bodyBuilder = new StringBuilder();
        foreach (var statement in newAccountInfoButtonClickMethod.Body.Statements)
        {
            bodyBuilder.AppendLine(statement.ToString());
        }
        string body = bodyBuilder.ToString();

        Assert.That(body, Contains.Substring("new CreateAccountInfoWindow(_accountInfoCollection)"),
            "Clicking newAccountInfoButton should call the constructor of CreateAccountInfoWindow with parameter _accountInfoCollection");
        Assert.That(body, Contains.Substring("ShowDialog()"),
            "CreateAccountInfoWindow should be openend as a dialog window, giving it the only focus");
        Assert.That(body, Contains.Substring("accountInfoListBox.Items.Refresh()"),
            "Cliking newAccountInfoButton should refresh the Items collection of accountInfoListBox after the dialog window is closed.");
    }
}
