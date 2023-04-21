namespace Smart.Kh2Ems.Infrastructure.Shared.Injectables
{
    public interface IInjectableService { }
    public interface ITransientService : IInjectableService { }
    public interface IScopedService : IInjectableService { }
    public interface ISingletonService : IInjectableService { }
}
