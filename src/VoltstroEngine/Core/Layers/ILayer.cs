using VoltstroEngine.Events;

namespace VoltstroEngine.Core.Layers
{
	/// <summary>
	/// Base for a layer
	/// </summary>
	public interface ILayer
	{
		/// <summary>
		/// Called when the layer is added
		/// </summary>
		public void OnAttach();

		/// <summary>
		/// Called when the layer is removed
		/// </summary>
		public void OnDetach();

		/// <summary>
		/// Called every frame
		/// </summary>
		/// <param name="ts"></param>
		public void OnUpdate(TimeStep ts);

		/// <summary>
		/// Called when an even happens. E.G: Key pressed
		/// </summary>
		/// <param name="e"></param>
		public void OnEvent(IEvent e);
	}
}