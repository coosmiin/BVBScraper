namespace Investments.Logic.Calculus
{
	public static class MathHelper
	{
		public static bool IsApproxOne(this decimal value)
		{
			return value > 0.9m && value < 1.1m;
		}
	}
}