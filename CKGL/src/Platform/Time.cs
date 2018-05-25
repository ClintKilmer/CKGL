namespace CKGL
{
	public static class Time
	{
		private static double t = 0.0;
		public static float TotalSeconds { get { return (float)t; } }
		public static float TotalMilliseconds { get { return (float)(t * 1000.0); } }

		private static double updateHz = 60.0;
		private static double dt = 1.0 / updateHz;
		public static float DeltaTime { get { return (float)dt; } }

		public static bool UseMinimumUPS { get { return MinimumUPS > 0; } }
		public static int MinimumUPS { get; private set; } = 0;
		public static double MinimumSPU { get; private set; } = 0.0;

		public static bool UseMaximumFPS { get { return MaximumFPS > 0; } }
		public static int MaximumFPS { get; private set; } = 0;
		public static double MaximumSPF { get; private set; } = 0.0;

		public static float UPS { get; private set; } = (float)(1.0 / dt);
		public static float SPU { get; private set; } = (float)dt;

		public static float FPSRaw { get; private set; } = (float)(1.0 / dt);
		public static float SPFRaw { get; private set; } = (float)dt;
		public static float FPS { get; private set; } = (float)(1.0 / dt);
		public static float SPF { get; private set; } = (float)dt;
		public static float FPSSmoothed { get; private set; } = (float)(1.0 / dt);
		public static float SPFSmoothed { get; private set; } = (float)dt;

		private static double accumulator = 0.0;
		public static bool DoUpdate { get { return accumulator >= dt; } }
		public static bool DoDraw { get; private set; } = false;

		public static void SetUpdateHz(double updateHz)
		{
			if (Time.updateHz != updateHz)
			{
				if (updateHz <= 0f)
					throw new System.Exception("UpdateHz must be greater than 0.");
				Time.updateHz = updateHz;
				dt = 1.0 / Time.updateHz;

				SPU = (float)dt;
				UPS = (float)(1.0 / dt);
			}
		}

		public static void SetMinimumUPS(int minimumUPS)
		{
			if (MinimumUPS != minimumUPS)
			{
				if (minimumUPS <= 0)
				{
					MinimumUPS = 0;
					MinimumSPU = 0.0;
				}
				else
				{
					MinimumUPS = minimumUPS;
					MinimumSPU = 1.0 / MinimumUPS;
				}
			}
		}

		public static void SetMaximumFPS(int maximumFPS)
		{
			if (MaximumFPS != maximumFPS)
			{
				if (maximumFPS <= 0)
				{
					MaximumFPS = 0;
					MaximumSPF = 0.0;
				}
				else
				{
					MaximumFPS = maximumFPS;
					MaximumSPF = 1.0 / MaximumFPS;
				}
			}
		}

		private static ulong tickCurrentTime = Platform.PerformanceCounter;
		private static double maximumFPSAccumulator = 0.0;
		public static void Tick()
		{
			ulong newTime = Platform.PerformanceCounter;
			double tickDeltaTime = (newTime - tickCurrentTime) / (double)Platform.PerformanceFrequency;

			// Minimum UpdateHz, slow game down if slower
			if (UseMinimumUPS && tickDeltaTime > MinimumSPU)
				tickDeltaTime = MinimumSPU;

			tickCurrentTime = newTime;

			accumulator += tickDeltaTime;

			// MaximumFPS
			if (UseMaximumFPS)
			{
				maximumFPSAccumulator += tickDeltaTime;
				if (maximumFPSAccumulator >= MaximumSPF)
				{
					maximumFPSAccumulator %= MaximumSPF;
					DoDraw = true;
				}
			}
			else
			{
				DoDraw = true;
			}
		}

		public static void Update()
		{
			accumulator -= dt;
			t += dt;

			if (!UseMaximumFPS)
				DoDraw = true;

			SPFSmoothed = Math.Lerp(SPFSmoothed, SPF, SPU * 4f);
			FPSSmoothed = Math.Lerp(FPSSmoothed, FPS, SPU * 4f);
		}

		private static ulong drawCurrentTime;
		private static double averageTarget = 0.5;
		private static int drawAverageSamples = 0;
		private static double drawAverageAccumulator = 0.0;
		public static void Draw()
		{
			DoDraw = false;

			ulong newTime = Platform.PerformanceCounter;
			double drawDeltaTime = ((newTime - drawCurrentTime) / (double)Platform.PerformanceFrequency);
			drawCurrentTime = newTime;

			SPFRaw = (float)drawDeltaTime;
			FPSRaw = (float)(1.0 / drawDeltaTime);

			drawAverageSamples++;
			if ((drawAverageAccumulator += drawDeltaTime) >= averageTarget)
			{
				SPF = (float)drawAverageAccumulator / drawAverageSamples;
				FPS = (float)(1.0 / (drawAverageAccumulator / drawAverageSamples));
				drawAverageSamples = 0;
				drawAverageAccumulator = 0.0;
			}
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

			public static void StopWithOutput()
			{
				Output.WriteLine($"Stopwatch: {Stop() * 1000.0}");
			}
		}
	}
}