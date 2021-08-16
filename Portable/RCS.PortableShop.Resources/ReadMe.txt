Some notes, as this project has been creating confusement.

Structure (based on https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/localization/text)
- Have a default resource file (Labels.resx) without a language indication. 
The contents should correspond with the NeutralLanguage property in the main project file, no longer in an AssemblyInfo file 
(https://docs.microsoft.com/en-us/dotnet/core/tools/csproj#assemblyinfo-properties). 
This file is the only one that has a designer file (Labels.Designer.c)s. 
And these are the only ones explictly listed in the project file.
- Additionally explicit language files can be added, indicated by their names (Labels.nl-NL.resx). 
These don't have designer files.
They are not listed in the project file, just found implicitly by their names.
- Beware that because of the implicit nature of the new project file structure, extra files in a directory are noticed and can disturb. 
They may get a remove listing in the project file!

Current status (restart application after language switch)
- Android (OK)
	- German -> English.
	- Dutch -> Dutch.
	- American -> English.
	- English -> English.
- UWP
	- German -> Dutch. (TODO)
	- American ->  Dutch. (TODO)
	- Dutch -> Dutch. 
	- English -> Dutch. (TODO)

I keep getting warning:
	GENERATEPROJECTPRIFILE : warning : PRI257: 0xdef00522 - Resources found for language(s) 'en-gb,en-us,nl-nl' but no resources found for default language(s): 'en-us'. 
	Change the default language or qualify resources with the default language. http://go.microsoft.com/fwlink/?LinkId=231899
This does not make sense, after setting both NeutralLanguage and DefaultLanguage to en-GB on all projects, where possible. Also checking manifest files.
Also tried making an explicit en-GB resource besides a dummy one. 
All to no use.
Ignore it for the time being.
