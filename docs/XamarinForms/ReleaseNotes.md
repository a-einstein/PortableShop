## PortableShop - Xamarin Forms: ReleaseNotes 1.x

#### Summary
Enable global update of shopping projects by CoreWcf.

#### What's new
* Added optional client of the CoreWcf service.

#### Fixed
* Faulty filter initialisation.

#### Known issues
* Exception exclusively for WCF on Android: "Error in deserializing". This will probably be solved by [xamarin-android issue 7785](https://github.com/xamarin/xamarin-android/pull/7785)
* Recovering from a service problem does not succeed.