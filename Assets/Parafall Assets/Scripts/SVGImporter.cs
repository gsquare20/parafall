using UnityEngine;
using System.Collections;

public class SVGImporter : MonoBehaviour {

	public string svgImageFileName;

	private HexConverter hexConverter;

	private HSBColor hsbcolor;

	private string svgImageAbsolutePath;

	private int curveSubdivisions = 4;

	// Use this for initialization
	void Start () {
		hexConverter = new HexConverter ();
		hsbcolor = new HSBColor ();
		if (Application.isEditor) {
			Debug.Log ("Application data path : " + Application.dataPath);
			svgImageAbsolutePath = "file://" + Application.dataPath.Split ("Assets"[0])[0] + svgImageFileName;
			Debug.Log ("Svg Image Absolute Path : " + svgImageAbsolutePath);
			StartCoroutine("importData", svgImageAbsolutePath);
			//importData(svgImageAbsolutePath);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator importData(string dataPath){
		WWW www = new WWW (dataPath);
		yield return www;

		string svgImageData = www.text;
		svgImageData = svgImageData.Replace (" \n", "");
		svgImageData = svgImageData.Replace ("\n", "");
		svgImageData = svgImageData.Replace ("\t", "");
		svgImageData = svgImageData.Replace ("  ", " ");

		Debug.Log ("file data : " + svgImageData);

		string[] lines = svgImageData.Split ("<"[0]);

		foreach (string line in lines) {
			Debug.Log ("line : " + line);
			string elementName = line.Substring(0, 4);

			Vector2[] points;

			switch(elementName){
				case "path":
					points = pathPoints (line, curveSubdivisions);
					break;
				case "rect":
					points = rectPoints (line);
					break;
				case "poly":
					points = polygonPoints (line);
					break;
				default :
					continue;
					break;
			}
		}

	}

	Vector2[] polygonPoints(string inputStr){
		return null;
	}

	Vector2[] rectPoints(string inputStr){
		return null;
	}

	Vector2[] pathPoints(string inputStr, int div){
		return null;
	}

	void cubicBezier (Vector2[] pointsArr, int sub){
	}
}
