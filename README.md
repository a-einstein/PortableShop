## PortableShop

#### Description
Working portable (Android) application for test and demo purpose based on Xamarin Forms. It simulates limited shopping capabilities based on the AdventureWorks database.

#### News
* The APK installation file can be downloaded from Azure again, as described below.
* Migrated to .Net Standard.
* Integrated with Azure Devops build and release pipelines.
* Added the new Flyout, part of Shell navigation in Xamarin Forms 4.0.
* For full demonstration/test purposes, the data service is on request.

#### Purpose
* Explore various techniques based on C#, XAML and Xamarin Forms.
* Manage the code and releases by Git and GitHub.
* Explore continuous integration by using combinations of Git, GitHub, TeamCity and Azure DevOps.
* Explore Scrum process management by integration with Jira and Azure DevOps.

#### Prerequisites
* The application must be configured for a running instance of my [AdventureWorks services](https://github.com/a-einstein/AdventureWorks/blob/master/README.md).

#### Notes
* This is a near equivalent of my [WpfShop](https://github.com/a-einstein/WpfShop).
* Currently this is only tested on Android API 21, and no longer suited for all platforms anyway.
* Submitted to code analysis by **[Better Code Hub](https://bettercodehub.com)**. Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/PortableShop)](https://bettercodehub.com)
* Connected to automated Azure Devops build and release pipelines. Current build status for the master branch: [![Build Status](https://dev.azure.com/RcsProjects/PortableShop/_apis/build/status/Build%20APK?branchName=master)](https://dev.azure.com/RcsProjects/PortableShop/_build/latest?definitionId=13&branchName=master)

#### Aspects
* Xamarin Forms.
* .Net Standard.
* C# + XAML.
* MVVM.
* Make use of an Azure service.
* Client-server.
* WCF + SSL.
* Asynchronisity.
* Error handling.
* Globalized resources.
* Basic styling.
* Splash screen (Android).
* Shell navigation.
* Rotation.
* Custom controls.
* Extensions.
* Attached behaviours.
* Application icon (Android).

#### Installation
The application is plug & play, but use of the data service is on request. Contact the developer ahead. 
* Download and install the **[latest APK](https://rcsadventureworac85.blob.core.windows.net/portableshop-releases/latest/RCS.CyclOne.apk)** to your Android phone. You probably have to suppress some warnings.
* Start the application. Either it will start 'empty' displaying a warning, or if the data service is running one can 'shop' the contents.

