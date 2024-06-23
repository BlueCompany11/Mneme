using DryIoc;

namespace Mneme.Core.Bootstrapper
{
	public class RegistrationTransferer
	{
		public void TransferRegistrations(IContainer sourceContainer, IContainer targetContainer)
		{
			foreach (var r in sourceContainer.GetServiceRegistrations())
			{
				targetContainer.Register(r.Factory, r.ServiceType, r.OptionalServiceKey, IfAlreadyRegistered.AppendNotKeyed, true);
			}
		}
	}
}
