![image](http://www.ark-energy.eu/wp-content/uploads/ark-dark.png)
# Artesian.SDK

This Library provides read access to the Artesian API

## Getting Started
### Installation
This library is provided in NuGet.

Support for .NET Framework 4.6.1, .NET Standard 2.0.

In the Package Manager Console -
```
Install-Package Artesian.SDK
```

or download directly from NuGet.
* [NuGet](https://www.nuget.org/packages/Artesian.SDK/)

## How to use
The Artesian.SDK instance can be configured using either Client credentials or API-Key authentication
```csharp
//API-Key
 ArtesianServiceConfig _cfg = new ArtesianServiceConfig(
		new Uri("https://fake-artesian-env/"),
		"5418B0DB - 7AB9 - 4875 - 81BA - 6EE609E073B6"
		);

//Client credentials
 ArtesianServiceConfig _cfg = new ArtesianServiceConfig(
		new Uri("https://fake-artesian-env/"),
		"audience",
		"domain",
		"client_id",
		"client_secret"
		);
```

## QueryService
Using the ArtesianServiceConfig we create an instance of the QueryService which is used to create Actual, Versioned and Market Assessment time series queries

### Policy configuration
Optionally a custom policy can be introduced to configure policy constraints within the QueryService otherwise default policy
is implemented
```csharp
ArtesianPolicyConfig _policy = new ArtesianPolicyConfig();
	_policy
	    .RetryPolicyConfig(retryCount: 3, retryWaitTime: 200)
	    .CircuitBreakerPolicyConfig(maxExceptions: 2, durationOfBreak: 3)
	    .BulkheadPolicyConfig(maxParallelism: 10, maxQueuingActions: 15);

var qs = new QueryService(_cfg,_policy);
```

<table>
  <tr><th>Policies</th><th>Description</th></tr>
  <tr><td>Wait & Retry Policy</td><td>Retry, waiting a specified duration between each retry</td></tr>
  <tr><td>Circuit Breaker Policy</td><td>The CircuitBreakerPolicy instance maintains internal state across calls to track failures</td></tr>
  <tr><td>Bulkhead Policy</td><td>The bulkhead isolation policy assigns operations to constrained resource pools, such that one faulting channel of actions cannot swamp all resource</td></tr>
</table>

### Partition Strategy
Requests are partitioned by an IPartitionStrategy, optionally an IPartitionStrategy can be passed to use a certain partition  
strategy. A partition strategy by ID is implemented by default 
```csharp
PartitionByIDStrategy idStrategy = new PartitionByIDStrategy();
var act = qs.CreateActual(idStrategy)
       .ForMarketData(new int[] { 100000001 })
       .InGranularity(Granularity.Day)
       .InRelativeInterval(RelativeInterval.RollingMonth)
       .ExecuteAsync().Result;
```
<table>
  <tr><th>Partition Strategies</th><th>Description</th></tr>
  <tr><td>Partition by ID</td><td>Requests are partitioned into groups of ID's by a defined partition size </td></tr>
</table>

### Actual Time Series
```csharp
var queryservice = new QueryService(_cfg);
var actualTimeSeries = await qs.CreateActual()
                .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                .InGranularity(Granularity.Day)
                .InAbsoluteDateRange(new LocalDate(2018,08,01),new LocalDate(2018,08,10))
                .ExecuteAsync();
```
To construct an Actual Time Series the following must be provided.
<table>
  <tr><th>Actual Query</th><th>Description</th></tr>
  <tr><td>Market Data ID</td><td>Provide a market data id or set of market data id's to query</td></tr>
  <tr><td>Time Granularity</td><td>Specify the granularity type</td></tr>
  <tr><td>Time Extraction Window</td><td>An extraction time window for data to be queried</td></tr>
</table>

[Go to Time Extraction window section](#artesian-sdk-extraction-windows)

### Market Assessment Time Series
```csharp
var queryservice = new QueryService(_cfg);
var marketAssesmentSeries = await qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync();
```
To construct a Market Assessment Time Series the following must be provided.
<table>
  <tr><th>Actual Query</th><th>Description</th></tr>
  <tr><td>Market Data ID</td><td>Provide a market data id or set of market data id's to query</td></tr>
  <tr><td>Product</td><td>Provide a product or set of products</td></tr>
  <tr><td>Time Extraction Window</td><td>An extraction time window for data to be queried </td></tr>
</table>

[Go to Time Extraction window section](#artesian-sdk-extraction-windows)

### Versioned Time Series
```csharp
 var versionedSeries = await qs.CreateVersioned()
		.ForMarketData(new int[] { 100000001 })
		.InGranularity(Granularity.Day)
		.ForLastOfMonths(Period.FromMonths(-4))
		.InRelativeInterval(RelativeInterval.RollingMonth)
		.ExecuteAsync();
```
To construct a Versioned Time Series the following must be provided.
<table>
  <tr><th>Versioned Query</th><th>Description</th></tr>
  <tr><td>Market Data ID</td><td>Provide a market data id or set of market data id's to query</td></tr>
  <tr><td>Time Granularity</td><td>Specify the granularity type</td></tr>
  <tr><td>Versioned Time Extraction Window</td><td>Versioned extraction time window</td></tr>
  <tr><td>Time Extraction Window</td><td>An extraction time window for data to be queried</td></tr>
</table>

[Go to Versioned Time Extraction window section](#versioned-time-extraction-windows)  
[Go to Time Extraction window section](#artesian-sdk-extraction-windows)

### Versioned Time Extraction Windows
<table>
  <tr><th>Versioned Time Extraction Windows</th><th>Description</th></tr>
  <tr><td>Version</td><td>Gets the specified version of a versioned timeseries</td></tr>
  <tr><td>Last Days</td><td>Gets the latest version of a versioned timeseries of each day in a time window</td></tr>
  <tr><td>Last N</td><td>Gets the latest N timeseries versions that have at least a not-null value</td></tr>
  <tr><td>Most Recent</td><td>Gets the most recent version of a versioned timeseries in a time window</td></tr>
  <tr><td>Most Updated Version</td><td>Gets the timeseries of the most updated version of each timepoint of a versioned timeseries</td></tr>
</table>

Versioned Time Extraction window types  for queries.

Version
```csharp
 .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0, 0, 123))
```
Last Days
```csharp
 .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
```
Last N
```csharp
 .ForLastNVersions(3)
```
Most Recent
```csharp
 .ForMostRecent(Period.FromMonths(-1), Period.FromDays(20))
```
Most Updated Version
```csharp
 .ForMUV()
 /// optional paramater to limit version
 .ForMUV(new LocalDateTime(2019, 05, 01, 2, 0, 0))
```

### Artesian SDK Extraction Windows
Extraction window types  for queries.

Date Range
```csharp
 .InAbsoluteDateRange(new LocalDate(2018,08,01),new LocalDate(2018,08,10)
```
Relative Interval
```csharp
 .InRelativeInterval(RelativeInterval.RollingMonth)
```
Period
```csharp
 .InRelativePeriod(Period.FromDays(5))
```
Period Range
```csharp
 .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
```
### Filler Strategy
All extraction types (Actual,Versioned and Market Assessment) have an optional filler strategy.  

```csharp
var versionedSeries = await qs.CreateVersioned()
    .ForMarketData(new int[] { 100000001 })
	.InGranularity(Granularity.Day)
	.ForMostRecent()
	.InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
	.WithFillLatestValue(Period.FromDays(7))
```	
				   
Null
```csharp
 .WithFillNull()
```
None
```csharp
 .WithFillNone()
```
Custom Value
```csharp
 //Timeseries
 .WithFillCustomValue(123)
 // Market Assessment
 ..WithFillCustomValue(new MarketAssessmentValue { Settlement = 123, Open = 456, Close = 789, High = 321, Low = 654, VolumePaid = 987, VolumeGiven = 213, Volume = 435 })
```
Latest Value
```csharp
 .WithLFillLatestValue(Period.FromDays(7))
```

## MarketData Service

Using the ArtesianServiceConfig `_cfg` we create an instance of the MarketDataService which is used to retrieve and edit
MarketData refrences. `GetMarketReference` will read the marketdata entity by MarketDataIdentifier and returns an istance of IMarketData if it exists.
```csharp
//reference market data entity
var marketDataEntity = new MarketDataEntity.Input(){
    ProviderName = "TestProviderName",
    MarketDataName = "TestMarketDataName",
    OriginalGranularity = Granularity.Day,
    OriginalTimezone = "CET",
    AggregationRule = AggregationRule.Undefined,
    Type = MarketDataType.VersionedTimeSerie,
    MarketDataId = 1
}

var marketDataService = new MarketDataService(_cfg);

var marketData = await marketDataQueryService.GetMarketDataReference(new MarketDataIdentifier(
        marketDataEntity.ProviderName,
        marketDataEntity.MarketDataName)
    );
```

To Check MarketData for `IsRegistered` status, returns true if present or false if not found.
```csharp
var isRegistered = await marketData.IsRegistered();
```

To `Register` MarketData , it will first verify that it has not all ready been registered then proceed to register the given MarketData entity.
```csharp
await marketData.Register(marketDataEntity);
```

Calling `Update` will update the current MarketData metadata with changed values. Calling `Load`, retrieves the current metadata of a MarketData.
```csharp
marketData.Metadata.AggregationRule = AggregationRule.SumAndDivide;
marketData.Metadata.Transform = SystemTimeTransforms.GASDAY66;

await marketData.Update();

await marketData.Load();
```

Using `Write mode` to edit MarketData and `save` to save the data of the current MarketData providing an instant.
### Actual Time Series
`EditActual` starts the write mode for an Actual Time serie. Checks are done to verify registration and MarketDataType to verify it is an Actual Time Serie.
Using `AddData` to be written.
```csharp
var writeMarketData = marketdata.EditActual();

writeMarketData.AddData(new LocalDate(2018, 10, 03), 10);
writeMarketData.AddData(new LocalDate(2018, 10, 04), 15);

await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));
```

### Versioned Time Series
`EditVersioned` starts the write mode for a Versioned Time serie. Checks are done to verify registration and MarketDataType to verify it is a Versioned Time Serie.
Using `AddData` to be written.
 ```csharp
var writeMarketData = marketData.EditVersioned(new LocalDateTime(2018, 10, 18, 00, 00));

writeMarketData.AddData(new LocalDate(2018, 10, 03), 10);
writeMarketData.AddData(new LocalDate(2018, 10, 04), 15);

await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));
```

### Market Assessment Time Series
`EditMarketAssessment` starts the write mode for  a Market Assessment. Checks are done to verify registration and MarketDataType to verify it is a Market Assessment.
Using `AddData` to provide a local date time and a MarketAssessmentValue to be written.
```csharp
var writeMarketData = marketData.EditMarketAssessment();

var marketAssessmentValue = new MarketAssessmentValue()
{
    High = 47,
    Close = 20,
    Low = 18,
    Open = 33,
    Settlement = 22
};

writeMarketData.AddData(new LocalDate(2018, 11, 28), "Dec-18", marketAssessmentValue);

await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));
```

## Links
* [Nuget](https://www.nuget.org/packages/Artesian.SDK/)
* [Github](https://github.com/ARKlab/Artesian.SDK)
* [Ark Energy](http://www.ark-energy.eu/)

## Acknowledgments
* [Flurl](https://flurl.io/docs/fluent-url/)
