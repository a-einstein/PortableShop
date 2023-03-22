## PortableShop - Xamarin Forms: ReleaseNotes 1.x

#### Summary
Enable global update of shopping projects by CoreWcf.

#### What's new
* Added optional client of the CoreWcf service.

#### Fixed
* Faulty filter initialisation.

#### Known issues
* Exception exclusively for CoreWcf on Android: "Error in deserializing". This is related to [WCF issue 2463](https://github.com/dotnet/wcf/issues/2463)
* Recovering from a service problem does not succeed.