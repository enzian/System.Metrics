# System.Metrics

System.Metrics is a standard interface for metrics in .net applications. It's designed to be modular in terms of how metrics are processes and what backends are supported. It provides abstractions for different types of `sinks` that can be implemented as desired.

## Where we come from:
There is little agreement in the dotnet world on how metrics should be gathered. There are many good libraries out there that do a great job at bringing metrics to a specific backend (StatsD, Heroku, etc.), but lack the possibility to be modularly included into an application.

**We believe that the desire to gather metrics should not force you as a developer to choose a technology.** That is why we provide this abstraction library.
We do not denie that the way we model metrics data is not very closely related to how StatsD metrics are usually structured but we believe, that the `StatsD`-protocol has become a solid standard in the world of metrics and a solid ecosystem of back- and frontends have emerged independently of the products that initially introduced it.

This library will format metrics for you as you hand them over for processing and submission to some kind of backend. What backends you use is entirely up to you. Whether that be a StatsD deamon, a local file or the command line is entirely up to you.

## How can I use it?

It's simple, just add the most recent `System.Metrics` NuGet package to your project and start gathering metrics:

```csharp
using System;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main()
        {
            var endpoint = new Metrics.StandardEndpoint();
            subject.Record<Counting>("startups.total", 1);
        }
    }
}

```
... now, that wont do much for you, for this package to make sense, it needs a backend. What backend you use, is up to you. Here are some:

* [System.Metrics.StatsD](https://github.com/enzian/System.Metrics.StatsD) - is a `StatsD` backend that sends metrics via UDP or TCP to a given host.
* Console endpoint (planned)
* rolling file appender (planned)
* ... more to come.

### What shoulder It stands on:

We could not do anything without some exceptional other open source projects - lets pay hommage to them:

* [xUnit](https://github.com/xunit/xunit) - these buys provide us with some exceptional unit testing magic!

* [FluentAssertions](https://github.com/dennisdoomen/FluentAssertions) - [@ddoomen](https://twitter.com/ddoomen) the all the other contributors to `FluentAssertions` do an exceptional job at making our live easyer and our test-failures easier to interprete!