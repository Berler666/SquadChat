using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public Animator newUser;

	public void CloseNewUserWindow()
	{
		newUser.enabled = true;
		newUser.SetBool("isHidden", true);
	}

}
