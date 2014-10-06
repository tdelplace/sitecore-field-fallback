Sitecore Field Fallback Module
=======================

[![Build status](https://ci.appveyor.com/api/projects/status/orgxpb7u78vqckl7)](https://ci.appveyor.com/project/SeanKearney/sitecore-field-fallback)

Field Fallback is the ability for a field's value to come from somewhere other than the field itself, a clones source, or its standard values. Various fallback scenarios have been provided in this solution with the ability to customize it as needed. The supplied scenarios are: 

* Ancestor Fallback - A field falls back to the value of its nearest set ancestor
* Lateral Fallback - A field falls back to the value of another field, or chain of fields
* Default Fallback - A field falls back to a text value or a token that is transformed at render time (not item creation time!)
* Language fallback - This is based on [Alex Shyba's Language Fallback module](OriginalModule)

The concept for this module came about when Alex Shyba released his Language Fallback module. While the initial prototype, written by [Elena Zlateva](Elena-Zlateva), was heavily based on Alex' work there isn't too much remaining in the FieldFallback.Kernel project. However, to support language fallback while using the Field Fallback module we have provided Partial Language Fallback in the Processors.Globalization project. This code is still very much original to Alex. [Charles-Turano](Charlie) also provided some much appreciated help around performance tuning and getting this to be as fast as possible.

[Sean Kearney](Sean-Kearney) ([Twitter](Sean-Kearney-Twitter))  

Installation
------------
The best way to use this Sitecore module is to [download](BuildArtifacts) and install the latest build. 

Developing and Contributing
---------------------------

We'd love to get contributions from you! 

### Codebase
In order to contribute to Sitecore Field Fallback you'll need to have a github account. Once you have your account, fork the [HedgehogDevelopment/sitecore-field-fallback repo](CodeRepo), and clone it onto your local machine. The [github docs have a good explanation](HowToFork) of how to do all of this. 

### Dependencies

Make sure that you have access to a NuGet server with Sitecore assemblies. Please read [Alen Pelin's article on how to set up a Sitecore NuGet feed](SitecoreNuget).

[Team Development for Sitecore](TDS) is not required to make code changes, but any changes that are made to a Sitecore item must be made to the `.item` file in the corresponding TDS project. Additionally, there is some customization to the TDS projects that will require the creation of a NuGet package that you can host locally. (**instructions coming**)

Support
-------
Please log issues with [GitHub Issue Tracker](support). This module is NOT supported by the Hedgehog Development product team. This is a community effort of the development team. If you have any questions or suggestions please contact us through the comments section or contact us via [Twitter](Hedgehog-Twitter).  

<!-- References -->

[BuildArtifacts]: https://ci.appveyor.com/project/SeanKearney/sitecore-field-fallback/build/artifacts
[SitecoreNuGet]: http://www.alen.me.uk/2014/10/internal-sitecore-nuget-server.html
[CodeRepo]: https://github.com/HedgehogDevelopment/sitecore-field-fallback
[HowToFork]: https://help.github.com/articles/fork-a-repo
[TDS]: http://TeamDevelopmentForSitecore.com
[Support]: https://github.com/HedgehogDevelopment/sitecore-field-fallback/issues
[OriginalModule]: http://trac.sitecore.net/LanguageFallback

<!-- Developers -->
[Sean-Kearney]: http://seankearney.com
[Sean-Kearney-Twitter]: https://twitter.com/seankearney
[Elena-Zlateva]: http://twitter.com/ezlateva
[Charles-Turano]: http://sdn.sitecore.net/MVP/MVPs/Charles%20Turano.aspx
[Hedgehog-Twitter]: https://twitter.com/hhogdev