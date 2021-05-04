using UnityEditor;
using UnityEngine.UIElements;

public class UIElementsExampleWindow : EditorWindow
{
	[MenuItem("YRGO/Part 05/UIElements Example")]
	static void UIElementsExampleWindowMenuItem()
	{
		var window = EditorWindow.GetWindow<UIElementsExampleWindow>();
		window.minSize = new UnityEngine.Vector2(150, 100f);
		window.maxSize = new UnityEngine.Vector2(window.minSize.x + 200f, window.minSize.y);
		window.Show();
	}

	public void OnEnable()
	{
		var root = this.rootVisualElement;
		root.style.paddingTop = new StyleLength(10f);
		root.style.paddingBottom = new StyleLength(10f);
		root.style.paddingLeft = new StyleLength(10f);
		root.style.paddingRight = new StyleLength(10f);

		UnityEngine.UIElements.Toggle hideSliderBool = new Toggle("Hide Slider!");
		root.Add(hideSliderBool);

		var label = new Label();
		label.text = 0.ToString();
		root.Add(label);

		SliderInt slider = new SliderInt();
		root.Add(slider); // Add slider as a child of root.
		slider.RegisterCallback<ChangeEvent<int>>(evt =>
		{
			label.text = evt.newValue.ToString();
		});


		hideSliderBool.RegisterCallback<ChangeEvent<bool>>(evt =>
		{
			// You want to easily hide/show dynamically...
			if (hideSliderBool.value)
			{
				slider.style.display = UnityEngine.UIElements.DisplayStyle.None;
			}
			else
			{
				slider.style.display = UnityEngine.UIElements.DisplayStyle.Flex;
			}
		});


		// Or, once you're done with this element. Make it go away.
		Button removeSliderButton = new Button(() => slider.RemoveFromHierarchy());
		removeSliderButton.text = "Remove Slider?";
		root.Add(removeSliderButton);


		// Or, once you're done with this element. Make it go away.
		//slider.RemoveFromHierarchy();
	}
}