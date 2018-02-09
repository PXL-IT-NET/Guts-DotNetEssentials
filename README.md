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
1. Download a zipped copy of the repository
2. Clone the repository

#### Download a zipped coppy of the repository
Click on "Clone or download" at the top right of this page.
Click on "Download ZIP"

![alt text][img_download]

Unzip de files on your local machine and you are ready to go.

#### Clone the repository
If you are familiar with *Git* you can also choose to clone the repository to your local machine.
E.g. by using the *GitHub Desktop* tool.

#### Register on [guts-web.appspot.com](https://guts-web.appspot.com)
To be able to send your tests results to the Guts servers you need to register via [guts-web.appspot.com](https://guts-web.appspot.com/register).
After registration you will have the credentials you need to succesfully run automated tests for an exercise.

#### Start working on an exercise
Let's assume you want to make exercise 5 of chapter 5.
1. Open the solution in the folder "Chapter 5"
2. Locate the project "Exercise 5" and set it as your startup project
3. Write the code you need to write
4. (Re)build your project

![alt text][img_startup_project]

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

#### Check your results online (comming soon)
Test results of all students are sent to the Guts servers.
Soon you will be able to check your progress and compare with the averages of other students on  [guts-web.appspot.com](https://guts-web.appspot.com).

[img_book]:Images/book.jpg "Handboek 'Programmeren in C#'"
[img_projects]:Images/projects.png "Solution for chapter five with its projects"
[img_download]:Images/download.png "Download repository"
[img_startup_project]:Images/startup_project.png "Choose startup project"
[img_group_tests]:Images/group_tests.png "Group tests by project"
[img_test_detail]:Images/test_detail.png "Details of a test result"
[img_login_vs]:Images/login_vs.png "Visual studio login"

