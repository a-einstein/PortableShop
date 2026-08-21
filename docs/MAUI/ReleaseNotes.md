## PortableShop - MAUI
### ReleaseNotes 2.2.0

#### Summary
Working MAUI application. Still limited to Android release.

#### What's new
* Add theming.
* Remove reference to WCF-service.
* Target .Net 10.0.
* Target Windows 10.0.26100.0
* Target Android 16/SDK 36.

#### Fixed
* App-icon now working on Android.
* Text filter entry now readable with both themes on Android.
* Unclear order of services.
* Backlog of external package-versions.

#### Known issues
* Note how to solve **building-problems** as described in RCS.PortableShop.csproj.
* Exception exclusively for WCF on Android: "Error in deserializing". This will probably be solved by [xamarin-android issue 7785](https://github.com/xamarin/xamarin-android/pull/7785)
* Not yet released for Windows.
* No splash screen on Windows.
* Display shopping cart incorrect on Windows, until resizing.
* Validation of text filter not properly working.
* Refresh button invisible on Windows.
* Various icons and titles not working, depending on platform and theme setting.
* Restored filter values not visible on Windows.
