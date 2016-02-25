using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteveBagnall.CmaEs_IPOP
{
	public struct Solution
	{
		public StopReason StopReason;
		public double MinimizedFitness;
		public CMAState BestCMAState;
		public int BestNumEvals;
		public int BestNumRestarts;
		public double[] BestParameters;
		public CMAState LatestCMAState;
		public int LatestNumEvals;
		public int LatestNumRestarts;
		public double[] LatestParameters;
		public Queue<double> History;
	}
}
