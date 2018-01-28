namespace SteveBagnall.CmaEs_IPOP
{
	public enum StopReason 
	{ 
		NotSet = 0, 
		Incomplete,
		GoodEnough, 
		MaxIterations, 
		MaxPopulation,
		MaxRestarts,
		Restart,
	}
}
