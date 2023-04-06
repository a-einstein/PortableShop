TODO 

Keep having build Warning MSB3277 "Found conflicts between different versions of "System.Security.Principal.Windows" that could not be resolved."

Note there currently only seems to be 1 version at all in:
C:\Program Files (x86)\Microsoft SDKs\UWPNuGetPackages\microsoft.netcore.universalwindowsplatform\6.2.14\ref\uap10.0.15138

Applying bindingRedirect in App.config in either the portable or the platform project seems unable to work, probably ignored.

Also see the Android-project.