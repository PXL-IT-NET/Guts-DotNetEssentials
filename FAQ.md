# FAQ

## How do I deal with updates / bugfixes on the start code?
When you are working on the exercises of a chapter and you get notified that the start code changed (e.g. after a bug fix in one of the tests), you want to get the latest version of the tests **without losing any work** you already did.

This can be achieved in two ways:
1. get the latest version of the code by doing a git pull request (this is the fastest way, but is only possible when you work with git and have a locally cloned repository)
2. download the start code as zip again and manually copy the changes you made (this is more cumbersome, but you don't have to know anything about git)

### method 1: do a pull request
With git you can *stash* (or set aside) your local changes, then pull (or get) the latest version of the online repository to finally reapply the stashed local changes.

To do this, follow these steps:
* Open a command prompt
* Navigate to the local folder in which you cloned the online repository
* Save your local changes by executing the command **git stash**
* Do a pull request that overwrites the local code with the code in the online repository: **git pull origin master**
* Reapply your stashed changes: **git stash apply**

### method 2: download a new version of the code
Most of the times it is only the MainWindow that has been changed. You can download the start code again and then copy the MainWindow files (.xaml and .cs) into your solution.

To do this, follow these steps:
* Download the repository again as zip. Unzip the file in a different folder then the folder you are currently working in
* Open the newly downloaded solution
* Right click on the project into which you want to copy the MainWindow code
* Choose "Add -> Existing Item..."
* Navigate to the matching MainWindow.xaml file in the *old* folder
* Agree for Visual Studio to replace the existing files. The matching .cs file should be automatically copied along with the .xaml file. 
