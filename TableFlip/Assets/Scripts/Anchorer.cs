using UnityEngine;

public class Anchorer : MonoBehaviour { 

	[ContextMenu("SetAnchorAccordingToSize")]
	public void SetAnchorSameToSize(){
		RectTransform child = transform.GetComponent<RectTransform> ();
		RectTransform parent = transform.parent.GetComponent<RectTransform> ();

		Vector2 parentSize = new Vector2 (parent.rect.width, parent.rect.height);
		Vector2 posMin = new Vector2(parentSize.x * child.anchorMin.x, parentSize.y * child.anchorMin.y);
		Vector2 posMax = new Vector2(parentSize.x * child.anchorMax.x, parentSize.y * child.anchorMax.y);

		posMin = posMin + child.offsetMin;
		posMax = posMax + child.offsetMax;

		posMin = new Vector2(posMin.x / parent.rect.width, posMin.y / parent.rect.height);
		posMax = new Vector2(posMax.x / parent.rect.width, posMax.y / parent.rect.height);

		child.anchorMin = posMin;
		child.anchorMax = posMax;

		child.offsetMin = Vector2.zero;
		child.offsetMax = Vector2.zero;
	}
		
}
