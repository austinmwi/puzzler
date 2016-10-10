using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class gazeControl : MonoBehaviour {




		private Vector3 startingPosition;

		void Start() {
			startingPosition = transform.localPosition;
			SetGazedAt(false);
		}

		void LateUpdate() {
			GvrViewer.Instance.UpdateState();
			if (GvrViewer.Instance.BackButtonPressed) {
				Application.Quit();
			}
		}

		public void SetGazedAt(bool gazedAt) {
			//GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
		}

		public void Reset() {
			transform.localPosition = startingPosition;
		}

		public void ToggleVRMode() {
			GvrViewer.Instance.VRModeEnabled = !GvrViewer.Instance.VRModeEnabled;
		}

		public void ToggleDistortionCorrection() {
			switch(GvrViewer.Instance.DistortionCorrection) {
			case GvrViewer.DistortionCorrectionMethod.Unity:
				GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.Native;
				break;
			case GvrViewer.DistortionCorrectionMethod.Native:
				GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.None;
				break;
			case GvrViewer.DistortionCorrectionMethod.None:
			default:
				GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.Unity;
				break;
			}
		}

		public void ToggleDirectRender() {
			GvrViewer.Controller.directRender = !GvrViewer.Controller.directRender;
		}

		public void TeleportRandomly() {
			Vector3 direction = Random.onUnitSphere;
			direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
			float distance = 2 * Random.value + 1.5f;
			transform.localPosition = direction * distance;
		}

		#region IGvrGazeResponder implementation

		/// Called when the user is looking on a GameObject with this script,
		/// as long as it is set to an appropriate layer (see GvrGaze).
		public void OnGazeEnter() {
			SetGazedAt(true);
		}

		/// Called when the user stops looking on the GameObject, after OnGazeEnter
		/// was already called.
		public void OnGazeExit() {
			SetGazedAt(false);
		}

		/// Called when the viewer's trigger is used, between OnGazeEnter and OnGazeExit.
		public void OnGazeTrigger() {
			TeleportRandomly();
		}

		#endregion



}
