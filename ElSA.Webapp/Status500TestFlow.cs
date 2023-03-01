using Elsa.Activities.Http;
using Elsa.Activities.Temporal;
using Elsa.Activities.Console;
using Elsa.Builders;
using NodaTime;

namespace ElSA.Webapp
{
    public class Status500TestFlow : IWorkflow
    {
        // This might be needed to change depending on you system
        private const string HealthUrl = "https://localhost:7010/is-healthy";

        public void Build(IWorkflowBuilder builder)
        {
            builder
            .Timer(Duration.FromSeconds(10))
            .SendHttpRequest(activity => activity
                    .WithSupportedHttpCodes(() => new[] { 200, 500 })
                    .WithUrl(new Uri(HealthUrl)),
                activity =>
                {
                    activity
                        .When("200")
                        .WriteLine("Received a 200 status code.");

                    activity
                        .When("500")
                        .WriteLine("Server returned an error.");

                    activity
                        .When("Unsupported Status Code")
                        .WriteLine("Received an unexpected status code");
                })
            .WriteLine("And we're done. See you in 10 seconds.,");
        }
    }
}
