using static OpenAL.Bindings;

namespace CKGL
{
	public enum AudioDistanceModel : int
	{
		InverseDistance = alDistanceModelType.InverseDistance,
		InverseDistanceClamped = alDistanceModelType.InverseDistanceClamped,
		LinearDistance = alDistanceModelType.LinearDistance,
		LinearDistanceClamped = alDistanceModelType.LinearDistanceClamped,
		ExponentDistance = alDistanceModelType.ExponentDistance,
		ExponentDistanceClamped = alDistanceModelType.ExponentDistanceClamped,
		None = alDistanceModelType.None
	}
}