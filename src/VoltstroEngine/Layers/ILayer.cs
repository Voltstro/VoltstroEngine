using VoltstroEngine.Events;

namespace VoltstroEngine.Layers
{
	public interface ILayer
	{
		public void OnAttach();
		public void OnDetach();

		public void OnUpdate();

		public void OnEvent(IEvent e);
	}
}