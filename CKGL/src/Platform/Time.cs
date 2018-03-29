namespace CKGL
{
	public static class Time
	{
		private static ulong previous;
		private static ulong current;

		public static ulong TotalMilliseconds
		{
			get { return Platform.TotalMilliseconds; }
		}

		public static float TotalSeconds
		{
			get { return Platform.TotalMilliseconds * 0.001f; }
		}

		public static float DeltaTime { get; private set; }
		//public static float DeltaTimeAverage { get; private set; }

		public static void Update()
		{
			previous = current;
			current = Platform.PerformanceCounter;

			DeltaTime = (float)(((current - previous) * 1000) / (double)Platform.PerformanceFrequency);
			//DeltaTimeAverage = DeltaTimeAverage.Lerp(DeltaTime, 0.1f).Min(500f);
		}

		public static class Stopwatch
		{
			private static ulong start;
			private static ulong end;

			public static void Start()
			{
				start = Platform.PerformanceCounter;
				end = 0;
			}

			public static double Stop()
			{
				end = Platform.PerformanceCounter;
				return ((end - start) / (double)Platform.PerformanceFrequency);
			}
		}
	}
}