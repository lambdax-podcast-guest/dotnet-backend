using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;
namespace Guests.Tests
{
    // -------------------------------------------------------------------------------------------------
    /// <summary>
    /// Base class that includes database fixture and xUnit outputter in the constructor
    /// Abstract class meant for inheritance only
    /// 
    /// All classes that inherit from this class will share the SAME database and server instance
    /// </summary>
    // -------------------------------------------------------------------------------------------------
    [Collection("DbCollection")]
    public abstract class TestBaseWithFixture
    {
        public DatabaseFixture fixture { get; private set; }
        public ITestOutputHelper outputter { get; private set; }

        public TestBaseWithFixture(DatabaseFixture fixture, ITestOutputHelper output)
        {
            this.fixture = fixture;
            // Use output.WriteLine to print to console
            // This ITestOutputHelper class only knows how to use the Visual Studio Output though, so to tell it to use the console here in VSCode, run the test command like this:
            // dotnet test -l "console;verbosity=detailed"
            outputter = output;
        }
    }
}
