## PortableShop - MAUI: ReleaseNotes 2.x.y

#### Summary
Working MAUI application. Still limited to Android release.

#### What's new
* Added optional client of the CoreWcf service.

#### Fixed
* Faulty filter initialisation.

#### Known issues
* Exception exclusively for WCF on Android: "Error in deserializing". This will probably be solved by [xamarin-android issue 7785](https://github.com/xamarin/xamarin-android/pull/7785)
* Not yet released for Windows.
* No splash screen on Windows.
* Display shopping cart incorrect on Windows, until resizing.
* Validation of text filter not properly working.
* Refresh button invisible on Windows.
* Various icons and titles not working, depending on platform and theme setting.
* Restored filter values not visible on Windows.