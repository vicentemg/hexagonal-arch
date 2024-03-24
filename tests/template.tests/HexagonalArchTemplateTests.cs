using Microsoft.Extensions.Logging;
using Microsoft.TemplateEngine.Authoring.TemplateVerifier;

namespace template.tests;

public class HexagonalArchTemplateTests
{
    private readonly ILoggerFactory _loggerFactory;
    public HexagonalArchTemplateTests()
    {
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddSimpleConsole();
        });
    }

    [Fact]
    public async Task DefaultUsage()
    {
        var options = new TemplateVerifierOptions("hexagonal")
        {
            TemplateSpecificArgs = ["--name", "RewardEat"],
            SnapshotsDirectory = "../Snapshots/",
            DisableDiffTool = true,
            DoNotAppendTemplateArgsToScenarioName = true,

        };

        var engine = new VerificationEngine(_loggerFactory);
        await engine.Execute(options);
    }
}