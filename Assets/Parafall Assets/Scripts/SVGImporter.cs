using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Collections.Generic;

public class SVGImporter : MonoBehaviour {

	public string svgImageFileName;

	private HexConverter hexConverter;

	private HSBColor hsbcolor;

	private string svgImageAbsolutePath;

	private int curveSubdivisions = 20;

	public float pScale = 0.01f;

	public Vector3 offsetVector;

	public Material mat;

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

	IEnumerator importData1(string dataPath){
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

			if(line.Length < 4)
				continue;

			string elementName = line.Substring(0, 4);

			Debug.Log ("element name : " + elementName);

			Vector2[] points;

			switch(elementName){
				case "path":
					//points = pathPoints (line, curveSubdivisions);
					break;
				case "rect":
					//points = rectPoints (line);
					break;
				case "poly":
					//points = polygonPoints (line);
					break;
				default :
					continue;
					break;
			}
		}

	}

	IEnumerator importData(string dataPath){
		XPathDocument xPathDoc = new XPathDocument (dataPath);
		Debug.Log ("XPAth Doc : " + xPathDoc);
		yield return xPathDoc;
		XPathNavigator nav = xPathDoc.CreateNavigator ();
		Debug.Log ("XPAth nav : " + nav);

		XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager (nav.NameTable);
		xmlNamespaceManager.AddNamespace ("svg", "http://www.w3.org/2000/svg");

		XPathExpression expr = nav.Compile ("//svg:path");
		XPathNodeIterator iterator = nav.Select ("//svg:path", xmlNamespaceManager);
		//Debug.Log ("Iterator count : " + iterator.Count);

		while (iterator.MoveNext()) {
			XPathNavigator svgPathNavigator = iterator.Current;
			Vector2[] pointsArr = pathPoints (svgPathNavigator, curveSubdivisions);
			foreach(Vector2 point in pointsArr){
				Debug.Log (point);
			}
			if(null == pointsArr || pointsArr.Length == 0)
				continue;

			GameObject pMesh = new GameObject();
//			pMesh.tag = "svg";

			string pathId = svgPathNavigator.GetAttribute("id", "");
			if(null == pathId || pathId.Length == 0)
				pMesh.name = "noname";
			else
				pMesh.name = pathId;

			string pathFill = svgPathNavigator.GetAttribute("fill", "");
			if(null == pathFill || pathFill.Length == 0)
				pathFill = "#000000";
			Color pathColor = hexConverter.hexToRGB(pathFill.Substring(1).ToCharArray());

			string pathOpacity = svgPathNavigator.GetAttribute("opacity", "");

			Traingulator1 traingulator = new Traingulator1(pointsArr);

			int[] indices = traingulator.Triangulate("reverse");

			Debug.Log ("No of traingles : " + indices.Length);

			Vector3[] vertices = new Vector3[pointsArr.Length];

			Debug.Log ("No of vertices : " + vertices.Length);

			for(int i=0; i<pointsArr.Length; i++){
				vertices[i] = new Vector3(pScale*pointsArr[i].x, -pScale*pointsArr[i].y, 0);
				//Debug.Log (vertices[i]);
			}

			Mesh mesh = new Mesh();
			mesh.vertices = vertices;

			mesh.triangles = indices;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			mesh.uv = pointsArr;

			pMesh.transform.position += transform.position;
			pMesh.transform.position += offsetVector;
			pMesh.transform.parent = transform;
			pMesh.AddComponent<MeshFilter>();
			pMesh.AddComponent<MeshRenderer>();
			pMesh.GetComponent<MeshFilter>().mesh = mesh;
		
			Material pathMat = (Material) Instantiate(mat);
			pathMat.SetColor ("_Color", pathColor);
			HSBColor pathHSBcolor = HSBColor.FromColor(pathColor);
			pathHSBcolor.b *= 0.36f;

			pathColor = HSBColor.ToColor(pathHSBcolor);
			pathMat.SetColor ("_Emission", pathColor);
			pMesh.renderer.material = pathMat;

			break;
		}
	}

	Vector2[] polygonPoints(string inputStr){
		return null;
	}

	Vector2[] rectPoints(string inputStr){
		return null;
	}

	Vector2[] pathPoints(XPathNavigator pathNav, int div){

		float x = 0f;
		float y = 0f;
		float x1 = 0f;
		float y1 = 0f;

		List<Vector2> returnArr = new List<Vector2>();

		Vector2 cp0;
		Vector2 cp1;
		Vector2 cp2;
		Vector2 cp3;

		List<Vector2> controlPoints = new List<Vector2>();

		Vector2[] divPointsReturnArr;

		int returnArrCount = 0;

		string pathData = pathNav.GetAttribute ("d", "");
		//Debug.Log (pathData);

		List<string> instructionList = new List<string> ();

		char[] instructions = {'M', 'm', 'Z', 'z', 'L', 'l', 'H', 'h', 'V', 'v', 'C', 'c', 'S', 's', 'Q', 'q', 'T', 't', 'A', 'a'};

		bool instructionFound = false;

		string instructionValue = "";

		for (int i=0; i<pathData.Length; i++) {
			//Debug.Log (pathData[i]);
			foreach(char instruction in instructions){
				if(pathData[i] == instruction){
					instructionFound = true;
					break;
				}
			}
			if(instructionFound){
				if(instructionValue.Length != 0)
					instructionList.Add (instructionValue);
				instructionList.Add(pathData[i].ToString());
				instructionValue = "";
			}
			else{
				instructionValue += pathData[i].ToString();
			}
			
			if(instructionFound)
				instructionFound = false;
		}

		for (int i=0; i<instructionList.Count; i+=2) {
			string instruction = instructionList[i];
			Debug.Log ("Instruction : " + instruction);

			string instructionVal = "";
			if((i+1)<=(instructionList.Count-1)){
				instructionVal = instructionList[i+1];
				Debug.Log ("Instruction value : " + instructionVal);
			}

			instructionVal = instructionVal.Trim();

			string[] instructionArr = {};

			switch(instruction){
				case "m" :
					instructionArr = instructionVal.Split(","[0]);
					x = float.Parse(instructionArr[0]);
					y = float.Parse(instructionArr[1]);
					//Debug.Log ("x : " + x);
					x1 = x;
					y1 = y;
					returnArr.Add (new Vector2(x, y));
					break;

				case "M" :
					instructionArr = instructionVal.Split(","[0]);
					x = float.Parse(instructionArr[0]);
					y = float.Parse(instructionArr[1]);
					x1 = x;
					y1 = y;
					returnArr.Add (new Vector2(x, y));
					break;

				case "c" :
					
					Vector2 localcp3 = new Vector2();

					instructionArr = instructionVal.Split(" "[0]);
					cp0 = new Vector2(x1, y1);
					controlPoints.Add (cp0);
					int tokenCount = 0;
					foreach(string instructionToken in instructionArr){
						string[] instructionSubArr = instructionToken.Split (","[0]);
						Debug.Log ("Instruction Values : " + instructionSubArr[0] + " : " + instructionSubArr[1]);
						if(tokenCount == 0){
							cp1 = new Vector2(x1+float.Parse (instructionSubArr[0]), y1+float.Parse (instructionSubArr[1]));
							controlPoints.Add (cp1);
						}		
						if(tokenCount == 1){
							cp2 = new Vector2(x1+float.Parse (instructionSubArr[0]), y1+float.Parse (instructionSubArr[1]));
							controlPoints.Add (cp2);
						}
						if(tokenCount == 2){
							cp3 = new Vector2(x1+float.Parse (instructionSubArr[0]), y1+float.Parse (instructionSubArr[1]));
							x1 = cp3.x;
							y1 = cp3.y;
							controlPoints.Add (cp3);
							localcp3 = cp3;
						}
						tokenCount++;
					}

//					foreach(Vector2 controlPoint in controlPoints){
//						Debug.Log("Control Point : " + controlPoint);	
//					}
					
					divPointsReturnArr =  cubicBezier(controlPoints.ToArray(), div);
					foreach(Vector2 divPoint in divPointsReturnArr){
						returnArr.Add (divPoint);
					}

					returnArr.Add (localcp3);
					
					break;

				case "z" :
					break;

				case "Z" :
					break;

				case "l" :
					instructionArr = instructionVal.Split(","[0]);
					for(int j=0; j<instructionArr.Length; j += 2){
						x = x1 + float.Parse(instructionArr[j]);
						y = y1 + float.Parse(instructionArr[j+1]);
						returnArr.Add (new Vector2(x, y));
					}
					x1 = x;
					y1 = y;
					break;

				case "L" :
					break;

				case "h" :
					break;

				case "H" :
					break;

				case "v" :
					break;

				case "V" :
					break;



				case "C" :
					break;

				case "s" :
					break;

				case "S" :
					break;

				case "q" :
					break;

				case "Q" :
					break;

				case "T" :
					break;

				case "t" :
					break;

				case "a" :
					break;

				case "A" :
					break;
			}
		}

		List<Vector2> returnArr2 = new List<Vector2> ();
		returnArr2.Add (returnArr [0]);

		for (int i=1; i<returnArr.Count; i++) {
			if(Vector2.Distance (returnArr[i], returnArr[(i+1)%returnArr.Count]) > 0.01){
				returnArr2.Add (returnArr[i]);
			}
		}

		return returnArr2.ToArray ();
	}

	Vector2[] cubicBezier (Vector2[] pointsArr, int sub){
		float t1;
		Vector2 p01;
		Vector2 p12;
		Vector2 p23;
		Vector2 p0112;
		Vector2 p1223;
		Vector2 divPoint;

		List<Vector2> divPoints = new List<Vector2> ();

		for (int i=1; i< (sub+1); i++) {
			t1 = i*1f/(sub+1f);
			//Debug.Log ("Parameter Value : " + t1);
			p01 = pointsArr[0]*(1-t1) + pointsArr[1]*t1;
			//Debug.Log ("Point on P01 : " + p01);
			p12 = pointsArr[1]*(1-t1) + pointsArr[2]*t1;
			//Debug.Log ("Point on P12 : " + p12);
			p23 = pointsArr[2]*(1-t1) + pointsArr[3]*t1;
			//Debug.Log ("Point on P23 : " + p23);

			p0112 = p01*(1-t1) + p12*t1;
			//Debug.Log ("Point on P0112 : " + p0112);
			p1223 = p12*(1-t1) + p23*t1;
			//Debug.Log ("Point on P1223 : " + p1223);

			divPoint = p0112*(1-t1) + p1223*t1;
			//Debug.Log ("DivPoint : " + divPoint);
			divPoints.Add (divPoint);
		}
		return divPoints.ToArray();
	}
}
