using System.IO;
using CommandLineParser.Arguments;
using CommandLineParser.Exceptions;
using ParserTest;
using Xunit;

namespace Tests
{
    public partial class Tests
    {
#pragma warning disable CS0649
        class DeclarativeArgumentsParsingTarget
        {
            [SwitchArgument(ShortName = 's', LongName = "show", DefaultValue = true, Description = "Set whether show or not")]
            public bool show;

            [SwitchArgument(ShortName = 'h', LongName = "hide", DefaultValue = false, Description = "Set whether hide or not")]
            public bool Hide { get; set; }

            [ValueArgument(typeof(decimal), ShortName = 'v', LongName = "version", Description = "Set desired version", Aliases = new [] { "w", "ver"})]
            public decimal version;

            [ValueArgument(typeof(string), ShortName = 'l', LongName = "level", Description = "Set the level")]
            public string level;

            [ValueArgument(typeof(Point), ShortName = 'p', LongName = "point", Description = "specify the point")]
            public Point point;

            [BoundedValueArgument(typeof(int), ShortName = 'o', LongName = "optimization", MinValue = 0, MaxValue = 3, Description = "Level of optimization")]
            public int optimization;

            [EnumeratedValueArgument(typeof(string), ShortName = 'c', LongName = "color", AllowedValues = "red;green;blue")]
            public string color;

            [FileArgument(ShortName = 'i', LongName = "input", Description = "Input file", FileMustExist = false)]
            public FileInfo inputFile;

            [FileArgument(ShortName = 'x', LongName = "output", Description = "Output file", FileMustExist = false)]
            public FileInfo outputFile;

            [DirectoryArgument(ShortName = 'd', LongName = "directory", Description = "Input directory", DirectoryMustExist = false)]
            public DirectoryInfo InputDirectory;
        }
#pragma warning restore CS0649

        private CommandLineParser.CommandLineParser InitDeclarativeArguments()
        {
            var commandLineParser = new CommandLineParser.CommandLineParser();
            DeclarativeArgumentsParsingTarget p = new DeclarativeArgumentsParsingTarget();

            // read the argument attributes 
            commandLineParser.ExtractArgumentAttributes(p);

            return commandLineParser;
        }

        [Fact]
        public void DeclarativeArguments_1()
        {
            var commandLineParser = InitDeclarativeArguments();
            string[] args = new[] { "--version", "1.3" };
            commandLineParser.ParseCommandLine(args);
        }

        [Fact]
        public void DeclarativeArguments_2()
        {
            var commandLineParser = InitDeclarativeArguments();
            string[] args = new[] { "--color", "red", "--version", "1.2" };
            commandLineParser.ParseCommandLine(args);
        }

        [Fact]
        public void DeclarativeArguments_3()
        {
            var commandLineParser = InitDeclarativeArguments();
            string[] args = new[] { "--point", "[1;3]", "-o", "2" };
            commandLineParser.ParseCommandLine(args);
        }

        [Fact]
        public void DeclarativeArguments_4()
        {
            var commandLineParser = InitDeclarativeArguments();
            string[] args = (new[] { "-d", "C:\\Input", "-i", "in.txt", "-x", "out.txt" });
            commandLineParser.ParseCommandLine(args);
        }

        [Fact]
        public void DeclarativeArguments_5()
        {
            var commandLineParser = InitDeclarativeArguments();
            string[] args = new[] { "--show", "--hide" };
            commandLineParser.ParseCommandLine(args);
        }

        [Fact]
        public void DeclarativeArguments_Ex6()
        {
            var commandLineParser = InitDeclarativeArguments();
            string[] args = new[] { "-d", "C:\\Input" };
            commandLineParser.ParseCommandLine(args);
        }

        [Fact]
        public void DeclarativeArguments_CommandLineArgumentException()
        {
            string[] args = new[] { "-d" };
            var commandLineParser = InitDeclarativeArguments();

            var ex = Assert.Throws<CommandLineArgumentException>(() => commandLineParser.ParseCommandLine(args));
            Assert.Contains("must be followed by a value", ex.Message);
        }

        [Fact]
        public void DeclarativeArguments_shouldRecognizeShortAlias()
        {
            string[] args = new[] { "-w", "3" };

            var commandLineParser = InitImperativeArguments();

            commandLineParser.ParseCommandLine(args);

            Assert.Equal(3M, ((IValueArgument)commandLineParser.LookupArgument("v")).Value);
        }

        [Fact]
        public void DeclarativeArguments_shouldRecognizeLongAlias()
        {
            string[] args = new[] { "--ver", "3" };

            var commandLineParser = InitImperativeArguments();

            commandLineParser.ParseCommandLine(args);

            Assert.Equal(3M, ((IValueArgument)commandLineParser.LookupArgument("v")).Value);
        }
    }
}