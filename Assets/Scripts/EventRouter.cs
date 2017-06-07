using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;


public class EventRouter : MonoBehaviour
{

	public static EventRouter Instance = null;

	/* used to tag buttons that should be ommitted in parsing and will be linked manually */
	const string _manuallyLinkedButtonName = "ManuallyConnectedButton";

	/* define listener method signature */
	delegate void ListenerMethodSignatureType (MonoBehaviour sender);

	/* stores listeners which have been subscibed on to receive events */
	static Dictionary <string, ListenerMethodSignatureType> _listeners;


	/* INIT */
	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}
		

	public void initialize ()
	{
		_listeners = new Dictionary <string, ListenerMethodSignatureType> ();

		/* connect buttons as event emitters */
		connectButtons ();
	}


	/* parse and connect all buttons to the router */
	void connectButtons ()
	{
		Button[] buttons = GetComponentsInChildren<Button> (); /* get all button elements */
		for (int i = 0; i < buttons.Count (); i++) {
			Button button = buttons [i];
			if (!button.tag.Equals (_manuallyLinkedButtonName)) /* filter buttons that use manual subscription */
				button.onClick.AddListener (() => invokeTargetMethodWithSender (button)); /* invokeTargetMethodWithSender will be invoced on button event */
		}
	}
		

	/* called on event from any connected button */
	void invokeTargetMethodWithSender (MonoBehaviour sender)
	{
		ListenerMethodSignatureType targetMethod = null;

		/* get pointer on target method from dictionary using sender namevas key */
		_listeners.TryGetValue (sender.name, out targetMethod);

		if (targetMethod != null) {
			targetMethod (sender); /* invoke target method */
		} else {
			Debug.Log ("Events Router ERROR: event for key [" + sender.name + "] not exists");
		}
	}


	/* called by listener */
	public  void subscribeToEvent (string eventName, Action <MonoBehaviour> listenerTargetMethod)
	{
		/* check if listenerTargetMethod is new and not created before */
		ListenerMethodSignatureType targetMethod = null;
		_listeners.TryGetValue (eventName, out targetMethod);
		if (targetMethod == null) { 

			/* add target method using event name as a key */
			targetMethod = new ListenerMethodSignatureType (listenerTargetMethod);
			_listeners.Add (eventName, targetMethod);

		}
	}


	public  void unsubscribeFromEvent (string eventName)
	{
		ListenerMethodSignatureType targetMethod = null;
		_listeners.TryGetValue (eventName, out targetMethod);

		if (targetMethod != null)
			_listeners.Remove (eventName);
		
	}


}
