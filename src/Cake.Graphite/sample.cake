#addin nuget:?package=ahd.Graphite&version=2.0.0
#r "Cake.Graphite"
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

using Cake.Graphite;

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
   Information("Hello Cake!");
   data.Graphite.Send("hest", 3);
});

RunTarget(target);