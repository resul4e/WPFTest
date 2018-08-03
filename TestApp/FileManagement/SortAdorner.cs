using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using TestApp.Annotations;

namespace TestApp.FileManagement
{
	/// <summary>
	/// Sorts a column of a listview
	/// </summary>
	public class SortAdorner : Adorner
	{
		#region Public Properties

		public ListSortDirection SortDirection { get; }

		#endregion

		#region Ctor

		public SortAdorner([NotNull] UIElement _adornedElement, ListSortDirection _sortDir) : base(_adornedElement)
		{
			SortDirection = _sortDir;
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// renders a black arrow pointing up or down depending on the sort direction
		/// </summary>
		/// <param name="_drawingContext"></param>
		protected override void OnRender(DrawingContext _drawingContext)
		{
			base.OnRender(_drawingContext);

			if (AdornedElement.RenderSize.Width < 20)
			{
				return;
			}

			TranslateTransform transform = new TranslateTransform
			(
				(AdornedElement.RenderSize.Width / 2.0f) - 3.5f,
				5
			);
			_drawingContext.PushTransform(transform);

			Geometry geometry = sr_ascGeometry;
			if (SortDirection == ListSortDirection.Descending)
			{
				geometry = sr_descGeometry;
			}
			_drawingContext.DrawGeometry(Brushes.Black, null, geometry);

			_drawingContext.Pop();
		}

		#endregion

		#region Private Properties

		private static readonly Geometry sr_ascGeometry =
			Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

		private static readonly Geometry sr_descGeometry =
			Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

		#endregion
	}
}
