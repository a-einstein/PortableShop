## PortableShop

#### Description
Working portable (Android) application for test and demo purpose based on Xamarin Forms. It simulates limited shopping capabilities based on the AdventureWorks database.

#### News
* Release is now to GitHub with an automatic change summary.
* Applied dependency injection.
* Improved use of HttpClient.
* Improved Flyout Menu.
* Added storage and retrieval of the overview filter between sessions.
* Added ability to switch between the two service types and store this in settings.
* Added usage of Web API service as alternative to WCF.
* Makes use of own certified domain for webservice.
* Migrated to .Net Standard.

#### Known issues
* The application may crash at startup if the data service is not available, particularly in the error details message.
* Restoring the text filter does work, but does not visibly show in the entry field.
* The application icon is not correctly displayed on screen.

#### Purpose
* Explore various techniques based on C#, XAML and Xamarin Forms.
* Manage the code and releases by Git and GitHub.
* Explore continuous integration by using combinations of Git, GitHub, Azure DevOps and TeamCity.
* Explore Scrum process management by integration with Jira and Azure DevOps.

#### Prerequisites
* The application must be configured for a running instance of my **[AdventureWorks services](https://github.com/a-einstein/AdventureWorks/blob/master/README.md)**.

#### Notes
* This is a near equivalent of my **[WpfShop](https://github.com/a-einstein/WpfShop)**.
* Currently this is mainly tested on Android API 29.
* Submitted to code analysis by [Better Code Hub](https://bettercodehub.com). Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/PortableShop)](https://bettercodehub.com)
* Connected to automated Azure Devops build and release pipelines. Current build status for the master branch: [![Build Status](https://dev.azure.com/RcsProjects/PortableShop/_apis/build/status/Build%20APK?branchName=master)](https://dev.azure.com/RcsProjects/PortableShop/_build/latest?definitionId=13&branchName=master)

#### Aspects
* Xamarin Forms including Xamarin.Essentials.
* .Net Standard.
* C# + XAML.
* MVVM.
* Dependency injection.
* Client-server + SSL.
* Asynchronisity.
* WCF.
* Web API.
* Has made use of an Azure service.
* Error handling.
* Globalized resources.
* Basic styling.
* Splash screen (Android).
* Shell navigation including Fly out.
* Rotation.
* Custom controls.
* Extensions.
* Attached behaviours.
* Application icon (Android).

#### Installation
The application is plug & play, but use of the data service is on request. Contact the developer ahead. 
* Download the APK from Assets of the latest available release from **[Releases](https://github.com/a-einstein/PortableShop/releases)** to your Android phone. 
* Install the aplication from the notification or the download folder. You probably have to suppress some warnings.
* In some cases one needs to 'Uninstall for all users' and repeat the install.
* Start the application. Either it will start 'empty' displaying a warning, or if the data service is running, one can 'shop' the contents.
* Uninstallation can be done by the normal Android procedure.
* Updating is currently not supported, but can be done manually after uninstalling. One can also use the option from the application's menu to get to the Release page.

