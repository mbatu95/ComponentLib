using System;
namespace CurveExtension
{
	public static class CurveTools
	{
		public static Func<float,float> funcFindCubic(float x0, float x1, float v0, float v1)
        {
			//Takes in the start and end values, and the gradients at those points, and finds a parametric cubic such that f([0,1]) maps to these values

			float a = v1 - 2*x1 + 2*x0 + v0;
			float b = -v1 -2*v0 +3*x1 - 3*x0;
			float c = v0;
			float d = x0;
			Func<float, float> f = (t => a * MathF.Pow(t, 3) + b * MathF.Pow(t, 2) + c * t + d);
			return f;

        }
		
	}
}