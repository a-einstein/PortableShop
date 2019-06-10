## PortableShop

#### Description
Working portable (Android) application for test and demo purpose based on Xamarin Forms. It simulates limited shopping capabilities based on the AdventureWorks database.

#### News
* Added the new Flyout, part of Shell navigation in Xamarin Forms 4.0.
* The data service on Azure is no longer functional.

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

#### Aspects
* Xamarin Forms.
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
Currently one has to both compile this client as well as the data service, and create the database.

The application no longer is plug & play. Just for its own sake the application can still be directly installed. It is still configured to use my data service on Azure, so it will fail to work.
* Download the latest APK from **[releases](https://github.com/a-einstein/PortableShop/releases)** to your Android phone.
* When completed, choose install.

