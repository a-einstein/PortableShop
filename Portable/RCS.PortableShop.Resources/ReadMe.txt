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
