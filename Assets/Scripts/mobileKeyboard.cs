using UnityEngine;
using System.Collections;

public class mobileKeyboard : MonoBehaviour {

	public void OpenKeyboard()
	{
		TouchScreenKeyboard.Open("", TouchScreenKeyboardType.ASCIICapable);
	}
}
