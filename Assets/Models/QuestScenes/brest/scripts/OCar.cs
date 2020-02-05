using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Vehicles.Car
{

public class OCar : MonoBehaviour {
	
	public bool Player;
	public GameObject PolicMan;
	public bool inCar;
	public GameObject CarCamera;
	public GameObject PlayerCamera;
	public GameObject Exit;
    public GameObject tube;
	
	void Start (){
		gameObject.GetComponent<CarController>().enabled = false;
	   gameObject.GetComponent<CarUserControl>().enabled = false;
	   gameObject.GetComponent<CarAudio>().enabled = false;
	   CarCamera.SetActive (false);
	   PlayerCamera.SetActive (true);
	   
   }
	

void OnTriggerEnter(Collider other){
	if(other.tag=="Player")
	{
		Player=true;
			
		}
}
void OnTriggerExit(Collider other){
	if(other.tag=="Player")
	{
		Player=false;
	}
}
void Update (){
	if (Player==true){
   if (Input.GetKeyDown(KeyCode.F)){
	   PolicMan.SetActive (false);
	   inCar=true;
	   gameObject.GetComponent<CarController>().enabled = true;
	   gameObject.GetComponent<CarUserControl>().enabled = true;
	   gameObject.GetComponent<CarAudio>().enabled = true;
	   CarCamera.SetActive (true);
	   PlayerCamera.SetActive (false);
	   
   }
	   
}
if (inCar==true){
	if (Input.GetKeyDown(KeyCode.E)){
		PolicMan.SetActive (true);
		PolicMan.transform.position=Exit.transform.position;
		inCar=false;
		gameObject.GetComponent<CarController>().enabled = false;
	   gameObject.GetComponent<CarUserControl>().enabled = false;
	   gameObject.GetComponent<CarAudio>().enabled = false;
	   CarCamera.SetActive (false);
       PlayerCamera.SetActive(true); 
	}
    if (Input.GetKeyDown(KeyCode.A))
    {
        tube.GetComponent<Animator>().SetTrigger("left");
    }
    else
    {
        tube.GetComponent<Animator>().SetTrigger("idle");
    }
    if (Input.GetKeyDown(KeyCode.D))
    {
        tube.GetComponent<Animator>().SetTrigger("right");
    }
    else
    {
        tube.GetComponent<Animator>().SetTrigger("idle");
    }
}
}
}

}
		

 
