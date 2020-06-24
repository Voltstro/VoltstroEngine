using VoltstroEngine.Events;

namespace VoltstroEngine.Core.Layers
{
	public interface ILayer
	{
		public void OnAttach();
		public void OnDetach();

		public void OnUpdate(TimeStep ts);

		public void OnEvent(IEvent e);
	}
}