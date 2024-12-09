#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace CWJ.Installer
{
	public static class PackageAutoInstaller
	{
		[InitializeOnLoadMethod]
		static void Init()
		{
			PackageManagerExtensions.RegisterExtension(new Ex());
		}

		class Ex : IPackageManagerExtension
		{
			public VisualElement CreateExtensionUI()
			{
				VisualElement ExtentionRoot = new VisualElement();
				VisualElement label = new VisualElement();
				ExtentionRoot.Add(label);
				detail = new Label();
				detail.text = "test";
				label.Add(detail);


				VisualElement buttons = new VisualElement();
				ExtentionRoot.Add(buttons);

				buttons.style.flexDirection = FlexDirection.Row;
				buttons.style.flexWrap = Wrap.Wrap;

				const int width = 160;

				openFolder = new Button();
				openFolder.text = "Open Cache Folder";
				openFolder.style.width = width;
				buttons.Add(openFolder);

				opengit = new Button();
				opengit.text = "Open Git Link";
				opengit.clicked += Opengit_clicked;
				opengit.style.width = width;
				buttons.Add(opengit);
				return ExtentionRoot;
			}

			private void Opengit_clicked()
			{
				if (current?.source == PackageSource.Git)
				{
					var url = current.GetType().GetField("m_ProjectDependenciesEntry",
					                                     BindingFlags.NonPublic | BindingFlags.Instance)
					                 .GetValue(current) as string;
					Debug.Log($"OPEN LINK：{url}");
					Application.OpenURL(url);
				}
			}

			public PackageInfo current = null;
			private Button openFolder;
			private Button opengit;
			private Label detail;

			public void OnPackageSelectionChange(PackageInfo packageInfo)
			{
				//packageInfo It is always null and should be a bug.
				current = packageInfo;
				bool isGit = current?.source == PackageSource.Git;

				detail.text = $"[Git : {isGit}]";
				if (current != null && current.name == "com.cwj.unitydevtool.installer")
				{
					Debug.LogError("installed " + packageInfo.displayName);
					// Debug.Log(current.displayName + "    " + StringUtil.ToStringReflection(current));
				}

				opengit.SetEnabled(isGit);
			}

			public void OnPackageAddedOrUpdated(PackageInfo packageInfo)
			{
			}

			public void OnPackageRemoved(PackageInfo packageInfo)
			{
			}
		}
	}
}

#endif
