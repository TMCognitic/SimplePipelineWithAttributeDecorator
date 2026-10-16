using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimplePipelineWithAttributeDecorator;
using SimplePipelineWithAttributeDecorator.Abstractions;
using SimplePipelineWithAttributeDecorator.Decorators;
using SimplePipelineWithAttributeDecorator.Results;
using System.Text.Json;

IServiceCollection services = new ServiceCollection();
services.AddLogging(o => o.AddConsole());
services.AddHandlersFromAssembly(typeof(Program).Assembly);

IServiceProvider provider = services.BuildServiceProvider();

provider = provider.CreateScope().ServiceProvider;

ICommandHandler<SimpleCommand> handler = provider.GetRequiredService<ICommandHandler<SimpleCommand>>();
IPipeline<SimpleCommand>? pipeline = provider.GetService<IPipeline<SimpleCommand>>();

Result result = (pipeline is not null) ? pipeline.Execute(new SimpleCommand()) : handler.Handle(new SimpleCommand());





Console.WriteLine(JsonSerializer.Serialize(result));


class SimpleCommand : ICommandDefinition
{

}

[Log]
class SimpleCommandHandler : ICommandHandler<SimpleCommand>
{
    public Result Handle(SimpleCommand command)
    {
        return Result.Success();
    }
}