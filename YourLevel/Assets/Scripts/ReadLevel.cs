using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class ReadLevel : MonoBehaviour {
    
    public List<GameObject> props;
    public int iCount = 0, iInit = 0, iIndex = 0;
    GameObject activeProp;

    public bool start = false;
    public bool createObject = false;

    public int iParam = 0;

    List<string> properties;
    List<string> lines;

    public bool levelStart = false;

    private bool elementsAdded = false;

	// Use this for initialization
    void Start () {
    	Physics2D.IgnoreLayerCollision(10, 10, true);
    	properties = new List<string>();
    	lines = new List<string>();
        LoadFileLv("nivel.txt");
        foreach (string line in lines) {
        	processLine(line);
        }
        elementsAdded = true;
        activeProp = null;
    }

    void updateProps() {
    	if (elementsAdded == false) {
    		return;
    	}
    	if (iIndex < props.Count) {
	        if (activeProp != null) {
	        	if (activeProp.layer == 8 || activeProp.layer == 11) {
		            int fingerCount = 0;
		            Vector2 vectorPos = new Vector2(0,0);
		            foreach (Touch touch in Input.touches) {
		                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
		                    fingerCount = 1;
		                    vectorPos = touch.position;
		                }
		            }
		            if (Input.GetMouseButtonDown(0)) {
		                fingerCount = 1;
		                vectorPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		            }
		            if (fingerCount > 0) {
		                activeProp.GetComponent<SpriteRenderer>().sprite = activeProp.GetComponent<PropHandler>().oldSprite;
		                Vector3 vecPos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(vectorPos.x, vectorPos.y, GameObject.Find("Main Camera").GetComponent<Camera>().nearClipPlane));
		                activeProp.transform.position = vecPos;
		                if (activeProp.layer == 11) {
		                	vecPos.z = 10;
		                }
		                iIndex = iIndex + 1;
		                activeProp = null;
		            }
		        } else {
		        	int fingerCount = 0;
		            Vector2 vectorPos = new Vector2(0,0);
		            foreach (Touch touch in Input.touches) {
		                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
		                    fingerCount = 1;
		                    vectorPos = touch.position;
		                }
		            }
		            if (Input.GetMouseButtonDown(0)) {
		                fingerCount = 1;
		                vectorPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		            }
		            if (fingerCount > 0) {
		            	activeProp.GetComponent<Animator>().runtimeAnimatorController = activeProp.GetComponent<ObHandler>().cont;
		                activeProp.GetComponent<SpriteRenderer>().sprite = activeProp.GetComponent<ObHandler>().oldSprite;
		                Vector3 vecPos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(vectorPos.x, vectorPos.y, GameObject.Find("Main Camera").GetComponent<Camera>().nearClipPlane));
		                if (activeProp.GetComponent<ObHandler>().type == "enemy3") {
		                	vecPos.z = 5;
		                }
		                activeProp.transform.position = vecPos;
		                iIndex = iIndex + 1;
		                activeProp = null;
		            }
		        }
	        } else {
	            activeProp = props[iIndex];
	            if (activeProp.layer == 8 || activeProp.layer == 11) {
	            	GameObject.Find("NextImage").GetComponent<Image>().sprite = activeProp.GetComponent<PropHandler>().oldSprite;
	        	} else {
	        		GameObject.Find("NextImage").GetComponent<Image>().sprite = activeProp.GetComponent<ObHandler>().oldSprite;
	        	}
	        }
    	} else {
    		GetComponent<AudioSource>().Play();
    		levelStart = true;
    		Destroy(GameObject.Find("NextImage"));
    	}
    }
    
    // Update is called once per frame
    void Update () {
        updateProps();
    }

    private void createOb(List<string> properties) {
    	GameObject obj = GameObject.Find("Goomba");;
    	string ef = properties[0];
    	bool inv = (properties[1] == "true" ? true : false);
    	int lf = int.Parse(properties[2]);
    	int mv = 1;
    	switch (properties[3]) {
    		case "play":
    			mv = 0; break;
    		case "left":
    			mv = 1; break;
    		case "right":
    			mv = 2; break;
    		case "leftright":
    			mv = 3; break;
    		case "updown":
    			mv = 4; break;
    	}
    	string nm = properties[4];
    	int sp = int.Parse(properties[5]);
    	string tp = properties[6];
    	switch (tp) {
    		case "player":
    			obj = GameObject.Find("Player"); break;
    		case "enemy1":
    			obj = GameObject.Find("Goomba"); break;
    		case "enemy2":
    			obj = GameObject.Find("Koopa"); break;
    		case "enemy3":
    			obj = GameObject.Find("Piranha"); break;
    		case "coin":
    			obj = GameObject.Find("Coin"); break;
    		case "star":
    			obj = GameObject.Find("Star"); break;
    	}
    	GameObject ob = Instantiate (obj, new Vector3(0,0,obj.transform.position.z), new Quaternion(0,0,0,0)) as GameObject;
    	ob.GetComponent<ObHandler>().oldSprite = obj.GetComponent<ObHandler>().oldSprite;
    	ob.GetComponent<ObHandler>().cont = obj.GetComponent<ObHandler>().cont;
    	ob.GetComponent<ObHandler>().Initialize(nm, tp, mv, sp, ef, lf, inv);
    	GameObject.Find("GameMaster").GetComponent<ReadLevel>().props.Add(ob);
		GameObject.Find("GameMaster").GetComponent<ReadLevel>().iCount += 1;
		properties.Clear();
    }

    void processLine(string line) {
    	if (createObject) {
    		if (line.IndexOf('}') > -1) {
    			createOb(properties);
    			createObject = false;
    		} else {
	            int start = line.IndexOf('"');
	            string ln = line.Substring(start+1, line.Length-(start+1));
				int end = ln.IndexOf('"') + start;
				//string result = line.Substring(start+1, (end+1)-(start+1));
				ln = line.Substring((end+1)+1, line.Length-(end+2));
				int start2 = ln.IndexOf('"');
	            string ln2 = ln.Substring(start2+1, ln.Length-(start2+1));
				int end2 = ln2.IndexOf('"') + start2;
				string result2 = ln.Substring(start2+1, (end2+1)-(start2+1));
				properties.Add(result2);
			}
		}

		if (line.Length == 1 && line == "{") {
			start = true;
		} else {
			if (line.IndexOf("{") > -1) {
				createObject = true;
			}
		}
    }

    private bool LoadFileLv(string fileName) {
     // Handle any problems that might arise when reading the text
     try
     {
         string line;
         // Create a new StreamReader, tell it which file to read and what encoding the file
         // was saved as
         StreamReader theReader = new StreamReader(fileName, Encoding.Default);
         // Immediately clean up the reader after this block of code is done.
         // You generally use the "using" statement for potentially memory-intensive objects
         // instead of relying on garbage collection.
         // (Do not confuse this with the using directive for namespace at the 
         // beginning of a class!)
         using (theReader)
         {
             // While there's lines left in the text file, do this:
             do
             {
                 line = theReader.ReadLine();
                     
                 if (line != null)
                 {
                     // Do whatever you need to do with the text line, it's a string now
                     // In this example, I split it into arguments based on comma
                     // deliniators, then send that array to DoStuff()
                     //string[] entries = line.Split(',');
                     //if (entries.Length > 0)
                     //    DoStuff(entries);
                    //Debug.Log(line);
                    lines.Add(line);
                 }
             }
             while (line != null);
             //Debug.Log("Closing");
             // Done reading, close the reader and return true to broadcast success    
             theReader.Close();
             return true;
             }
         }
         // If anything broke in the try block, we throw an exception with information
         // on what didn't work
         catch (Exception e)
         {
             Debug.Log(e.Message);
             return false;
         }
     }
}
