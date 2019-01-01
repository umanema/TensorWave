using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AzureSky;

public class AzureUIController : MonoBehaviour
{
	public AzureSkyController skyController;
	public Slider slider;
	public Image transitionBar;
	private Vector3 m_scale;

	// Update is called once per frame
	void Update ()
	{
		skyController.timeOfDay.hour = slider.value;
		if (transitionBar)
		{
			m_scale = new Vector3 (skyController.weatherTransitionTime, 1.0f, 1.0f);
			transitionBar.rectTransform.localScale = m_scale;
		}
	}
}