using budget_backend.data;
using budget_backend.data.dbo;
using MediatR;

namespace budget_backend.Domain.Commands;

public class CreateTemplateCommand : IRequest {
    public CreateTemplateCommand(Template template)
    {
        Template = template;
    }

    public Template Template { get; init; }
}

public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand>
{
    private readonly CashflowDataContext _cashflowDataContext;

    public CreateTemplateCommandHandler(CashflowDataContext cashflowDataContext)
    {
        _cashflowDataContext = cashflowDataContext;
    }

    public async Task<Unit> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        await _cashflowDataContext.AddTemplateAsync(request.Template);
        await _cashflowDataContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}