# twingly-search-api-dotnet
.NET client for Twingly Search API (previously known as Twingly Analytics API). Twingly is a blog search service that provides [a searchable API](https://developer.twingly.com/resources/search/).

![Build status](https://ci.appveyor.com/api/projects/status/gljbvg2ds257o6jw?svg=true)

## Configuration

* [Required] Set API key in the appSettings section of your config file:

```.NET

  <appSettings>
    <add key="TWINGLY_API_KEY" value="YOUR_KEY_GOES_HERE"/>
  </appSettings>

```

* [Optional] Set request timeout. The default timeout value is 10 seconds.
```.NET

  <appSettings>
    <add key="TWINGLY_TIMEOUT_MS" value="REQUEST_TIMEOUT_IN_MILLISECONDS"/>
  </appSettings>

```

## Example usage
Fetch docs about "Slack" published since yesterday. Limit results to 10 posts.

```.NET

    Query theQuery = QueryBuilder.Create("Slack page-size:10")
                                        .StartTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                                        .Build();

    ITwinglySearchClient client = new TwinglySearchClient();
    QueryResult matchingDocs = client.Query(theQuery);
    foreach (var post in matchingDocs.Posts)
    {
        Console.WriteLine("Title: '{0}', Date: '{1}', Url: '{2}'",
        doc.Title, doc.Published, doc.Url);
    }

```
To learn more about the features of this client, check out the example code that can be found in [Twingly.Search.Samples](Twingly.Search.Samples).

### Exception handling

Client exceptions are organized into the following hierachy:
* TwinglyRequestException - base class for any Twingly-related exception
    * ApiKeyDoesNotExistException - thrown when no API key was found;
    * UnauthorizedApiKeyException - thrown when API key is not authorized for the action being performed;
    * TwinglyServiceUnavailableException - thrown when service is not available;
    * ApiKeyNotConfiguredException - thrown when an API key was not found in the config file.
        
## License

The MIT License (MIT)

Copyright (c) 2012-2016 Twingly AB

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
