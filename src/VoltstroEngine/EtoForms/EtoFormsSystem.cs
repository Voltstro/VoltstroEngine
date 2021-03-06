﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Eto.Forms;
using VoltstroEngine.Core.Logging;
using VoltstroEngine.DebugTools;

namespace VoltstroEngine.EtoForms
{
	public static class EtoFormsSystem
	{
		private static Application app;
		private static List<Form> etoForms;

		/// <summary>
		/// Sets up Eto.Forms
		/// </summary>
		internal static void Init()
		{
			ProfilerTimer.Profile((() =>
			{
				etoForms = new List<Form>();

				Thread appThread = new Thread(() =>
				{
					try
					{
						app = new Application();
						app.Run();
					}
					catch (Exception e)
					{
						Debug.Assert(false, $"An error occured in the Eto.Forms system!\n{e}");
#if !DEBUG
						Logger.Error("An error occured in the Eto.Forms system!\n{@Message}", e);
#endif
					}
				});
				appThread.SetApartmentState(ApartmentState.STA);
				appThread.Start();
			}));
		}

		/// <summary>
		/// Shuts down the Eto.Forms system
		/// </summary>
		internal static void Shutdown()
		{
			ProfilerTimer.Profile(() =>
			{
				// ReSharper disable once ForCanBeConvertedToForeach
				for (int i = 0; i < etoForms.Count; i++)
				{
					DestroyForm(etoForms[i]);
				}

				app.Quit();
				app.Dispose();

				etoForms.Clear();
			});
		}

		/// <summary>
		/// Adds a form to be displayed
		/// </summary>
		/// <param name="form"></param>
		/// <param name="dontShow"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void AddForm(Form form, bool dontShow = false)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form), "Form cannot be null!");

			ProfilerTimer.Profile(() =>
			{
				etoForms.Add(form);

				if(!dontShow)
					form.Show();
			});
		}

		/// <summary>
		/// Closes and destroys a form from the system
		/// </summary>
		/// <param name="form"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void DestroyForm(Form form)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form), "Form cannot be null!");

			etoForms.Remove(form);
			form.Close();
			form.Dispose();
		}
	}
}