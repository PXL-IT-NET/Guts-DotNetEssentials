# Guts-DotNetEssentials
In this repository you can find (visual studio) start solutions for the exercises of the **.NET Essentials** course of **PXL-IT**.

![alt text][img_book]

The exercises are grouped by chapter. There is a (visual studio) solution for each chapter.
A chapter solution contains multiple projects:
- A WPF project for each exercise. E.g. *Exercise01*
- A test project for each WPF project. E.g. *Exercise01.Tests*

![alt text][img_projects]

The WPF solutions are empty and waiting for you to complete them.
The matching test projects contain **automated tests** that can be run to check if your solution is correct.

## Getting Started
First you need to clone or download the files in this repository on your local machine.
You have 2 options:
1. Clone the repository using git (recommended because with git it is easier to deal with changes. Also see the [FAQ](/FAQ.md))
2. Download a zipped copy of the repository

#### Clone the repository
If you are familiar with *Git* you can clone this repository to your local machine.
Click on "Clone or download" at the top right of this page.
Click on "Open in Desktop" if you use the *GitHub Desktop* tool or copy the web URL and clone using the command line or your favorite git tool.

![alt text][img_clone]

Working with git is the recommended way because it makes is very easy to get bugfixes on the tests without loosing any of your work. Also see the [FAQ](/FAQ.md).

#### Download a zipped coppy of the repository
Click on "Clone or download" at the top right of this page.
Click on "Download ZIP"

![alt text][img_download]

Unzip de files on your local machine and you are ready to go.

#### Register on [guts-web.appspot.com](https://guts-web.appspot.com)
To be able to send your tests results to the Guts servers you need to register via [guts-web.appspot.com](https://guts-web.appspot.com/register).
After registration you will have the credentials you need to succesfully run automated tests for an exercise.

#### Start working on an exercise
Let's assume you want to make exercise 5 of chapter 5.
1. Open the solution in the folder "Chapter 5". You can do this by doubleclicking on the **.sln** file or by opening visual studio, clicking on *File -> Open -> Project/Solution...* and selecting the **.sln** file.
![alt text][img_open_solution]
2. **Build the solution** (Menu: Build -> Build Solution or Ctrl+Shift+B)
3. Locate the project "Exercise 5" and set it as your startup project
![alt text][img_startup_project]
4. Write the code you need to write

#### Run the automated tests
Let's assume you are working on exercise 5 of chapter 5.
1. Open the *Test Explorer* window (Menu: Test -> Windows -> Test Explorer)
2. In the top left corner, right click on the down arrow of the *group by* button and group the automated tests by project. (If you don't see any tests appearing, you probably should (re)build your solution)
![alt text][img_group_tests]
3. Right click on the project that matches your exercise and click on *Run selected tests*
4. The first time you run a test a popup may appear thats asks you to log in. You should fill in your credentials from [guts-web.appspot.com](https://guts-web.appspot.com).
![alt text][img_login_vs]

#### Inspect the test results
Tests that pass will be green. Tests that don't pass will be red. 

The *name of the test* gives an indication of what is tested in the automated test.
If you click on a test you can also read more detailed messages that may help you to find out what is going wrong.

![alt text][img_test_detail]

Although it is not a guarantee, having all tests green is a good indication that you completed the exercise correctly.

#### Check your results online
Test results of all students are sent to the Guts servers.
You can check your progress and compare with the averages of other students via [guts-web.appspot.com](https://guts-web.appspot.com).
Login, go to ".NET Essentials" in the navigation bar and select the chapter you want to view.
![alt text][img_chapter_contents]

## Questions?
Maybe the [FAQ](/FAQ.md) page already has the answer...

[img_book]:Images/book.jpg "Handboek 'Programmeren in C#'"
[img_projects]:Images/projects.png "Solution for chapter five with its projects"
[img_download]:Images/download.png "Download repository"
[img_clone]:Images/clone.png "Clone repository"
[img_open_solution]:Images/open_solution.png "Open solution"
[img_startup_project]:Images/startup_project.png "Choose startup project"
[img_group_tests]:Images/group_tests.png "Group tests by project"
[img_test_detail]:Images/test_detail.png "Details of a test result"
[img_login_vs]:Images/login_vs.png "Visual studio login"
[img_chapter_contents]:Images/chaptercontents.png "Chapter contents"

