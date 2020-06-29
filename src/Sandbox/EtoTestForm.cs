using Eto.Forms;

namespace Sandbox
{
	public class EtoTestForm : Form
	{
		public Label TestLabel;

		public EtoTestForm()
		{
			Init();
		}

		public void Init()
		{
			Title = "Test";
			Width = 200;
			Height = 200;

			TestLabel = new Label
			{
				Text = "SWAG"
			};

			Content = new StackLayout
			{
				Items = { TestLabel }
			};
		}
	}
}
