using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {


	void Start () {
		EventRouter.Instance.initialize ();
		EventRouter.Instance.subscribeToEvent ("ButtonA", onButtonAEvent);
		EventRouter.Instance.subscribeToEvent ("ButtonB", onButtonBEvent);
		EventRouter.Instance.subscribeToEvent ("UnsubscribeButton", onUnsubscribeButtonEvent);
	}


	void onButtonAEvent(MonoBehaviour sender){
		Debug.Log ("::: onButtonAEvent, sender -> " + sender.name);
	}

	void onButtonBEvent(MonoBehaviour sender){
		Debug.Log ("::: onButtonBEvent, sender -> " + sender.name);
	}


	void onUnsubscribeButtonEvent(MonoBehaviour sender){
		Debug.Log ("::: unsubscribed ");
		unsubscribe ();
	}

	void unsubscribe(){
		EventRouter.Instance.unsubscribeFromEvent ("ButtonA");
		EventRouter.Instance.unsubscribeFromEvent ("ButtonB");
		EventRouter.Instance.unsubscribeFromEvent ("UnsubscribeButton");
	}	

}
