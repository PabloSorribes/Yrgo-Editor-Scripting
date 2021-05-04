namespace Paalo.UnityMiscTools
{
	public class CurrentPackageConstants
	{
		private const string paaloMenuPath = "Paalo/";
		private const string currentPackageName = "Misc Tools/";
		private const string examplesPath = "Examples/";

		/// <summary>
		/// To make the menu be at the top of the GameObject-menu and the first option in the hierarchy.
		/// </summary>
		public const int packageMenuIndexPosition = -100;

		/// <summary>
		/// Add the toolName after this path eg:
		/// <para></para>
		/// menuPath = <see cref="CurrentPackageConstants.packageRightClickMenuPath"/> + toolName;
		/// </summary>
		public const string packageRightClickMenuPath = "GameObject/" + paaloMenuPath + currentPackageName; //+ toolName;

		/// <summary>
		/// Add the toolName after this path eg:
		/// <para></para>
		/// menuPath = <see cref="CurrentPackageConstants.packageWindowMenuPath"/> + toolName;
		/// </summary>
		public const string packageWindowMenuPath = "Window/" + paaloMenuPath + currentPackageName; //+ toolName;

		public const string packageExamplesMenuPath = "Window/" + paaloMenuPath + currentPackageName + examplesPath; //+ toolName;


		//Example Implementation:
		#region ToolName and SetupWindow
		//private const string toolName = "Find Directory Of Script";

		//[MenuItem(CurrentPackageConstants.packageRightClickMenuPath + toolName, false, CurrentPackageConstants.packageMenuIndexPosition)]
		//public static void RightClickMenu() => SetupWindow();

		//[MenuItem(CurrentPackageConstants.packageWindowMenuPath + toolName, false, CurrentPackageConstants.packageMenuIndexPosition)]
		//public static void ToolsMenu() => SetupWindow();

		//public static void SetupWindow()
		//{
		//	var window = GetWindow<FindDirectoryOfScript>(true, toolName, true);
		//	window.minSize = new Vector2(340, 200);
		//	window.maxSize = new Vector2(340, 1024);
		//}
		#endregion ToolName and SetupWindow
	}
}