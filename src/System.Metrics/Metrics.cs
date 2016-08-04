namespace System.Metrics
{
    public interface IAllowsSampleRate { }
    public interface IAllowsDelta { }

    public interface IAllowsDouble { }
    public interface IAllowsInteger { }
    public interface IAllowsString { }

    public class Counting : IAllowsSampleRate, IAllowsInteger { }
    public class Timing : IAllowsSampleRate, IAllowsInteger { }
    public class Gauge : IAllowsDouble, IAllowsInteger, IAllowsDelta { }
    public class Histogram : IAllowsInteger { }
    public class Meter : IAllowsInteger { }
    public class Set : IAllowsString { }
}