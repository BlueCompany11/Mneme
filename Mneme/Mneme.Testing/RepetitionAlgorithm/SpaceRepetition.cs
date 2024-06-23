using Mneme.Model.TestCreation;

namespace Mneme.Testing.RepetitionAlgorithm
{
	public class SpaceRepetition
	{
		private readonly Fibo fibo;

		public SpaceRepetition(Fibo fibo)
		{
			this.fibo = fibo;
		}
		public bool ShouldBeTested(IUserTest userTest)
		{
			return (DateTime.Now - userTest.TestInfo.Updated).Days + 1 >= fibo.FiboValues[userTest.TestInfo.Occurrence];
		}
	}
}
