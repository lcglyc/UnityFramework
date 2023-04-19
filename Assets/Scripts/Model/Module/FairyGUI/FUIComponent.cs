using FairyGUI;

namespace ECSModel
{
	[ObjectSystem]
	public class FUIComponentAwakeSystem : AwakeSystem<FUIComponent>
	{
		public override void Awake(FUIComponent self)
		{
			GRoot.inst.SetContentScaleFactor(750, 1334, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
			self.Root = ComponentFactory.Create<FUI, GObject>(GRoot.inst);
			//UIConfig.defaultFont = "FZCHSJW";
			//FontManager.RegisterFont(FontManager.GetFont("Assets/Resources/Fonts/FZCHSJW"), "方正黑粗宋简");
		}
	}
	
	/// <summary>
	/// 管理所有顶层UI, 顶层UI都是GRoot的孩子
	/// </summary>
	public class FUIComponent: Component
	{
		public FUI Root;
		
		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();
            if(this.Root !=null)
			    this.Root.RemoveChildren();
		}

		public void Add(FUI ui)
		{
			this.Root.Add(ui);
		}
		
		public void Remove(string name)
		{
			this.Root.Remove(name);
		}
		
		public FUI Get(string name)
		{
			FUI ui = this.Root.Get(name);
			return ui;
		}

		public bool Check(string name)
		{
			return this.Root.CheckChild(name);
		}
	}
}