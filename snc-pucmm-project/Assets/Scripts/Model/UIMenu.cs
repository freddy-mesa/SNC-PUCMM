using System;

namespace SncPucmm.Model
{
	public class UIMenu
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name{ get; set;}

		/// <summary>
		/// Initializes a new instance of the <see cref="UserInterface.UIMenu"/> class.
		/// </summary>
		/// <param name="Name">Name.</param>
		public UIMenu (string Name)
		{
			this.Name = Name;
		}
	}
}

