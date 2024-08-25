using MediatR;
using System.Text.Json.Serialization;
using ThreadboxApi.Application.Csp.Models;

namespace ThreadboxApi.Application.Csp.Commands
{
    public class CreateCspReport : IRequestHandler<CreateCspReport.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            [JsonPropertyName("csp-report")]
            public CspReport CspReport { get; set; }
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}