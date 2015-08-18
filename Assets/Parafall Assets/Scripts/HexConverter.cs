using UnityEngine;
using System.Collections;

public class HexConverter{

	public string getHex(float iDecimal){
		return iDecimal.ToString("X");
	}

	public int hexToInt(char hexchar){
		string hex = "";
		hex += hexchar;
		return int.Parse (hex, System.Globalization.NumberStyles.HexNumber);
	}

	public string rGBtoHex(Color color){
		float red = color.r;
		float green = color.g;
		float blue = color.b;

		string hexA = getHex (Mathf.Floor (red / 16));
		string hexB = getHex (Mathf.Floor (red % 16));

		string hexC = getHex (Mathf.Floor (green / 16));
		string hexD = getHex (Mathf.Floor (green % 16));

		string hexE = getHex (Mathf.Floor (blue / 16));
		string hexF = getHex (Mathf.Floor (blue % 16));

		return hexA + hexB + hexC + hexD + hexE + hexF;
	}

	public Color hexToRGB(char[] color){
		float red = (hexToInt (color[1]) + hexToInt (color[0])) * 16.0f / 255;
		float green = (hexToInt (color[3]) + hexToInt (color[2])) * 16.0f / 255;
		float blue = (hexToInt (color[5]) + hexToInt (color[4])) * 16.0f / 255;

		Color rgb = new Color();
		rgb.r = red;
		rgb.g = green;
		rgb.b = blue;
		rgb.a = 1;

		return rgb;
	}
}
