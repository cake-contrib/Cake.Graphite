# Cake.Graphite

Cake.Graphite is a set of aliases for Cake to help with sending metrics to Graphite as part of a build.

[![License](http://img.shields.io/:license-mit-blue.svg)](http://cake-contrib.mit-license.org)

## Information

| | Stable | Pre-release |
|---|---|---|
|GitHub Release|-|[![GitHub release](https://img.shields.io/github/release/cake-contrib/Cake.Graphite.svg)](https://github.com/cake-contrib/Cake.Graphite/releases/latest)|
|NuGet|[![NuGet](https://img.shields.io/nuget/v/Cake.Graphite.svg)](https://www.nuget.org/packages/Cake.Graphite)|[![NuGet](https://img.shields.io/nuget/vpre/Cake.Graphite.svg)](https://www.nuget.org/packages/Cake.Graphite)|

## Build Status

|Develop|Master|
|:--:|:--:|
|[![Build status](https://ci.appveyor.com/api/projects/status/<appveyorlink>/branch/develop?svg=true)](https://ci.appveyor.com/project/cakecontrib/cake-graphite/branch/develop)|[![Build status](https://ci.appveyor.com/api/projects/status/<appveyorlink2>/branch/master?svg=true)](https://ci.appveyor.com/project/cakecontrib/cake-graphite/branch/master)|

## Code Coverage

[![Coverage Status](https://coveralls.io/repos/github/cake-contrib/Cake.Graphite/badge.svg?branch=develop)](https://coveralls.io/github/cake-contrib/Cake.Graphite?branch=develop)

## Chat Room

Come join in the conversation about Cake.Graphite in our Gitter Chat Room

[![Join the chat at https://gitter.im/cake-contrib/Lobby](https://badges.gitter.im/cake-contrib/Lobby.svg)](https://gitter.im/cake-contrib/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Build

To build this package we are using Cake.

On Windows PowerShell run:

```powershell
./build
```

## Example
```csharp
#addin nuget:?package=Cake.Graphite&loaddependencies=true
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

public class BuildData
{
    public Graphite Graphite { get; }

    public BuildData(Graphite graphite)
    {
       Graphite = graphite;
    }
}

Setup(setupContext  =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");

   var graphiteClient = Graphite(new GraphiteSettings{
      Host = "localhost"
   });

   return new BuildData(graphiteClient);
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
.Does<BuildData>(data => {
   data.Graphite.Send("example", 3);
});

RunTarget(target);
```
