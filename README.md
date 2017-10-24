# Twingly Search API .NET

[![Build status](https://ci.appveyor.com/api/projects/status/h3ga4nbgue02ufcm/branch/master?svg=true)](https://ci.appveyor.com/project/Twingly/twingly-search-api-dotnet/branch/master)
[![Build Status Travis](https://www.travis-ci.org/twingly/twingly-search-api-dotnet.svg?branch=master)](https://www.travis-ci.org/twingly/twingly-search-api-dotnet)

.NET client for Twingly Search API (previously known as Twingly Analytics API). Twingly is a blog search service that provides [a searchable API](https://developer.twingly.com/resources/search/).

## Installation

Install via [NuGet](https://www.nuget.org/packages/Twingly.Search.Client/)

    Install-Package Twingly.Search.Client

## Configuration

* [Required] Set API key in the appSettings section of your config file:

```xml
<appSettings>
  <add key="TWINGLY_SEARCH_KEY" value="YOUR_KEY_GOES_HERE"/>
</appSettings>
```

* [Optional] Set request timeout. The default timeout value is 10 seconds.

```xml
<appSettings>
  <add key="TWINGLY_TIMEOUT_MS" value="REQUEST_TIMEOUT_IN_MILLISECONDS"/>
</appSettings>
```

* Alternatively: Set these settings in the environment variables. The settings are first read from configuration, then from the environment variables.

## Example usage

Fetch docs about "Slack" published since yesterday. Limit results to 10 posts.

```cs
Query theQuery = QueryBuilder.Create("Slack page-size:10")
                                    .StartTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                                    .Build();

ITwinglySearchClient client = new TwinglySearchClient();
// identify your company by setting the user agent.
client.UserAgent = "Willy Wonka Chocolate Factory";

QueryResult matchingDocs = client.Query(theQuery);
foreach (var post in matchingDocs.Posts)
{
    Console.WriteLine("Title: '{0}', Date: '{1}', Url: '{2}'",
    post.Title, post.Published, post.Url);
}
```

To learn more about the features of this client, check out the example code in [Twingly.Search.Samples](Twingly.Search.Samples).

### Exception handling

Exceptions are organized into the following hierachy:
* `RequestException` - base class for any Twingly-related exception
  * `AuthException` - thrown when the provided key does not have access to the API
  * `QueryException` - thrown when there is something wrong with the parameters or the query sent to the API
  * `ServerException` - thrown when service is not available
* `ApiKeyNotConfiguredException` - thrown when an API key was not found in the config file

## Requirements

* API key, [sign up](https://www.twingly.com/try-for-free) via [twingly.com](https://www.twingly.com/) to get one
* .NET Framework v4.5.2 or Mono 5.0

## Development

### Code coverage

It is possible to check the code coverage by using the tool [OpenCover](https://github.com/OpenCover/opencover).

Prerequisites:

* Install the [OpenCover binary](https://github.com/opencover/opencover/releases) or get it via [NuGet](https://www.nuget.org/packages/opencover)
* Install the [Visual Studio extension](https://visualstudiogallery.msdn.microsoft.com/6950a046-8919-4935-8542-c6f37956f688)

To use the tool:

1. Open the window `OpenCover Test Explorer`
1. Group by `Project`
1. Right-click `Twingly,Search.Tests` and select `Cover with OpenCover`
1. Open the window `OpenCover Results` to inspect the outcome

Note: The first time you use the Visual Studio extension you will need to give it the path to the OpenCover binary which is typically installed at: `%localappdata%\Apps\OpenCover\OpenCover.Console.exe`

### Release

A NuGet package is automatically generated on each build, using handy [automation scripts by Daniel Schroeder](https://newnugetpackage.codeplex.com/wikipage?title=NuGet%20Package%20To%20Create%20A%20NuGet%20Package%20From%20Your%20Project%20After%20Every%20Build&referringTitle=Home).

1. Bump the version and update the release notes in `Twingly.Search.Client\_CreateNewNuGetPackage\Config.ps1`
1. Bump the `AssemblyVersion` and `AssemblyFileVersion` in `Twingly.Search.Client\Properties\AssemblyInfo.cs`
1. Commit the changes
1. Tag the current commit with the same version number and push it
1. Build the project `Twingly.Search.Client` in release mode
  * The `.nupkg` file is placed in the `Twingly.Search.Client\bin\Release` directory
1. Run `Twingly.Search.Client\_CreateNewNuGetPackage\RunMeToUploadNuGetPackage.cmd`
1. Point it to the `.nupkg` file
1. Enter your API key

## License

The MIT License (MIT)

Copyright (c) 2016 Andrey Mironoff, Twingly AB

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
