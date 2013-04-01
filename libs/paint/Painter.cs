using UnityEngine;
using System.Collections;
using System.IO;

public class Painter : MonoBehaviour {
	Texture2D texture;
	GameObject tshirt;
	// Use this for initialization
	Object[] mesh;
	int index = 0;
	int count=0;
	MeshFilter meshFilter;
	string filename = "/Users/yuta0103/Desktop/151.jpg";
	bool showGUI = true;
	string[] meshLabels;
	Vector3 defaultScale;
	void Start () {
		/*texture=guiTexture.texture as Texture2D;
		if (texture == null) {
			texture = new Texture2D(256,256);
			guiTexture.texture = texture;
		}*/
		tshirt = GameObject.Find("tshirt");
		defaultScale = tshirt.transform.localScale;
		meshFilter = tshirt.GetComponent(typeof(MeshFilter))as MeshFilter;
		mesh = Resources.LoadAll("",typeof(Mesh));
		meshLabels = new string[mesh.Length];
		for(int i = 0; i<mesh.Length; i++){
			meshLabels[i] = mesh[i].name;
			Debug.Log(meshLabels[i]);
		}
		Debug.Log(mesh.Length); 
	}
	int rotate = 0;
	void RotateTshirt(int r) {
		switch(r) {
		case 1:
			tshirt.transform.Rotate(0* Time.deltaTime,5, 0 * Time.deltaTime);
			break;
		case 2:
			tshirt.transform.Rotate(0* Time.deltaTime,-5, 0 * Time.deltaTime);
			break;
		case 3:
			tshirt.transform.Rotate(-5,0, 0 * Time.deltaTime);
			break;
		case 4:
			tshirt.transform.Rotate(5,0, 0 * Time.deltaTime);
			break;
		}
	}
	int selectedMeshIndex=0;
	Vector2 scrollPos;
	void OnGUI () {
		if (!showGUI) return;
	    if(GUI.RepeatButton(new Rect(0,0,20,20),"<")){
			rotate=1;
		} 
		else if(GUI.RepeatButton(new Rect(30,0,20,20),">")){
			rotate=2;
		} else if (GUI.RepeatButton(new Rect(60,0,20,20),"^")){
			rotate =3;
		} else if(GUI.RepeatButton(new Rect(90,0,20,20),"v")){
			rotate = 4;
		}else {
			rotate = 0;
		}
		if (GUI.Button(new Rect(120,0,60,20),"reset")) {
			tshirt.transform.eulerAngles= new Vector3(0,0,0);
		}
		if(GUI.Button(new Rect(30,30,50,20),"<< >>")){
			Vector3 scale=tshirt.transform.localScale;
			tshirt.transform.localScale=new Vector3(scale.x+1f,scale.y+1f,scale.z+1f);
		}
		if(GUI.Button(new Rect(90,30,50,20),">> <<")){
			Vector3 scale=tshirt.transform.localScale;
			tshirt.transform.localScale=new Vector3(scale.x-1f,scale.y-1f,scale.z-1f);
		}
		if (GUI.Button(new Rect(165,30,60,20),"reset")) {
			tshirt.transform.localScale = defaultScale;
		}
		if(GUI.Button(new Rect(220,30,40,20),"[o]")){
			IEnumerator enu = Dosnapshot();
			StartCoroutine(enu);
		}
		scrollPos = GUI.BeginScrollView(new Rect(0,100,140,240),  scrollPos,new Rect(0,0,100,meshLabels.Length*30),false,true);
		int tmpIndex = GUI.SelectionGrid(new Rect(0,0,100,meshLabels.Length * 30),selectedMeshIndex,meshLabels,1);
		if (selectedMeshIndex != tmpIndex) {
			selectedMeshIndex = tmpIndex;	
			Object o = mesh[selectedMeshIndex];
			meshFilter.mesh=o as Mesh;
		}
		GUI.EndScrollView();
		
		filename= GUI.TextField(new Rect(0,60,300,20),filename);
		if (count%100 == 0){
			IEnumerator en = LoadLocalTexture();
			StartCoroutine(en);
		}
		RotateTshirt(rotate);
	}
	WWW w;
	Texture2D texture2d;
	IEnumerator LoadLocalTexture(){
		w = new WWW ("file://"+filename);
		yield return w;
		Debug.Log("www loaded. URL is " + filename + " and error is " + w.error + " and isDone is " + w.isDone);
		if (w.error == null && w.texture != null) {
			texture2d = w.texture;
			Destroy(tshirt.renderer.material.mainTexture);
			tshirt.renderer.material.mainTexture = texture2d;
		}
		texture2d = null;
		w.Dispose();
		w=null;
	}
	public IEnumerator Dosnapshot()
    {
        showGUI = false; // disable main GUI

        string s_name = filename+"_thumbnail_" + System.DateTime.Now.ToString("yyyyMMddHHmmss")+".png";
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false );
  
        // Read screen contents into the texture
        yield return new WaitForEndOfFrame();
        tex.ReadPixels( new Rect(0, 0, Screen.width, Screen.height), 0, 0 );
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
		File.WriteAllBytes(s_name,bytes);
		showGUI=true;
    }
	
	bool write = false;
	// Update is called once per frame
	void Update () {
		/*foreach(Touch t in Input.touches) {
				
		}*/
		/*if (write) {
			Vector3 v=Input.mousePosition;
			Color[] colors = new Color[100];
			for(int i=0;i<100;i++) {
				colors[i]=Color.red;
			}
			texture.SetPixels((int)v.x,(int)v.y,10,10,colors);
			guiTexture.texture = texture;
			tshirt.gameObject.renderer.material.mainTexture=texture;
			texture.Apply();
		}*/
		if (count%50==0) {
			Object o = mesh[index];
			//meshFilter.mesh=o as Mesh;
			index++;
		}
		count++;
		if (index>=mesh.Length) {
			index=0;
			count=0;
		}
		//RotateTshirt(rotate);
		//tshirt.transform.Rotate(0* Time.deltaTime,10* Time.deltaTime, 0 * Time.deltaTime);
	}
	void OnMouseDown(){
		write=true;
	}
	void OnMouseUp(){
		write=false;
		rotate=0;
	}
}
