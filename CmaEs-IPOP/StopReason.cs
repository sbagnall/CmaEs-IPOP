using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
