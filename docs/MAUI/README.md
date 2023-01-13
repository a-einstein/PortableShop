## PortableShop MAUI

#### Summary
Working portable application for test and demo purpose. It simulates limited shopping capabilities based on the Microsoft AdventureWorks database.

#### Purpose
* Explore various techniques based on C#, XAML and MAUI.
* Manage the code and releases by Git and GitHub.
* Explore continuous integration by using combinations of Git, GitHub, Azure DevOps.

#### Notes
* This is the MAUI-conversion of my **[PortableShop Xamarin Forms](../XamarinForms/README.md)**
* This is a near equivalent of my **[WpfShop](https://github.com/a-einstein/WpfShop)**.
* Currently the Android application is only tested on API 30.
* Currently the Windows application is only tested on Windows 10 version 22H2.
* Submitted to code analysis by [Better Code Hub](https://bettercodehub.com). Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/PortableShop)](https://bettercodehub.com)
* Connected to automated Azure Devops build and release pipelines. Current build status for the master branch: [![Build Status](https://dev.azure.com/RcsProjects/PortableShop/_apis/build/status/Build%20APK?branchName=master)](https://dev.azure.com/RcsProjects/PortableShop/_build/latest?definitionId=13&branchName=master)
* See **[Release Notes](ReleaseNotes.md)** for the latest developments.

#### Prerequisites
* The application must be configured for a running instance of my **[AdventureWorks services](https://github.com/a-einstein/AdventureWorks/blob/master/README.md)**.

#### Aspects
* MAUI + MAUI Community Toolkit.
* .Net 7 + .Net Standard 2.0.
* C# + XAML.
* MVVM.
* Dependency injection.
* Client-server + SSL.
* Asynchronicity.
* WCF or Web API.
* Has made use of an Azure service.
* Error handling.
* Globalized resources.
* Basic styling.
* Splash screen.
* Shell navigation including Fly out.
* Rotation.
* Custom controls.
* Extensions.
* Attached behaviours.
* Application icon (Android).
* MSIX installation.
* Azure DevOps self hosted build pipeline + release pipeline to GitHub.

#### Installation
* The application is plug & play, but use of the data service is on request. Contact the developer ahead. 
* Installables are to be obtained from Assets of the latest available release in **[Releases](https://github.com/a-einstein/PortableShop/releases)**.

##### Android
* Download the latest APK file to your Android phone. 
* Install the aplication from the notification or the download folder. You probably have to suppress some warnings.
* In some cases one needs to 'Uninstall for all users' and repeat the install.
* Start the application. Either it will start 'empty' displaying a warning, or if the data service is running, one can 'shop' the contents.
* Uninstallation can be done by the normal Android procedure.
* Updating is currently not supported, but can be done manually after uninstalling. One can use the option from the application's menu to get to the Release page.

##### Windows
* Download the latest ZIP file to your PC. 
* Extract if needed.
* If not already done, install the certificate by right clicking on the .cer file. Choose 'Local Machine' and select 'Trusted Root Certification Authorities' as store.
* Now install the application by double clicking the .msix or .msixbundle file. If selected, the application starts right away.
* Note that the application gets installed in the Windows 'Apps' menu, and will not show up in the 'Programs and Features' list.
* Both starting (left click) and uninstallation (right click) are from the 'Apps' menu.