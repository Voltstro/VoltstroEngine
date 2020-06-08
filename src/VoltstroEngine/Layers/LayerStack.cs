using System.Collections.Generic;
using System.Linq;

namespace VoltstroEngine.Layers
{
	public class LayerStack
	{
		private readonly List<ILayer> layers;
		private int layerInset;

		public LayerStack()
		{
			layers = new List<ILayer>();
			layerInset = 0;
		}

		public void PushLayer(ILayer layer)
		{
			layers.Insert(layerInset, layer);
			layerInset++;
		}

		public void PushOverlay(ILayer layer)
		{
			layers.Add(layer);
		}

		public void PopLayer(ILayer layer)
		{
			ILayer it = layers.FirstOrDefault(x => x.Equals(layer));
			if (it != layers.Last())
			{
				layers.Remove(it);
				layerInset--;
			}
		}

		public void PopOverlay(ILayer layer)
		{
			ILayer it = layers.FirstOrDefault(x => x.Equals(layer));
			if (it != layers.Last())
				layers.Remove(it);
		}

		public List<ILayer> GetLayers()
		{
			return layers;
		}
	}
}