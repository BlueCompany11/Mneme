namespace Mneme.Model.TestCreation
{
	public interface IUserTest
	{
		TestInfo TestInfo { get; }
		public void Tested(bool successfully)
		{
			if (successfully)
			{
				TestInfo.Occurrence++;
				TestInfo.Updated = DateTime.Now;
			}
			else
			{
				TestInfo.Occurrence -= 3;
				if (TestInfo.Occurrence < 0)
				{
					TestInfo.Occurrence = 0;
				}
			}
		}
	}
}
