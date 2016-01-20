using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface IEventsInterface : IEventSystemHandler {

	// Use this for initialization
	void OnCreateNextSection ();
	void OnCollisionWithObstacle();
	void OnBlockPass();
	void PlayJumpSfx();
	void PlayCrouchSfx();
	void PlayRunSfx();
	void PlayDieSfx();
}
