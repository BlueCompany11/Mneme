using DryIoc;

namespace Mneme.Core.Bootstrapper
{
	public static class BootstrapperHelper
	{
		public static IContainer CreateChild(this IContainer container,
		IfAlreadyRegistered? ifAlreadyRegistered = null, Rules newRules = null, bool withDisposables = false)
		{
			var rules = newRules != null && newRules != container.Rules ? newRules : container.Rules;
			return container.With(
				container.Parent,
				ifAlreadyRegistered == null ? rules : rules.WithDefaultIfAlreadyRegistered(ifAlreadyRegistered.Value),
				container.ScopeContext,
				RegistrySharing.CloneAndDropCache,
				container.SingletonScope.Clone(withDisposables),
				container.CurrentScope?.Clone(withDisposables));
		}
	}
}
