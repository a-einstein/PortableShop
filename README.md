## PortableShop

Submitted to code analysis by **[Better Code Hub](https://bettercodehub.com)**.  
Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/PortableShop)](https://bettercodehub.com)

#### Purpose
* Creating a working portable application for test and demo purpose based on Xamarin Forms.

#### Notes
* This is a near equivalent of my [WpfShop](https://github.com/a-einstein/WpfShop).
* Currently this is only tested on Android API 21, and no longer suited for all platforms anyway.
* For the time being, the needed data service is hosted on Azure and configured to in this application. That means that this code can be build and run right away, if it can make use of an internet connection.
* Intention is to make this application avaliable as an apk file on GitHub too. So it would be directly installable as a binary.

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
* Orientation.
* Custom controls.
* Extension.
* Behaviour.
* Application icon (Android).

#### Prerequisites
* The application assumes the presence of my [AdventureWorks services](https://github.com/a-einstein/AdventureWorks/blob/master/README.md), to which a service connection should be configured.
