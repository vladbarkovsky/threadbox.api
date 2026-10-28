using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using ThreadboxApi.Application.Csp.Models;

namespace ThreadboxApi.Application.Csp.Commands
{
    public class CreateCspReport : IRequestHandler<CreateCspReport.Command, NoContentResult>
    {
        public class Command : IRequest<NoContentResult>
        {
            [JsonPropertyName("csp-report")]
            public CspReport CspReport { get; init; }
        }

        public async Task<NoContentResult> Handle(Command request, CancellationToken cancellationToken)
        {
            // TODO: Do something with CSP reports
            return new NoContentResult();
        }
    }
}
