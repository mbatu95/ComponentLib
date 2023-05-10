using System;
namespace IntegralToolsExtension
{
	public static class IntegralTools
	{

		//Evaluates an integral running from fStart to fEnd
		public static float fCalculateIntegral(Func<float, float> funcToIntegrate, float fStart, float fEnd, int iSamples)
        {

			//Uses rectangles to estimate integral - could use trapeziums but not too worried about accuracy for now
			float fWidth = fEnd - fStart;
			float fRectangleWidth = fWidth / (float)iSamples;
			float fOutput = 0;
			for (int i = 0; i < iSamples - 1; i++)
            {
				//Finds point in centre of rectangle 
				float fPoint = fStart + ((i + 0.5f) * fRectangleWidth);
				float fHeight = funcToIntegrate(fPoint);
				fOutput += fHeight * fRectangleWidth;
            }
			return fOutput;
        }

		//Evaluates an integral running from fStart to x, where x is chosen
		public static Func<float,float> funcIntegrateFunction(Func<float, float> funcToIntegrate, float fStart, int iSamples)
        {
			Func<float, float> funcResult = (x => fCalculateIntegral(funcToIntegrate, fStart, x, iSamples));
			return funcResult;
        }
	}
}