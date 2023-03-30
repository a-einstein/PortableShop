TODO

Warning MSB3277 "Found conflicts between different versions of "System.Numerics.Vectors" that could not be resolved."

It's remarkable that only the directory for MonoAndroid\v1.0 actually holds (old) dlls, the other ones (up to 13.0, currently) don't.
C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ReferenceAssemblies\Microsoft\Framework\MonoAndroid\v1.0\

I would think the other versions of MonoAndroid need to be provided with newer version of the dlls.

Explicitly adding the newer Nuge package does not work as there are conflicting references within the same project. 
Though there is no direct reference from within my project to MonoAndroid or even XamarinForms.

Described at various places and as actual issues.
https://github.com/xamarin/xamarin-android/issues/6375
https://github.com/xamarin/Xamarin.Forms/issues/15046
https://github.com/xamarin/Xamarin.Forms/issues/15663